using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VirtualGrid.Headers
{
    public struct GridHeaderNodePatcher
    {
        private readonly GridHeaderNode _node;

        private readonly IReadOnlyList<object> _newKeys;

        private readonly IGridHeaderDecomposer _decomposer;

        private readonly int _offset;

        private readonly IGridHeaderDeltaListener _listener;

        private IReadOnlyList<object> _oldKeys
        {
            get
            {
                return _node.Keys;
            }
        }

        private IReadOnlyDictionary<object, int> _oldKeyMap
        {
            get
            {
                return _node.KeyMap;
            }
        }

        public GridHeaderNodePatcher(GridHeaderNode node, IReadOnlyList<object> newKeys, IGridHeaderDecomposer decomposer, int offset, IGridHeaderDeltaListener listener)
        {
            _node = node;
            _newKeys = newKeys;
            _decomposer = decomposer;
            _offset = offset;
            _listener = listener;
        }

        public void Patch()
        {
            // 参照的に同一のリストを比較しても差分は出ない。
            if (EqualityComparer<IReadOnlyList<object>>.Default.Equals(_oldKeys, _newKeys))
                return;

            _node.BeginPatch();

            var si = 0;
            var ti = 0;
            var gi = _offset;

            // 新しい方の各キーの位置を計算する。
            for (var i = 0; i < _newKeys.Count; i++)
            {
                _node.AddNewIndex(_newKeys[i], i);
            }
            var newKeyMap = _node._newMap;

            // それぞれのリストにおいてキーに重複がないことを表明。
            Debug.Assert(_oldKeyMap.Count == _oldKeys.Count);
            Debug.Assert(newKeyMap.Count == _newKeys.Count);

            while (si < _oldKeys.Count || ti < _newKeys.Count)
            {
                // キーが null でないことを表明。
                Debug.Assert(si == _oldKeys.Count || _oldKeys[si] != null);
                Debug.Assert(ti == _newKeys.Count || _newKeys[ti] != null);

                bool doInsert;
                bool doRemove;

                if (si == _oldKeys.Count)
                {
                    // 古いキーがもうないなら、残りはすべて挿入。
                    doInsert = true;
                    doRemove = false;
                }
                else if (ti == _newKeys.Count)
                {
                    // 新しいキーがもうないなら、残りはすべて削除。
                    doInsert = false;
                    doRemove = true;
                }
                else
                {
                    // 次のキーが一致するならマッチ。新しい方を分解して内部の差分を取る。
                    if (EqualityComparer<object>.Default.Equals(_oldKeys[si], _newKeys[ti]))
                    {
                        var childNode = _node.Children[si];

                        var decompositionOpt = _decomposer.Decompose(_newKeys[ti]);
                        if (!decompositionOpt.HasValue)
                        {
                            // リーフノード
                            childNode.BeginPatch();
                            childNode.SetNewSpan(1);
                            childNode.EndPatch();
                        }
                        else if (decompositionOpt.Value.IsDirty)
                        {
                            new GridHeaderNodePatcher(
                                childNode,
                                decompositionOpt.Value.ElementKeys,
                                _decomposer,
                                gi,
                                _listener
                            ).Patch();
                        }

                        _node.AddNewNode(_oldKeys[si], childNode);
                        si++;
                        ti++;
                        gi += childNode.Span;
                        continue;
                    }

                    // 次の新しいキーが古い方にも含まれているか、
                    // 次の古いキーが新しい方にも含まれているか、を調べる。
                    int sj;
                    int tj;
                    if (_oldKeyMap.TryGetValue(_newKeys[ti], out sj) && sj >= si)
                    {
                        if (newKeyMap.TryGetValue(_oldKeys[si], out tj) && tj >= ti)
                        {
                            // 次の新しいキーは古い方の sj 番目にあり、
                            // 次の古いキーは新しい方の tj 番目にある。

                            // 次の新しいキーをマッチさせるために削除しなければいけない要素の個数。
                            // これが大きいなら、次の新しいキーはマッチを諦めて挿入する。
                            var removeCount = sj - si;
                            Debug.Assert(removeCount >= 0);

                            // 次の古いキーをマッチさせるために挿入しなければいけない要素の個数。
                            // これが大きいなら、次の古いキーはマッチを諦めて削除する。
                            var insertCount = tj - ti;
                            Debug.Assert(insertCount >= 0);

                            doInsert = insertCount <= removeCount;
                            doRemove = insertCount >= removeCount;
                        }
                        else
                        {
                            // 次の古いキーが新しい方に含まれていないか、すでにスキップされているなら、マッチしないので削除するしかない。
                            // 次の新しいキーはまだマッチする可能性が残っているので挿入しない。
                            doInsert = false;
                            doRemove = true;
                        }
                    }
                    else
                    {
                        // 次の新しいキーが古い方に含まれていないか、すでにスキップされているなら、マッチしないので挿入するしかない。
                        // 次の古いキーはまだマッチする可能性が残っているので削除しない。
                        doInsert = true;
                        doRemove = false;
                    }
                }

                // 挿入か削除のどちらかは起こるはず。(そうでないと無限ループに陥る。)
                Debug.Assert(doInsert || doRemove);

                if (doInsert)
                {
                    // 新しいノードを構築・追加する。(空のノードに対して Patch するだけ。)
                    var childNode = new GridHeaderNode();
                    {
                        var decompositionOpt = _decomposer.Decompose(_newKeys[ti]);
                        if (!decompositionOpt.HasValue)
                        {
                            // リーフノード
                            childNode.BeginPatch();
                            childNode.SetNewSpan(1);
                            childNode.EndPatch();
                        }
                        else
                        {
                            new GridHeaderNodePatcher(
                                childNode,
                                decompositionOpt.Value.ElementKeys,
                                _decomposer,
                                gi,
                                _listener
                            ).Patch();
                        }

                        _node.AddNewNode(_newKeys[ti], childNode);
                    }

                    _listener.OnInsert(_newKeys[ti], gi);
                    ti++;
                    gi += childNode.Span;
                }

                if (doRemove)
                {
                    Debug.Assert(si < _oldKeys.Count);
                    _listener.OnRemove(_oldKeys[si], gi);
                    si++;
                }
            }

            _node.SetNewSpan(gi - _offset);
            _node.EndPatch();
        }
    }
}

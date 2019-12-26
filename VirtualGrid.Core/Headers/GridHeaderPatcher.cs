using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VirtualGrid.Headers
{
    public struct GridHeaderPatcher
    {
        private readonly GridHeader _oldElement;

        private readonly GridHeaderBuilder _newElement;

        private readonly IGridHeaderDeltaListener _listener;

        private List<GridHeaderNode> _oldKeys
        {
            get
            {
                return _oldElement.Keys;
            }
        }

        private Dictionary<GridHeaderNode, int> _oldKeyMap
        {
            get
            {
                return _oldElement.KeyMap;
            }
        }

        private List<GridHeaderNode> _newKeys
        {
            get
            {
                return _newElement.Keys;
            }
        }

        private Dictionary<GridHeaderNode, int> _newKeyMap
        {
            get
            {
                return _newElement.KeyMap;
            }
        }

        public GridHeaderPatcher(GridHeader oldElement, GridHeaderBuilder newElement, IGridHeaderDeltaListener listener)
        {
            _oldElement = oldElement;
            _newElement = newElement;
            _listener = listener;
        }

        public int Patch(int offset)
        {
            // 参照的に同一のリストを比較しても差分は出ない。
            if (EqualityComparer<IReadOnlyList<GridHeaderNode>>.Default.Equals(_oldKeys, _newKeys))
                return _oldElement.TotalCount;

            Debug.Assert(_oldKeyMap.Count == _oldKeys.Count);
            Debug.Assert(_newKeyMap.Count == _newKeys.Count);

            var si = 0;
            var ti = 0;
            var count = 0;

            while (si < _oldKeys.Count || ti < _newKeys.Count)
            {
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
                    // 次のキーが一致するならマッチ。
                    if (EqualityComparer<GridHeaderNode>.Default.Equals(_oldKeys[si], _newKeys[ti]))
                    {
                        // FIXME: newKeys を使わない？
                        var node = _oldKeys[si];
                        node.Patch(offset + count);
                        count += node.TotalCount;

                        si++;
                        ti++;
                        continue;
                    }

                    // 次の新しいキーが古い方にも含まれているか、
                    // 次の古いキーが新しい方にも含まれているか、を調べる。
                    int sj;
                    int tj;
                    if (_oldKeyMap.TryGetValue(_newKeys[ti], out sj) && sj >= si)
                    {
                        if (_newKeyMap.TryGetValue(_oldKeys[si], out tj) && tj >= ti)
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
                    var node = _newKeys[ti].Create(offset + count);
                    if (node.IsLeaf)
                    {
                        _listener.OnInsert(node.ElementKey, offset + count);;
                    }
                    count += node.TotalCount;

                    ti++;
                }

                if (doRemove)
                {
                    Debug.Assert(si < _oldKeys.Count);
                    var node = _oldKeys[si];
                    node.Destroy(offset + count);
                    if (node.IsLeaf)
                    {
                        _listener.OnRemove(offset + count);
                    }
                    count += node.TotalCount;

                    si++;
                }
            }

            return count;
        }
    }
}

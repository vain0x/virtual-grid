using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Headers
{
    /// <summary>
    /// グリッドのローヘッダーあるいはカラムヘッダーのレイアウトを記録したツリーのノード。
    /// </summary>
    public sealed class GridHeaderNode
    {
        private List<object> _oldKeys =
            new List<object>();

        private List<GridHeaderNode> _oldNodes =
            new List<GridHeaderNode>();

        private Dictionary<object, int> _oldMap =
            new Dictionary<object, int>();

        private int _oldSpan = 0;

        private List<object> _newKeys =
            new List<object>();

        private List<GridHeaderNode> _newNodes =
            new List<GridHeaderNode>();

        internal Dictionary<object, int> _newMap =
            new Dictionary<object, int>();

        private int _newSpan = 0;

        private bool _isPatching = false;

        public IReadOnlyList<object> Keys
        {
            get
            {
                return _oldKeys;
            }
        }

        public IReadOnlyList<GridHeaderNode> Children
        {
            get
            {
                return _oldNodes;
            }
        }

        public IReadOnlyDictionary<object, int> KeyMap
        {
            get
            {
                return _oldMap;
            }
        }

        public int Count
        {
            get
            {
                return _oldKeys.Count;
            }
        }

        public int Span
        {
            get
            {
                return _oldSpan;
            }
        }

        public bool TryGetNode(object elementKey, out GridHeaderNode child)
        {
            int index;
            if (!_oldMap.TryGetValue(elementKey, out index))
            {
                child = null;
                return false;
            }

            child = _oldNodes[index];
            return true;
        }

        public IEnumerable<object> Hit(object elementKey, int index)
        {
            if (Children.Count == 0 && 0 <= index && index < Span)
            {
                if (elementKey != null)
                    yield return elementKey;
            }
            else
            {
                for (var i = 0; i < Count; i++)
                {
                    foreach (var hit in Children[i].Hit(Keys[i], index))
                    {
                        yield return hit;
                    }

                    index -= Children[i].Span;
                    if (index < 0)
                        yield break;
                }
            }
        }

        internal void BeginPatch()
        {
            Debug.Assert(!_isPatching);
            _isPatching = true;

            Debug.Assert(_newKeys.Count == 0);
            Debug.Assert(_newNodes.Count == 0);
            Debug.Assert(_newMap.Count == 0);
            Debug.Assert(_newSpan == 0);
        }

        internal void EndPatch()
        {
            Debug.Assert(_isPatching);
            _isPatching = false;

            Debug.Assert(_newKeys.Count == _newNodes.Count);
            Debug.Assert(_newKeys.Count == _newMap.Count);

            Swap(ref _oldKeys, ref _newKeys);
            Swap(ref _oldNodes, ref _newNodes);
            Swap(ref _oldMap, ref _newMap);
            Swap(ref _oldSpan, ref _newSpan);

            _newKeys.Clear();
            _newNodes.Clear();
            _newMap.Clear();
            _newSpan = 0;
        }

        internal void AddNewIndex(object elementKey, int index)
        {
            Debug.Assert(_isPatching);

            // AddNewNode より前にだけ呼べる。
            Debug.Assert(_newKeys.Count == 0);

            // キーが重複してはいけない。
            Debug.Assert(!_newMap.ContainsKey(elementKey));

            _newMap.Add(elementKey, index);
        }

        internal void AddNewNode(object elementKey, GridHeaderNode node)
        {
            Debug.Assert(_isPatching);

            Debug.Assert(elementKey != null);
            Debug.Assert(node != null);

            // _newMap のインデックスが正しいことを表明。
            Debug.Assert(_newMap.ContainsKey(elementKey) && _newMap[elementKey] == _newKeys.Count);

            _newKeys.Add(elementKey);
            _newNodes.Add(node);

            Debug.Assert(_oldKeys.Count == _oldNodes.Count);
        }

        internal void SetNewSpan(int newSpan)
        {
            Debug.Assert(_isPatching);
            Debug.Assert(_newSpan == 0);
            Debug.Assert(newSpan >= _newMap.Count);

            _newSpan = newSpan;
        }

        private static void Swap<T>(ref T first, ref T second)
        {
            var t = first;
            first = second;
            second = t;
        }
    }
}

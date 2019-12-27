using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VirtualGrid.Headers
{
    public sealed class GridHeaderBuilder<TListener>
        : IGridHeaderNode
        where TListener : IGridHeaderDeltaListener
    {
        private GridHeader<TListener> _inner;

        internal List<GridHeaderNode> Keys =
            new List<GridHeaderNode>();

        internal Dictionary<GridHeaderNode, int> KeyMap =
            new Dictionary<GridHeaderNode, int>();

        public GridHeaderBuilder(GridHeader<TListener> parent)
        {
            _inner = parent;
        }

        public object ElementKey
        {
            get
            {
                return _inner.ElementKey;
            }
        }

        public int Offset
        {
            get
            {
                return _inner.Offset;
            }
        }

        public int TotalCount
        {
            get
            {
                return _inner.TotalCount;
            }
        }

        private void AddCore(GridHeaderNode elementKey)
        {
            // キーが重複してはいけない。
            Debug.Assert(!KeyMap.ContainsKey(elementKey));

            var index = Keys.Count;
            Keys.Add(elementKey);
            KeyMap.Add(elementKey, index);
        }

        public void Add(object elementKey)
        {
            AddCore(GridHeaderNode.NewLeaf(elementKey));
        }

        public void AddNode(IGridHeaderNode childNode)
        {
            AddCore(GridHeaderNode.NewNode(childNode));
        }

        public GridHeaderList AddList(object elementKey)
        {
            var list = new GridHeaderList(elementKey, _inner.Listener);
            AddNode(list);
            return list;
        }

        public void Clear()
        {
            Keys.Clear();
            KeyMap.Clear();
        }

        public void SetOffset(int offset)
        {
            _inner.SetOffset(offset);
        }

        public GridHeaderNode Create(int offset)
        {
            return _inner.Create(offset);
        }

        public void Destroy(int offset)
        {
            _inner.Destroy(offset);
        }

        public void Patch(int offset)
        {
            _inner.Patch(offset);
        }

        public GridHeaderHitResult? Hit(int index)
        {
            return _inner.Hit(index);
        }
    }
}

using System.Collections.Generic;

namespace VirtualGrid.Headers
{
    public sealed class GridHeaderListBuilder
        : IGridHeaderNode
    {
        private GridHeaderList _inner;

        internal List<GridHeaderDelta> _diff =
            new List<GridHeaderDelta>();

        public GridHeaderListBuilder(GridHeaderList inner)
        {
            _inner = inner;
        }

        public object ElementKey
        {
            get
            {
                return _inner.ElementKey;
            }
        }

        public bool IsDirty
        {
            get
            {
                return _inner.IsDirty;
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

        public void Insert(int index, object elementKey)
        {
            _diff.Add(GridHeaderDelta.NewInsert(index, elementKey));
        }

        public void RemoveAt(int index)
        {
            _diff.Add(GridHeaderDelta.NewRemove(index));
        }

        public bool Remove(object elementKey)
        {
            var indexOpt = _inner.TryGetIndex(elementKey);
            if (!indexOpt.HasValue)
                return false;

            RemoveAt(indexOpt.Value);
            return true;
        }

        public void Clear()
        {
            _diff.Clear();
        }

        public void SetOffset(int offset)
        {
            _inner.SetOffset(offset);
        }

        public GridHeaderNode Create(int offset)
        {
            Patch(offset);
            return GridHeaderNode.NewNode(_inner);
        }

        public void Destroy(int offset)
        {
            _inner.Destroy(offset);
        }

        public void Patch()
        {
            Patch(Offset);
        }

        public void Patch(int offset)
        {
            _inner.Patch(offset);
            Clear();
        }
    }
}

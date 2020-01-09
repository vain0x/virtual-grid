using System;
using System.Diagnostics;

namespace VirtualGrid.Headers
{
    public interface IGridHeaderNode
    {
        object ElementKey { get; }

        int Offset { get; }

        int TotalCount { get; }

        GridHeaderHitResult? Hit(int index);

        void SetOffset(int offset);

        GridHeaderNode Create(int offset);

        void Destroy(int offset);

        void Patch(int offset);
    }

    public struct GridHeaderNode
        : IGridHeaderNode
    {
        public IGridHeaderNode _innerOpt;

        private GridHeaderNode(object elementKey, IGridHeaderNode innerOpt)
        {
            ElementKey = elementKey;
            _innerOpt = innerOpt;
        }

        public static GridHeaderNode NewLeaf(object elementKey)
        {
            Debug.Assert(elementKey != null);
            return new GridHeaderNode(elementKey, null);
        }

        public static GridHeaderNode NewNode(IGridHeaderNode inner)
        {
            return new GridHeaderNode(inner.ElementKey, inner);
        }

        public object ElementKey { get; private set; }

        public bool IsLeaf
        {
            get
            {
                return _innerOpt == null;
            }
        }

        public int Offset
        {
            get
            {
                return _innerOpt != null ? _innerOpt.Offset : 0;
            }
        }

        public int TotalCount
        {
            get
            {
                return _innerOpt != null ? _innerOpt.TotalCount : 1;
            }
        }

        public GridHeaderHitResult? Hit(int index)
        {
            if (_innerOpt != null)
                return _innerOpt.Hit(index);

            if (index != 0)
                return null;

            return GridHeaderHitResult.Create(this, 0);
        }

        public void SetOffset(int offset)
        {
            if (_innerOpt != null)
            {
                _innerOpt.SetOffset(offset);
            }
        }

        public GridHeaderNode Create(int offset)
        {
            if (_innerOpt == null)
                return this;

            return _innerOpt.Create(offset);
        }

        public void Patch(int offset)
        {
            if (_innerOpt != null)
            {
                _innerOpt.Patch(offset);
            }
        }

        public void Destroy(int offset)
        {
            if (_innerOpt != null)
            {
                _innerOpt.Destroy(offset);
            }
        }
    }
}

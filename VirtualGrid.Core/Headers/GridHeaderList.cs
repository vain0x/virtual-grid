using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Headers
{
    public sealed class GridHeaderList
        : IGridHeaderNode
    {
        private GridHeaderParent _parent;

        public object ElementKey { get; private set; }

        private IElementKeyInterner _interner;

        private IGridHeaderDeltaListener _listener;

        public bool IsDirty { get; private set; }

        public int Offset { get; private set; }

        public int TotalCount { get; private set; }

        private GridHeaderListBuilder _builder;

        public GridHeaderList(object elementKey, IElementKeyInterner interner, IGridHeaderDeltaListener listener)
        {
            ElementKey = elementKey;
            _interner = interner;
            _listener = listener;

            _builder = new GridHeaderListBuilder(this);
        }

        public int? TryGetIndex(object elementKey)
        {
            if (IsDirty)
            {
                _parent.UpdateChildren();
            }

            var indexOpt = _interner.TryGetIndex(elementKey);
            if (!(indexOpt.HasValue && Offset <= indexOpt.Value && indexOpt.Value < Offset + TotalCount))
                return null;

            return indexOpt.Value - Offset;
        }

        public object TryGetKey(int index)
        {
            if (IsDirty)
            {
                _parent.UpdateChildren();
            }

            if (!(0 <= index && index < TotalCount))
                return null;

            return _interner.TryGetKey(index + Offset);
        }

        public GridHeaderListBuilder GetBuilder()
        {
            return _builder;
        }

        public void SetOffset(int offset)
        {
            if (Offset != offset)
            {
                Offset = offset;
                IsDirty = false;
            }
        }

        public GridHeaderNode Create(int offset)
        {
            return GridHeaderNode.NewNode(this);
        }

        public void Destroy(int offset)
        {
            for (var i = TotalCount; i >= 1;)
            {
                i--;
                _listener.OnRemove(i);
            }
        }

        public void Patch(int offset)
        {
            var oldCount = TotalCount;

            foreach (var delta in _builder._diff)
            {
                switch (delta.Kind)
                {
                    case GridHeaderDeltaKind.Insert:
                        TotalCount++;
                        _listener.OnInsert(delta.ElementKey, offset + delta.Index);
                        continue;

                    case GridHeaderDeltaKind.Remove:
                        TotalCount--;
                        _listener.OnRemove(offset + delta.Index);
                        continue;

                    default:
                        throw new Exception("Unknown GridHeaderDeltaKind");
                }
            }

            _parent.OnChildChanged(TotalCount - oldCount);
        }
    }
}

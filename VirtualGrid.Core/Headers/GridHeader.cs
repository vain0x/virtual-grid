using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Headers
{
    public sealed class GridHeader
        : IGridHeaderParent
        , IGridHeaderNode
    {
        internal List<GridHeaderNode> Keys =
            new List<GridHeaderNode>();

        internal Dictionary<GridHeaderNode, int> KeyMap =
            new Dictionary<GridHeaderNode, int>();

        internal GridHeaderBuilder _builder;

        internal readonly IElementKeyInterner Interner;

        internal readonly IGridHeaderDeltaListener Listener;

        public object ElementKey { get; private set; }

        public bool IsDirty { get; private set; }

        public int Offset { get; private set; }

        public int TotalCount { get; private set; }

        public GridHeader(object elementKey, IElementKeyInterner interner, IGridHeaderDeltaListener listener)
        {
            ElementKey = elementKey;
            Interner = interner;
            Listener = listener;

            _builder = new GridHeaderBuilder(this);
        }

        public GridHeaderBuilder GetBuilder()
        {
            return _builder ?? new GridHeaderBuilder(this);
        }

        internal void Clear()
        {
            IsDirty = false;
            Offset = 0;
            TotalCount = 0;
            Keys.Clear();
            KeyMap.Clear();
        }

        public void SetOffset(int offset)
        {
            if (Offset != offset)
            {
                Offset = offset;
                IsDirty = true;
            }
        }

        public GridHeaderNode Create(int offset)
        {
            Offset = offset;
            return GridHeaderNode.NewNode(this);
        }

        public void Destroy(int offset)
        {
            UpdateChildren();

            foreach (var key in Keys)
            {
                offset += key.TotalCount;
            }

            for (var i = Keys.Count; i >= 1;)
            {
                i--;
                Keys[i].Destroy(offset);
            }
        }

        public void Patch(int offset)
        {
            new GridHeaderPatcher(this, _builder, Listener).Patch(offset);

            Offset = offset;
            IsDirty = false;
            Swap(ref Keys, ref _builder.Keys);
            Swap(ref KeyMap, ref _builder.KeyMap);
        }

        private static void Swap<T>(ref T first, ref T second)
        {
            var t = first;
            first = second;
            second = t;
        }

        public void OnChildChanged(int countChange)
        {
            TotalCount += countChange;
            Debug.Assert(TotalCount >= 0);

            IsDirty = true;
        }

        public void UpdateChildren()
        {
            if (!IsDirty)
                return;

            var offset = 0;

            foreach (var key in Keys)
            {
                key.SetOffset(offset);

                offset += key.TotalCount;
            }

            IsDirty = false;
        }
    }
}

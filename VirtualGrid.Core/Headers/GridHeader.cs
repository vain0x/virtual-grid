using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Headers
{
    public sealed class GridHeader<TListener>
        : IGridHeaderParent
        , IGridHeaderNode
        where TListener : IGridHeaderDeltaListener
    {
        internal List<GridHeaderNode> Keys =
            new List<GridHeaderNode>();

        internal Dictionary<GridHeaderNode, int> KeyMap =
            new Dictionary<GridHeaderNode, int>();

        internal GridHeaderBuilder<TListener> _builder;

        internal TListener Listener;

        public object ElementKey { get; private set; }

        public bool IsDirty { get; private set; }

        public int Offset
        {
            get
            {
                // NOTE: 必ずルートであるため。
                return 0;
            }
        }

        public int TotalCount { get; private set; }

        public GridHeader(object elementKey, TListener listener)
        {
            ElementKey = elementKey;
            Listener = listener;

            _builder = new GridHeaderBuilder<TListener>(this);
        }

        public GridHeaderBuilder<TListener> GetBuilder()
        {
            return _builder;
        }

        public GridHeaderHitResult? Hit(int index)
        {
            UpdateChildren();

            index -= Offset;

            foreach (var key in Keys)
            {
                if (index < 0)
                    break;

                if (index < key.TotalCount)
                    return GridHeaderHitResult.Create(key, index);

                index -= key.TotalCount;
            }

            return null;
        }

        internal void Clear()
        {
            IsDirty = false;
            TotalCount = 0;
            Keys.Clear();
            KeyMap.Clear();
        }

        public void SetOffset(int offset)
        {
            // ルートであるため、オフセットが変更されることはない。
            Debug.Assert(offset == 0);
        }

        public GridHeaderNode Create(int offset)
        {
            Debug.Assert(offset == 0);
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
            Debug.Assert(offset == 0);

            new GridHeaderPatcher<TListener>(this, _builder, Listener).Patch(offset);

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

            var offset = Offset;

            foreach (var key in Keys)
            {
                key.SetOffset(offset);

                offset += key.TotalCount;
            }

            IsDirty = false;
        }
    }
}

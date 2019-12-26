using System;
using System.Diagnostics;
using VirtualGrid.Layouts;

namespace VirtualGrid.Rendering
{
    public struct GridCellBuilder<TProvider>
        : IGridLayoutBuilder
    {
        public GridElementKey ElementKey;

        public TProvider Provider;

        public GridCellBuilder(GridElementKey elementKey, TProvider provider)
        {
            ElementKey = elementKey;
            Provider = provider;
        }

        IGridLayoutNode IGridLayoutBuilder.ToGridLayoutNode()
        {
            throw new NotImplementedException();
        }
    }
}

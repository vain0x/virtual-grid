using System;
using VirtualGrid.Layouts;

namespace VirtualGrid.Rendering
{
    // FIXME: インターフェイスにする
    public sealed class IGridCellBuilder<TProvider>
        : IGridLayoutBuilder
    {
        public object ElementKey;

        internal GridRenderContext<TProvider> Context;

        public TProvider Provider
        {
            get
            {
                return Context.Provider;
            }
        }

        public IGridCellBuilder(object elementKey, GridRenderContext<TProvider> context)
        {
            ElementKey = elementKey;
            Context = context;
        }

        IGridLayoutNode IGridLayoutBuilder.ToGridLayoutNode()
        {
            return new CellGridLayoutNode(ElementKey);
        }
    }
}

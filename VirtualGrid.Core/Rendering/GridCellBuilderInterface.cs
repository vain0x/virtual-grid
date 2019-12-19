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

        private void SetAttribute(string attribute, object value)
        {
            Context.SetElementAttribute(ElementKey, attribute, value);
        }

        public IGridCellBuilder<TProvider> SetValue(object value)
        {
            SetAttribute("A_VALUE", value);
            return this;
        }

        IGridLayoutNode IGridLayoutBuilder.ToGridLayoutNode()
        {
            return new CellGridLayoutNode(ElementKey);
        }
    }
}

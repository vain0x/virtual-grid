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

        public IGridCellBuilder<TProvider> OnClick(Action action)
        {
            SetAttribute("A_ON_CLICK", action);
            return this;
        }

        public IGridCellBuilder<TProvider> OnTextChanged(Action<string> action)
        {
            SetAttribute("A_ON_CHANGED", new Action<object>(value =>
            {
                var text = value as string;
                if (text != null || value == null)
                {
                    action(text);
                }
            }));
            return this;
        }

        public IGridCellBuilder<TProvider> OnCheckChanged(Action<bool> action)
        {
            SetAttribute("A_ON_CHANGED", new Action<object>(value =>
            {
                var isChecked = value as bool?;
                if (isChecked != null)
                {
                    action(isChecked.Value);
                }
            }));
            return this;
        }

        IGridLayoutNode IGridLayoutBuilder.ToGridLayoutNode()
        {
            return new CellGridLayoutNode(ElementKey);
        }
    }
}

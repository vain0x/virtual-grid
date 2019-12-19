using System;
using VirtualGrid.Layouts;

namespace VirtualGrid.Rendering
{
    // FIXME: インターフェイスにする
    public sealed class IGridCellBuilder
        : IGridLayoutBuilder
    {
        internal object ElementKey;

        internal GridRenderContext Context;

        public IGridCellBuilder(object elementKey, GridRenderContext context)
        {
            ElementKey = elementKey;
            Context = context;
        }

        internal string GetText()
        {
            return Context.GetElementAttribute(ElementKey, "A_VALUE", "");
        }

        private void SetAttribute(string attribute, object value)
        {
            Context.SetElementAttribute(ElementKey, attribute, value);
        }

        public IGridCellBuilder SetReadOnly(bool isReadOnly)
        {
            SetAttribute("A_READ_ONLY", isReadOnly);
            return this;
        }

        public IGridCellBuilder SetText(string text)
        {
            SetAttribute("A_VALUE", text);
            return this;
        }

        public IGridCellBuilder SetValue(object value)
        {
            SetAttribute("A_VALUE", value);
            return this;
        }

        public IGridCellBuilder SetIsChecked(bool isChecked)
        {
            SetAttribute("A_IS_CHECKED", isChecked);
            return this;
        }

        public IGridCellBuilder OnClick(Action action)
        {
            SetAttribute("A_ON_CLICK", action);
            return this;
        }

        public IGridCellBuilder OnTextChanged(Action<string> action)
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

        public IGridCellBuilder OnCheckChanged(Action<bool> action)
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

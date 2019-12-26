using System;
using VirtualGrid.Rendering;
using P = VirtualGrid.WinFormsDemo.AttributeBuilder;

namespace VirtualGrid.WinFormsDemo
{
    public static class GridCellBuilderExtensions
    {
        public static GridCellBuilder<P> SetIsChecked(this GridCellBuilder<P> self, bool value)
        {
            self.Provider.IsCheckedAttribute.SetValue(self.ElementKey, value);
            return self;
        }

        public static GridCellBuilder<P> OnCheckChanged(this GridCellBuilder<P> self, Action<bool> action)
        {
            self.Provider.OnCheckChangedAttribute.SetValue(self.ElementKey, action);
            return self;
        }

        public static GridCellBuilder<P> OnClick(this GridCellBuilder<P> self, Action action)
        {
            self.Provider.OnClickAttribute.SetValue(self.ElementKey, action);
            return self;
        }

        public static GridCellBuilder<P> OnTextChanged(this GridCellBuilder<P> self, Action<string> action)
        {
            self.Provider.OnTextChangedAttribute.SetValue(self.ElementKey, action);
            return self;
        }

        public static GridCellBuilder<P> SetReadOnly(this GridCellBuilder<P> self, bool value)
        {
            self.Provider.ReadOnlyAttribute.SetValue(self.ElementKey, value);
            return self;
        }

        public static GridCellBuilder<P> SetText(this GridCellBuilder<P> self, string text)
        {
            self.Provider.TextAttribute.SetValue(self.ElementKey, text);
            return self;
        }
    }
}

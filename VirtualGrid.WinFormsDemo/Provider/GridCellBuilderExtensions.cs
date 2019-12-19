using System;
using VirtualGrid.Rendering;
using P = VirtualGrid.WinFormsDemo.DataGridViewGridProvider;

namespace VirtualGrid.WinFormsDemo
{
    public static class GridCellBuilderExtensions
    {
        public static IGridCellBuilder<P> SetIsChecked(this IGridCellBuilder<P> self, bool value)
        {
            self.Provider.IsCheckedAttribute.SetValue(self.ElementKey, value);
            return self;
        }

        public static IGridCellBuilder<P> OnClick(this IGridCellBuilder<P> self, Action action)
        {
            self.Provider.OnClickAttribute.SetValue(self.ElementKey, action);
            return self;
        }

        public static IGridCellBuilder<P> SetReadOnly(this IGridCellBuilder<P> self, bool value)
        {
            self.Provider.ReadOnlyAttribute.SetValue(self.ElementKey, value);
            return self;
        }

        public static IGridCellBuilder<P> SetText(this IGridCellBuilder<P> self, string text)
        {
            self.Provider.TextAttribute.SetValue(self.ElementKey, text);
            return self;
        }
    }
}

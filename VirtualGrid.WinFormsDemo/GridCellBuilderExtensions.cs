using VirtualGrid.Rendering;
using P = VirtualGrid.WinFormsDemo.DataGridViewGridProvider;

namespace VirtualGrid.WinFormsDemo
{
    public static class GridCellBuilderExtensions
    {
        public static IGridCellBuilder<P> SetText(this IGridCellBuilder<P> self, string text)
        {
            self.Provider.TextAttribute.SetValue(self.ElementKey, text);
            return self;
        }
    }
}

namespace VirtualGrid.Rendering
{
    public static class GridCellAdderExtensions
    {
        public static IGridCellBuilder AddText(this IGridCellAdder self, string text)
        {
            return self.AddCell().SetText(text).SetReadOnly(true);
        }

        public static IGridCellBuilder AddEdit(this IGridCellAdder self, string text)
        {
            return self.AddCell().SetText(text);
        }

        public static IGridCellBuilder AddButton(this IGridCellAdder self, string text)
        {
            // FIXME: 実装
            return self.AddText(text);
        }
    }
}

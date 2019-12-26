using VirtualGrid.Rendering;
using P = VirtualGrid.WinFormsDemo.AttributeBuilder;

namespace VirtualGrid.WinFormsDemo
{
    public static class GridCellAdderExtensions
    {
        /// <summary>
        /// 編集不可のテキストセルを追加する。
        /// </summary>
        public static GridCellBuilder<P> AddText(this IGridCellAdder<P> self, string text)
        {
            return self.AddCell().SetText(text).SetReadOnly(true);
        }

        /// <summary>
        /// 編集可能なテキストセルを追加する。
        /// </summary>
        public static GridCellBuilder<P> AddEdit(this IGridCellAdder<P> self, string text)
        {
            return self.AddCell().SetText(text);
        }

        public static GridCellBuilder<P> AddCheckBox(this IGridCellAdder<P> self, bool isChecked)
        {
            return self.AddCell().SetIsChecked(isChecked).SetReadOnly(true);
        }

        public static GridCellBuilder<P> AddButton(this IGridCellAdder<P> self, string text)
        {
            return self.AddText("[" + text + "]");
        }
    }
}

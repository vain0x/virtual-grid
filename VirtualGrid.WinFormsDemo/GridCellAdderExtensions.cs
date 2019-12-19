using VirtualGrid.Rendering;
using P = VirtualGrid.WinFormsDemo.DataGridViewGridProvider;

namespace VirtualGrid.WinFormsDemo
{
    public static class GridCellAdderExtensions
    {
        /// <summary>
        /// 編集不可のテキストセルを追加する。
        /// </summary>
        public static IGridCellBuilder<P> AddText(this IGridCellAdder<P> self, string text)
        {
            return self.AddCell().SetText(text).SetReadOnly(true);
        }

        /// <summary>
        /// 編集可能なテキストセルを追加する。
        /// </summary>
        public static IGridCellBuilder<P> AddEdit(this IGridCellAdder<P> self, string text)
        {
            return self.AddCell().SetText(text);
        }

        public static IGridCellBuilder<P> AddCheckBox(this IGridCellAdder<P> self, bool isChecked)
        {
            return self.AddCell().SetIsChecked(isChecked).SetReadOnly(true);
        }

        public static IGridCellBuilder<P> AddButton(this IGridCellAdder<P> self, string text)
        {
            // FIXME: 実装
            return self.AddText(text);
        }
    }
}

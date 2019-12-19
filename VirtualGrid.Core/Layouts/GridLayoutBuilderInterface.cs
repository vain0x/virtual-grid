namespace VirtualGrid.Layouts
{
    /// <summary>
    /// レイアウトの構築の中間構造
    /// </summary>
    public interface IGridLayoutBuilder
    {
        IGridLayoutNode ToGridLayoutNode();
    }
}

using System;

namespace VirtualGrid.Layouts
{
    /// <summary>
    /// グリッドのピボットの集まり。
    ///
    /// ここでは行または列を代表する識別子をピボットと呼んでいる。
    /// </summary>
    public struct GridPivots
    {
        public readonly object[] RowPivots;

        public readonly object[] ColumnPivots;

        public static readonly GridPivots Empty =
            new GridPivots(Array.Empty<object>(), Array.Empty<object>());

        public GridPivots(object[] rowPivots, object[] columnPivots)
        {
            RowPivots = rowPivots;
            ColumnPivots = columnPivots;
        }
    }
}

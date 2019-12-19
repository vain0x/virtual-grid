using System.Diagnostics;

namespace VirtualGrid.Layouts
{
    [DebuggerDisplay("{AsDebug}")]
    public struct GridLayoutDelta
    {
        public readonly GridLayoutDeltaKind Kind;

        public readonly int Index;

        private GridLayoutDelta(GridLayoutDeltaKind kind, int index)
        {
            Kind = kind;
            Index = index;
        }

        public string AsDebug
        {
            get
            {
                return string.Format("{0}({1})", Kind, Index);
            }
        }

        public static GridLayoutDelta Create(GridLayoutDeltaKind kind, int index)
        {
            return new GridLayoutDelta(kind, index);
        }

        public static GridLayoutDelta NewInsertRow(RowIndex index)
        {
            return new GridLayoutDelta(GridLayoutDeltaKind.InsertRow, index.Row);
        }

        public static GridLayoutDelta NewRemoveRow(RowIndex index)
        {
            return new GridLayoutDelta(GridLayoutDeltaKind.RemoveRow, index.Row);
        }

        public static GridLayoutDelta NewInsertColumn(ColumnIndex index)
        {
            return new GridLayoutDelta(GridLayoutDeltaKind.InsertColumn, index.Column);
        }

        public static GridLayoutDelta NewRemoveColumn(ColumnIndex index)
        {
            return new GridLayoutDelta(GridLayoutDeltaKind.RemoveColumn, index.Column);
        }
    }
}

using System;
using System.Collections.Generic;

namespace VirtualGrid.Rendering
{
    public struct GridCellKey
        : IEquatable<GridCellKey>
    {
        public readonly GridPart GridPart;

        public readonly object RowElementKey;

        public readonly object ColumnElementKey;

        public readonly object CellElementKey;

        public GridCellKey(GridPart gridPart, object rowElementKey, object columnElementKey, object cellElementKey)
        {
            GridPart = gridPart;
            RowElementKey = rowElementKey;
            ColumnElementKey = columnElementKey;
            CellElementKey = cellElementKey;
        }

        public override bool Equals(object obj)
        {
            return obj is GridCellKey key && Equals(key);
        }

        public bool Equals(GridCellKey other)
        {
            return GridPart == other.GridPart &&
                   EqualityComparer<object>.Default.Equals(RowElementKey, other.RowElementKey) &&
                   EqualityComparer<object>.Default.Equals(ColumnElementKey, other.ColumnElementKey) &&
                   EqualityComparer<object>.Default.Equals(CellElementKey, other.CellElementKey);
        }

        public override int GetHashCode()
        {
            var hashCode = -1636306230;
            hashCode = hashCode * -1521134295 + GridPart.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(RowElementKey);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(ColumnElementKey);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(CellElementKey);
            return hashCode;
        }

        public static bool operator ==(GridCellKey left, GridCellKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridCellKey left, GridCellKey right)
        {
            return !(left == right);
        }
    }
}

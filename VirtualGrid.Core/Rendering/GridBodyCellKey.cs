using System;
using System.Collections.Generic;

namespace VirtualGrid.Rendering
{
    public sealed class GridBodyCellKey
        : IEquatable<GridBodyCellKey>
    {
        public readonly object RowElementKey;

        public readonly object ColumnElementKey;

        public GridBodyCellKey(object rowElementKey, object columnElementKey)
        {
            RowElementKey = rowElementKey;
            ColumnElementKey = columnElementKey;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GridBodyCellKey);
        }

        public bool Equals(GridBodyCellKey other)
        {
            return other != null &&
                   EqualityComparer<object>.Default.Equals(RowElementKey, other.RowElementKey) &&
                   EqualityComparer<object>.Default.Equals(ColumnElementKey, other.ColumnElementKey);
        }

        public override int GetHashCode()
        {
            var hashCode = 472338596;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(RowElementKey);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(ColumnElementKey);
            return hashCode;
        }

        public static bool operator ==(GridBodyCellKey left, GridBodyCellKey right)
        {
            return EqualityComparer<GridBodyCellKey>.Default.Equals(left, right);
        }

        public static bool operator !=(GridBodyCellKey left, GridBodyCellKey right)
        {
            return !(left == right);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    [DebuggerDisplay("{AsDebug}")]
    public struct GridVector
        : IEquatable<GridVector>
    {
        public readonly RowIndex Row;

        public readonly ColumnIndex Column;

        private GridVector(RowIndex row, ColumnIndex column)
        {
            Row = row;
            Column = column;
        }

        public static GridVector Create(RowIndex row, ColumnIndex column)
        {
            return new GridVector(row, column);
        }

        public static GridVector From(RowIndex row)
        {
            return Create(row, ColumnIndex.Zero);
        }

        public static GridVector From(ColumnIndex column)
        {
            return Create(RowIndex.Zero, column);
        }

        public static GridVector Zero
        {
            get
            {
                return Create(RowIndex.Zero, ColumnIndex.Zero);
            }
        }

        #region equality

        public bool Equals(GridVector other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return obj is GridVector && Equals((GridVector)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 240067226;
            hashCode = hashCode * -1521134295 + EqualityComparer<RowIndex>.Default.GetHashCode(Row);
            hashCode = hashCode * -1521134295 + EqualityComparer<ColumnIndex>.Default.GetHashCode(Column);
            return hashCode;
        }

        public static bool operator ==(GridVector left, GridVector right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridVector left, GridVector right)
        {
            return !(left == right);
        }

        #endregion

        public static GridVector operator -(GridVector first)
        {
            return (-1) * first;
        }

        public static GridVector operator +(GridVector first, GridVector second)
        {
            return Create(first.Row + second.Row, first.Column + second.Column);
        }

        public static GridVector operator -(GridVector first, GridVector second)
        {
            return Create(first.Row - second.Row, first.Column - second.Column);
        }

        public static GridVector operator *(int first, GridVector second)
        {
            return Create(first * second.Row, first * second.Column);
        }

        public string AsDebug
        {
            get
            {
                return "(" + Row.AsDebug + ", " + Column.AsDebug + ")";
            }
        }

        public GridVector WithRow(RowIndex row)
        {
            return Create(row, Column);
        }

        public GridVector WithColumn(ColumnIndex column)
        {
            return Create(Row, column);
        }

        public GridVector Min(GridVector other)
        {
            return Create(Row.Min(other.Row), Column.Min(other.Column));
        }

        public GridVector Max(GridVector other)
        {
            return Create(Row.Max(other.Row), Column.Max(other.Column));
        }

        public GridRange To(GridVector end)
        {
            return GridRange.Create(this, end);
        }
    }
}

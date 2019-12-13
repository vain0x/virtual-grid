using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    [DebuggerDisplay("{AsDebug}")]
    public struct GridMeasure
        : IEquatable<GridMeasure>
    {
        public readonly RowMeasure Row;

        public readonly ColumnMeasure Column;

        private GridMeasure(RowMeasure row, ColumnMeasure column)
        {
            Row = row;
            Column = column;
        }

        public static GridMeasure Create(RowMeasure row, ColumnMeasure column)
        {
            return new GridMeasure(row, column);
        }

        public static GridMeasure From(GridVector vector)
        {
            return Create(RowMeasure.From(vector.Row), ColumnMeasure.From(vector.Column));
        }

        public static readonly GridMeasure Zero =
            new GridMeasure(RowMeasure.Zero, ColumnMeasure.Zero);

        public static readonly GridMeasure Infinite =
            new GridMeasure(RowMeasure.Infinite, ColumnMeasure.Infinite);

        public string AsDebug
        {
            get
            {
                return "(" + Row.AsDebug + ", " + Column.AsDebug + ")";
            }
        }

        public GridMeasure Reduce(GridMeasure other)
        {
            return Create(Row.Reduce(other.Row), Column.Reduce(other.Column));
        }

        public GridMeasure Reduce(GridVector other)
        {
            return Reduce(From(other));
        }

        public GridMeasure Reduce(RowMeasure row)
        {
            return Create(Row.Reduce(row), Column);
        }

        public GridMeasure Reduce(ColumnMeasure column)
        {
            return Create(Row, Column.Reduce(column));
        }

        public GridMeasure Min(GridMeasure other)
        {
            return Create(Row.Min(other.Row), Column.Min(other.Column));
        }

        public GridMeasure Max(GridMeasure other)
        {
            return Create(Row.Max(other.Row), Column.Max(other.Column));
        }

        public override bool Equals(object obj)
        {
            return obj is GridMeasure measure && Equals(measure);
        }

        public bool Equals(GridMeasure other)
        {
            return Row.Equals(other.Row) &&
                   Column.Equals(other.Column);
        }

        public override int GetHashCode()
        {
            var hashCode = 240067226;
            hashCode = hashCode * -1521134295 + EqualityComparer<RowMeasure>.Default.GetHashCode(Row);
            hashCode = hashCode * -1521134295 + EqualityComparer<ColumnMeasure>.Default.GetHashCode(Column);
            return hashCode;
        }

        public static bool operator ==(GridMeasure left, GridMeasure right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridMeasure left, GridMeasure right)
        {
            return !(left == right);
        }

        public GridVector ToGridVector()
        {
            return GridVector.Create(
                Row.RowOpt ?? RowIndex.Zero,
                Column.ColumnOpt ?? ColumnIndex.Zero
            );
        }
    }
}

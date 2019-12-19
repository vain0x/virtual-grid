using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    /// <summary>
    /// 行番号または行数
    /// </summary>
    [DebuggerDisplay("{AsDebug}")]
    public struct RowIndex
        : IEquatable<RowIndex>
    {
        public readonly int Row;

        private RowIndex(int row)
        {
            Row = row;
        }

        public static readonly RowIndex MaxValue = From(Int32.MaxValue);

        public static RowIndex From(int row)
        {
            return new RowIndex(row);
        }

        public static RowIndex Zero
        {
            get
            {
                return From(0);
            }
        }

        #region equality

        public bool Equals(RowIndex other)
        {
            return Row == other.Row;
        }

        public override bool Equals(object obj)
        {
            return obj is RowIndex && Equals((RowIndex)obj);
        }

        public override int GetHashCode()
        {
            return -343017389 + Row.GetHashCode();
        }

        public static bool operator ==(RowIndex left, RowIndex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RowIndex left, RowIndex right)
        {
            return !(left == right);
        }

        #endregion

        public override string ToString()
        {
            return Row.ToString();
        }

        public static RowIndex operator -(RowIndex first)
        {
            return (-1) * first;
        }

        public static RowIndex operator +(RowIndex first, RowIndex second)
        {
            return From(first.Row + second.Row);
        }

        public static RowIndex operator +(RowIndex first, int second)
        {
            return From(first.Row + second);
        }

        public static RowIndex operator -(RowIndex first, RowIndex second)
        {
            return From(first.Row - second.Row);
        }

        public static RowIndex operator *(int first, RowIndex second)
        {
            return From(first * second.Row);
        }

        public static RowIndex operator /(RowIndex first, RowIndex second)
        {
            return From(first.Row / second.Row);
        }

        public static RowIndex operator %(RowIndex first, RowIndex second)
        {
            return From(first.Row % second.Row);
        }

        public static bool operator <(RowIndex first, RowIndex second)
        {
            return first.Row < second.Row;
        }

        public static bool operator <=(RowIndex first, RowIndex second)
        {
            return !(second < first);
        }

        public static bool operator >(RowIndex first, RowIndex second)
        {
            return second < first;
        }

        public static bool operator >=(RowIndex first, RowIndex second)
        {
            return !(first < second);
        }

        public static bool operator <(int first, RowIndex second)
        {
            return From(first) < second;
        }

        public static bool operator <=(int first, RowIndex second)
        {
            return !(second < first);
        }

        public static bool operator >(int first, RowIndex second)
        {
            return second < first;
        }

        public static bool operator >=(int first, RowIndex second)
        {
            return !(first < second);
        }

        public static bool operator <(RowIndex first, int second)
        {
            return first < From(second);
        }

        public static bool operator <=(RowIndex first, int second)
        {
            return !(second < first);
        }

        public static bool operator >(RowIndex first, int second)
        {
            return second < first;
        }

        public static bool operator >=(RowIndex first, int second)
        {
            return !(first < second);
        }

        public RowIndex Min(RowIndex other)
        {
            return From(Math.Min(Row, other.Row));
        }

        public RowIndex Max(RowIndex other)
        {
            return From(Math.Max(Row, other.Row));
        }

        public RowIndex Reduce(RowIndex other)
        {
            return this - Min(other);
        }

        public RowIndex Distance(RowIndex other)
        {
            return From(Math.Abs(Row - other.Row));
        }

        public ColumnIndex AsColumn
        {
            get
            {
                return ColumnIndex.From(Row);
            }
        }

        public GridVector AsVector
        {
            get
            {
                return GridVector.From(this);
            }
        }

        public string AsDebug
        {
            get
            {
                return Row.ToString();
            }
        }
    }
}

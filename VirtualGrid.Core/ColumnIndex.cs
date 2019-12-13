using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    /// <summary>
    /// 列番号または列数
    /// </summary>
    [DebuggerDisplay("{AsDebug}")]
    public struct ColumnIndex
        : IEquatable<ColumnIndex>
    {
        public readonly int Column;

        private ColumnIndex(int column)
        {
            Column = column;
        }

        public static ColumnIndex From(int column)
        {
            return new ColumnIndex(column);
        }

        public static ColumnIndex Zero
        {
            get
            {
                return From(0);
            }
        }

        #region equality

        public bool Equals(ColumnIndex other)
        {
            return Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return obj is ColumnIndex && Equals((ColumnIndex)obj);
        }

        public override int GetHashCode()
        {
            return -343017389 + Column.GetHashCode();
        }

        public static bool operator ==(ColumnIndex left, ColumnIndex right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ColumnIndex left, ColumnIndex right)
        {
            return !(left == right);
        }

        #endregion

        public static ColumnIndex operator -(ColumnIndex first)
        {
            return (-1) * first;
        }

        public static ColumnIndex operator +(ColumnIndex first, ColumnIndex second)
        {
            return From(first.Column + second.Column);
        }

        public static ColumnIndex operator -(ColumnIndex first, ColumnIndex second)
        {
            return From(first.Column - second.Column);
        }

        public static ColumnIndex operator *(int first, ColumnIndex second)
        {
            return From(first * second.Column);
        }

        public static ColumnIndex operator /(ColumnIndex first, ColumnIndex second)
        {
            return From(first.Column / second.Column);
        }

        public static ColumnIndex operator %(ColumnIndex first, ColumnIndex second)
        {
            return From(first.Column % second.Column);
        }

        public static bool operator <(ColumnIndex first, ColumnIndex second)
        {
            return first.Column < second.Column;
        }

        public static bool operator <=(ColumnIndex first, ColumnIndex second)
        {
            return !(second < first);
        }

        public static bool operator >(ColumnIndex first, ColumnIndex second)
        {
            return second < first;
        }

        public static bool operator >=(ColumnIndex first, ColumnIndex second)
        {
            return !(first < second);
        }

        public static bool operator <(int first, ColumnIndex second)
        {
            return first < second.Column;
        }

        public static bool operator <=(int first, ColumnIndex second)
        {
            return !(second < first);
        }

        public static bool operator >(int first, ColumnIndex second)
        {
            return second < first;
        }

        public static bool operator >=(int first, ColumnIndex second)
        {
            return !(first < second);
        }

        public static bool operator <(ColumnIndex first, int second)
        {
            return first.Column < second;
        }

        public static bool operator <=(ColumnIndex first, int second)
        {
            return !(second < first);
        }

        public static bool operator >(ColumnIndex first, int second)
        {
            return second < first;
        }

        public static bool operator >=(ColumnIndex first, int second)
        {
            return !(first < second);
        }

        public ColumnIndex Min(ColumnIndex other)
        {
            return From(Math.Min(Column, other.Column));
        }

        public ColumnIndex Max(ColumnIndex other)
        {
            return From(Math.Max(Column, other.Column));
        }

        public ColumnIndex Reduce(ColumnIndex other)
        {
            return this - Min(other);
        }

        public ColumnIndex Distance(ColumnIndex other)
        {
            return From(Math.Abs(Column - other.Column));
        }

        public RowIndex AsRow
        {
            get
            {
                return RowIndex.From(Column);
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
                return Column.ToString();
            }
        }

        public override string ToString()
        {
            return Column.ToString();
        }
    }
}

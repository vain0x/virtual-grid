using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    [DebuggerDisplay("{AsDebug}")]
    public struct GridElementKey
        : IEquatable<GridElementKey>
    {
        public readonly object RowElementKey;

        public readonly object ColumnElementKey;

        private GridElementKey(object rowElementKey, object columnElementKey)
        {
            RowElementKey = rowElementKey;
            ColumnElementKey = columnElementKey;
        }

        public static GridElementKey Create(object rowElementKey, object columnElementKey)
        {
            Debug.Assert(rowElementKey != null);
            Debug.Assert(columnElementKey != null);

            return new GridElementKey(rowElementKey, columnElementKey);
        }

        public string AsDebug
        {
            get
            {
                return string.Format("({0}, {1})", RowElementKey, ColumnElementKey);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is GridElementKey key && Equals(key);
        }

        public bool Equals(GridElementKey other)
        {
            return EqualityComparer<object>.Default.Equals(RowElementKey, other.RowElementKey) &&
                   EqualityComparer<object>.Default.Equals(ColumnElementKey, other.ColumnElementKey);
        }

        public override int GetHashCode()
        {
            var hashCode = -541575386;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(RowElementKey);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(ColumnElementKey);
            return hashCode;
        }

        public static bool operator ==(GridElementKey left, GridElementKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridElementKey left, GridElementKey right)
        {
            return !(left == right);
        }
    }
}

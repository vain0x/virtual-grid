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
        public readonly object RowElementKeyOpt;

        public readonly object ColumnElementKeyOpt;

        private GridElementKey(object rowElementKeyOpt, object columnElementKeyOpt)
        {
            RowElementKeyOpt = rowElementKeyOpt;
            ColumnElementKeyOpt = columnElementKeyOpt;
        }

        public static GridElementKey NewRowHeader(object elementKey)
        {
            Debug.Assert(elementKey != null);

            return new GridElementKey(elementKey, null);
        }

        public static GridElementKey NewColumnHeader(object elementKey)
        {
            Debug.Assert(elementKey != null);

            return new GridElementKey(null, elementKey);
        }

        public static GridElementKey NewBody(object rowElementKey, object columnElementKey)
        {
            Debug.Assert(rowElementKey != null);
            Debug.Assert(columnElementKey != null);

            return new GridElementKey(rowElementKey, columnElementKey);
        }

        public GridPart GridPart
        {
            get
            {
                if (ColumnElementKeyOpt == null)
                    return GridPart.RowHeader;

                if (RowElementKeyOpt == null)
                    return GridPart.ColumnHeader;

                return GridPart.Body;
            }
        }

        public string AsDebug
        {
            get
            {
                return string.Format("({0}, {1})", RowElementKeyOpt, ColumnElementKeyOpt);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is GridElementKey key && Equals(key);
        }

        public bool Equals(GridElementKey other)
        {
            return EqualityComparer<object>.Default.Equals(RowElementKeyOpt, other.RowElementKeyOpt) &&
                   EqualityComparer<object>.Default.Equals(ColumnElementKeyOpt, other.ColumnElementKeyOpt);
        }

        public override int GetHashCode()
        {
            var hashCode = -541575386;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(RowElementKeyOpt);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(ColumnElementKeyOpt);
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

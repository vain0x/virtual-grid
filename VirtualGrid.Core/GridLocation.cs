using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    [DebuggerDisplay("{AsDebug}")]
    public struct GridLocation
        : IEquatable<GridLocation>
    {
        public readonly GridPart Part;

        public readonly GridVector Index;

        public string AsDebug
        {
            get
            {
                return Part.ToString() + Index.AsDebug;
            }
        }

        private GridLocation(GridPart part, GridVector index)
        {
            Part = part;
            Index = index;
        }

        public static GridLocation Create(GridPart part, GridVector index)
        {
            return new GridLocation(part, index);
        }

        public static GridLocation NewColumnHeader(GridVector index)
        {
            return Create(GridPart.ColumnHeader, index);
        }

        public static GridLocation NewRowHeader(GridVector index)
        {
            return Create(GridPart.RowHeader, index);
        }

        public static GridLocation NewBody(GridVector index)
        {
            return Create(GridPart.Body, index);
        }

        public override bool Equals(object obj)
        {
            return obj is GridLocation location && Equals(location);
        }

        public bool Equals(GridLocation other)
        {
            return Part == other.Part &&
                   Index.Equals(other.Index);
        }

        public override int GetHashCode()
        {
            var hashCode = 978147999;
            hashCode = hashCode * -1521134295 + Part.GetHashCode();
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(GridLocation left, GridLocation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridLocation left, GridLocation right)
        {
            return !(left == right);
        }
    }
}

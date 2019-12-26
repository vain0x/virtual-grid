using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Spreads
{
    [DebuggerDisplay("{AsDebug}")]
    public struct SpreadLocation
        : IEquatable<SpreadLocation>
    {
        public readonly SpreadPart Part;

        public readonly GridVector Index;

        public string AsDebug
        {
            get
            {
                return Part.ToString() + Index.AsDebug;
            }
        }

        private SpreadLocation(SpreadPart part, GridVector index)
        {
            Part = part;
            Index = index;
        }

        public static SpreadLocation Create(SpreadPart part, GridVector index)
        {
            return new SpreadLocation(part, index);
        }

        public static SpreadLocation NewColumnHeader(GridVector index)
        {
            return Create(SpreadPart.ColumnHeader, index);
        }

        public static SpreadLocation NewRowHeader(GridVector index)
        {
            return Create(SpreadPart.RowHeader, index);
        }

        public static SpreadLocation NewBody(GridVector index)
        {
            return Create(SpreadPart.Body, index);
        }

        public override bool Equals(object obj)
        {
            return obj is SpreadLocation location && Equals(location);
        }

        public bool Equals(SpreadLocation other)
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

        public static bool operator ==(SpreadLocation left, SpreadLocation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SpreadLocation left, SpreadLocation right)
        {
            return !(left == right);
        }
    }
}

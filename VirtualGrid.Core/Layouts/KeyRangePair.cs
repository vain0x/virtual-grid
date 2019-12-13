using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    public struct KeyRangePair
        : IEquatable<KeyRangePair>
    {
        public readonly object Key;

        public readonly GridRange Range;

        public KeyRangePair(object key, GridRange range)
        {
            Key = key;
            Range = range;
        }

        public override bool Equals(object obj)
        {
            return obj is KeyRangePair pair && Equals(pair);
        }

        public bool Equals(KeyRangePair other)
        {
            return EqualityComparer<object>.Default.Equals(Key, other.Key) &&
                   Range.Equals(other.Range);
        }

        public override int GetHashCode()
        {
            var hashCode = -568781218;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + EqualityComparer<GridRange>.Default.GetHashCode(Range);
            return hashCode;
        }

        public static bool operator ==(KeyRangePair left, KeyRangePair right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(KeyRangePair left, KeyRangePair right)
        {
            return !(left == right);
        }
    }
}

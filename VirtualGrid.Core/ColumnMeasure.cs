using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    [DebuggerDisplay("{AsDebug}")]
    public struct ColumnMeasure
        : IEquatable<ColumnMeasure>
    {
        public readonly ColumnIndex? ColumnOpt;

        private ColumnMeasure(ColumnIndex? columnOpt)
        {
            ColumnOpt = columnOpt;
        }

        public static ColumnMeasure Zero
        {
            get
            {
                return From(0);
            }
        }

        public static ColumnMeasure Infinite
        {
            get
            {
                return From(null);
            }
        }

        public static ColumnMeasure From(ColumnIndex? columnOpt)
        {
            return new ColumnMeasure(columnOpt);
        }

        public static ColumnMeasure From(int column)
        {
            return From(ColumnIndex.From(column));
        }

        public static ColumnMeasure operator +(ColumnMeasure first, ColumnMeasure second)
        {
            return From(first.ColumnOpt + second.ColumnOpt);
        }

        public static bool operator ==(ColumnMeasure left, ColumnMeasure right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ColumnMeasure left, ColumnMeasure right)
        {
            return !(left == right);
        }

        public string AsDebug
        {
            get
            {
                return ColumnOpt.HasValue ? ColumnOpt.Value.ToString() : "Inf";
            }
        }

        public ColumnMeasure Reduce(ColumnMeasure other)
        {
            if (!ColumnOpt.HasValue)
                return Infinite;

            if (!other.ColumnOpt.HasValue)
                return Zero;

            return From(ColumnOpt.Value.Reduce(other.ColumnOpt.Value));
        }

        public ColumnMeasure Min(ColumnMeasure other)
        {
            if (!ColumnOpt.HasValue)
                return other;

            if (!other.ColumnOpt.HasValue)
                return this;

            return From(ColumnOpt.Value.Min(other.ColumnOpt.Value));
        }

        public ColumnMeasure Max(ColumnMeasure other)
        {
            if (!ColumnOpt.HasValue || !other.ColumnOpt.HasValue)
                return this;

            return From(ColumnOpt.Value.Max(other.ColumnOpt.Value));
        }

        public override bool Equals(object obj)
        {
            return obj is ColumnMeasure measure && Equals(measure);
        }

        public bool Equals(ColumnMeasure other)
        {
            return EqualityComparer<ColumnIndex?>.Default.Equals(ColumnOpt, other.ColumnOpt);
        }

        public override int GetHashCode()
        {
            return -331934140 + EqualityComparer<ColumnIndex?>.Default.GetHashCode(ColumnOpt);
        }
    }
}

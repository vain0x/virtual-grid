using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    /// <summary>
    /// 行数 (有限または無限)
    /// </summary>
    [DebuggerDisplay("{AsDebug}")]
    public struct RowMeasure
        : IEquatable<RowMeasure>
    {
        public readonly RowIndex? RowOpt;

        private RowMeasure(RowIndex? rowOpt)
        {
            RowOpt = rowOpt;
        }

        public static RowMeasure Zero
        {
            get
            {
                return From(0);
            }
        }

        public static RowMeasure Infinite
        {
            get
            {
                return From(null);
            }
        }

        public static RowMeasure From(RowIndex? rowOpt)
        {
            return new RowMeasure(rowOpt);
        }

        public static RowMeasure From(int row)
        {
            return From(RowIndex.From(row));
        }

        public static RowMeasure operator +(RowMeasure first, RowMeasure second)
        {
            return From(first.RowOpt + second.RowOpt);
        }

        public static bool operator ==(RowMeasure left, RowMeasure right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RowMeasure left, RowMeasure right)
        {
            return !(left == right);
        }

        public string AsDebug
        {
            get
            {
                return RowOpt.HasValue ? RowOpt.Value.AsDebug : "Inf";
            }
        }

        public RowMeasure Reduce(RowMeasure other)
        {
            if (!RowOpt.HasValue)
                return Infinite;

            if (!other.RowOpt.HasValue)
                return Zero;

            return From(RowOpt.Value.Reduce(other.RowOpt.Value));
        }

        public RowMeasure Min(RowMeasure other)
        {
            if (!RowOpt.HasValue)
                return other;

            if (!other.RowOpt.HasValue)
                return this;

            return From(RowOpt.Value.Min(other.RowOpt.Value));
        }

        public RowMeasure Max(RowMeasure other)
        {
            if (!RowOpt.HasValue || !other.RowOpt.HasValue)
                return Infinite;

            return From(RowOpt.Value.Max(other.RowOpt.Value));
        }

        public override bool Equals(object obj)
        {
            return obj is RowMeasure measure && Equals(measure);
        }

        public bool Equals(RowMeasure other)
        {
            return EqualityComparer<RowIndex?>.Default.Equals(RowOpt, other.RowOpt);
        }

        public override int GetHashCode()
        {
            return 698228976 + EqualityComparer<RowIndex?>.Default.GetHashCode(RowOpt);
        }
    }
}

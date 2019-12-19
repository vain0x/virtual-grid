using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    /// <summary>
    /// 2次元グリッド上の範囲
    /// </summary>
    [DebuggerDisplay("{AsDebug}")]
    public struct GridRange
        : IEquatable<GridRange>
    {
        public readonly GridVector Start;

        public readonly GridVector End;

        private GridRange(GridVector start, GridVector end)
        {
            Start = start;
            End = end;
        }

        public static GridRange Create(GridVector start, GridVector end)
        {
            return new GridRange(start, end);
        }

        public static GridRange Zero
        {
            get
            {
                return Create(GridVector.Zero, GridVector.Zero);
            }
        }

        public bool ContainsStrictly(GridVector index)
        {
            return Start.Row <= index.Row
                && index.Row < End.Row
                && Start.Column <= index.Column
                && index.Column < End.Column;
        }

        public GridRange WithStart(GridVector start)
        {
            return Create(start, End);
        }

        public GridRange WithEnd(GridVector end)
        {
            return Create(Start, end);
        }

        public GridVector Size
        {
            get
            {
                return GridVector.Create(
                    Start.Row.Distance(End.Row),
                    Start.Column.Distance(End.Column)
                );
            }
        }

        public string AsDebug
        {
            get
            {
                return Start.AsDebug + ".." + End.AsDebug;
            }
        }

        public GridRange Translate(GridVector offset)
        {
            return Create(Start + offset, End + offset);
        }

        internal GridRange Clip(GridRange range)
        {
            var end = End.Min(range.End);
            var start = Start.Max(range.Start).Min(end);
            return Create(start, end);
        }

        public override bool Equals(object obj)
        {
            return obj is GridRange range && Equals(range);
        }

        public bool Equals(GridRange other)
        {
            return Start.Equals(other.Start) &&
                   End.Equals(other.End);
        }

        public override int GetHashCode()
        {
            var hashCode = -1676728671;
            hashCode = hashCode * -1521134295 + Start.GetHashCode();
            hashCode = hashCode * -1521134295 + End.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(GridRange left, GridRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridRange left, GridRange right)
        {
            return !(left == right);
        }
    }
}

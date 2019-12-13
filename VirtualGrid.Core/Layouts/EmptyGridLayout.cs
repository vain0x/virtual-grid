using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    [DebuggerDisplay("{AsDebug}")]
    internal sealed class EmptyGridLayout
        : IGridLayout
        , IEquatable<EmptyGridLayout>
    {
        public object ElementKey { get; private set; }

        public EmptyGridLayout(object elementKey)
        {
            ElementKey = elementKey;
        }

        public string AsDebug
        {
            get
            {
                return "EmptyGridLayout(" + DebugString.From(ElementKey) + ")";
            }
        }

        public GridVector Measure(GridMeasure available, GridLayoutModel model)
        {
            return GridVector.Zero;
        }

        public void Arrange(GridRange range, GridLayoutModel model)
        {
        }

        public IGridLayout ToGridLayout()
        {
            return this;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EmptyGridLayout);
        }

        public bool Equals(EmptyGridLayout other)
        {
            return other != null &&
                   EqualityComparer<object>.Default.Equals(ElementKey, other.ElementKey);
        }

        public override int GetHashCode()
        {
            return -1122045474 + EqualityComparer<object>.Default.GetHashCode(ElementKey);
        }

        public static bool operator ==(EmptyGridLayout left, EmptyGridLayout right)
        {
            return EqualityComparer<EmptyGridLayout>.Default.Equals(left, right);
        }

        public static bool operator !=(EmptyGridLayout left, EmptyGridLayout right)
        {
            return !(left == right);
        }
    }
}

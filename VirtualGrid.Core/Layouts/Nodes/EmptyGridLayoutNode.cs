using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    /// <summary>
    /// セルを配置しないレイアウト。
    /// </summary>
    [DebuggerDisplay("{AsDebug}")]
    internal sealed class EmptyGridLayoutNode
        : IGridLayoutNode
        , IEquatable<EmptyGridLayoutNode>
    {
        public object ElementKey { get; private set; }

        public EmptyGridLayoutNode(object elementKey)
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

        public GridVector Measure(GridMeasure available, GridLayoutContext context)
        {
            return GridVector.Zero;
        }

        public void Arrange(GridRange range, GridLayoutContext context)
        {
        }

        public IGridLayoutNode ToGridLayout()
        {
            return this;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EmptyGridLayoutNode);
        }

        public bool Equals(EmptyGridLayoutNode other)
        {
            return other != null &&
                   EqualityComparer<object>.Default.Equals(ElementKey, other.ElementKey);
        }

        public override int GetHashCode()
        {
            return -1122045474 + EqualityComparer<object>.Default.GetHashCode(ElementKey);
        }

        public void Iterate(Action<IGridLayoutNode> action)
        {
        }

        public static bool operator ==(EmptyGridLayoutNode left, EmptyGridLayoutNode right)
        {
            return EqualityComparer<EmptyGridLayoutNode>.Default.Equals(left, right);
        }

        public static bool operator !=(EmptyGridLayoutNode left, EmptyGridLayoutNode right)
        {
            return !(left == right);
        }
    }
}

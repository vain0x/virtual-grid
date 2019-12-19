using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    /// <summary>
    /// 2つの要素を縦 (水平方向) に並べるレイアウト。
    /// </summary>
    [DebuggerDisplay("{AsDebug}")]
    internal sealed class VerticalLinkGridLayoutNode
        : IGridLayoutNode
    {
        public readonly IGridLayoutNode First;

        public readonly IGridLayoutNode Second;

        public VerticalLinkGridLayoutNode(IGridLayoutNode first, IGridLayoutNode second)
        {
            First = first;
            Second = second;
        }

        public string AsDebug
        {
            get
            {
                return "VerticalLinkGridLayout(" + DebugString.From(ElementKey) + ")";
            }
        }

        public object ElementKey
        {
            get
            {
                return Tuple.Create(First.ElementKey, Second.ElementKey);
            }
        }

        public GridVector Measure(GridMeasure available, GridLayoutContext context)
        {
            var first = context.Measure(First, available);

            available = available.Reduce(first);

            var second = context.Measure(Second, available);

            return GridVector.Create(first.Row + second.Row, first.Column.Max(second.Column));
        }

        public void Arrange(GridRange range, GridLayoutContext context)
        {
            var start = range.Start;

            var firstMeasure = context.LastMeasure(First.ElementKey);
            context.Arrange(First, start.To(start + firstMeasure).Clip(range));

            start += firstMeasure.Row.AsVector;

            var secondMeasure = context.LastMeasure(Second.ElementKey);
            context.Arrange(Second, start.To(start + secondMeasure).Clip(range));
        }

        public void Iterate(Action<IGridLayoutNode> action)
        {
            action(First);
            action(Second);
        }

        public IGridLayoutNode ToGridLayout()
        {
            return this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    /// <summary>
    /// 2つの要素を横 (水平方向) に並べるレイアウト。
    /// </summary>
    [DebuggerDisplay("{AsDebug}")]
    internal sealed class HorizontalLinkGridLayout
        : IGridLayout
    {
        public readonly IGridLayout First;

        public readonly IGridLayout Second;

        public HorizontalLinkGridLayout(IGridLayout first, IGridLayout second)
        {
            First = first;
            Second = second;
        }

        public string AsDebug
        {
            get
            {
                return "HorizontalLinkGridLayout(" + DebugString.From(ElementKey) + ")";
            }
        }

        public object ElementKey
        {
            get
            {
                return Tuple.Create(First.ElementKey, Second.ElementKey);
            }
        }

        public GridVector Measure(GridMeasure available, GridLayoutModel model)
        {
            var first = model.Measure(First, available);

            available = available.Reduce(first);

            var second = model.Measure(Second, available);

            return GridVector.Create(first.Row.Max(second.Row), first.Column + second.Column);
        }

        public void Arrange(GridRange range, GridLayoutModel model)
        {
            var start = range.Start;

            var first = model.Touch(First.ElementKey);
            model.Arrange(First, start.To(start + first.LastMeasure).Clip(range));

            start += first.LastMeasure.Column.AsVector;

            var second = model.Touch(Second.ElementKey);
            model.Arrange(Second, start.To(start + second.LastMeasure).Clip(range));
        }

        public IGridLayout ToGridLayout()
        {
            return this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    public struct GridLayoutDelta
    {
        public readonly KeyRangePair OldOpt;

        public readonly KeyRangePair NewOpt;
    }

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

    /// <summary>
    /// 1つのセルからなるレイアウト。
    /// </summary>
    [DebuggerDisplay("{AsDebug}")]
    internal sealed class CellGridLayout
        : IGridLayout
    {
        public object ElementKey { get; }

        public CellGridLayout(object elementKey)
        {
            ElementKey = elementKey;
        }

        public void CollectKeys(List<object> elementKeys)
        {
            elementKeys.Add(ElementKey);
        }

        public string AsDebug
        {
            get
            {
                return "CellGridLayout(" + DebugString.From(ElementKey) + ")";
            }
        }

        public GridVector Measure(GridMeasure available, GridLayoutModel model)
        {
            var row = RowIndex.From(1);
            var column = ColumnIndex.From(1);
            return GridVector.Create(row, column);
        }

        public void Arrange(GridRange range, GridLayoutModel model)
        {
        }

        public IGridLayout ToGridLayout()
        {
            return this;
        }
    }

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

    /// <summary>
    /// 2つの要素を縦 (水平方向) に並べるレイアウト。
    /// </summary>
    [DebuggerDisplay("{AsDebug}")]
    internal sealed class VerticalLinkGridLayout
        : IGridLayout
    {
        public readonly IGridLayout First;

        public readonly IGridLayout Second;

        public VerticalLinkGridLayout(IGridLayout first, IGridLayout second)
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

        public GridVector Measure(GridMeasure available, GridLayoutModel model)
        {
            var first = model.Measure(First, available);

            available = available.Reduce(first);

            var second = model.Measure(Second, available);

            return GridVector.Create(first.Row + second.Row, first.Column.Max(second.Column));
        }

        public void Arrange(GridRange range, GridLayoutModel model)
        {
            var start = range.Start;

            var first = model.Touch(First.ElementKey);
            model.Arrange(First, start.To(start + first.LastMeasure).Clip(range));

            start += first.LastMeasure.Row.AsVector;

            var second = model.Touch(Second.ElementKey);
            model.Arrange(Second, start.To(start + second.LastMeasure).Clip(range));
        }

        public IGridLayout ToGridLayout()
        {
            return this;
        }
    }
}

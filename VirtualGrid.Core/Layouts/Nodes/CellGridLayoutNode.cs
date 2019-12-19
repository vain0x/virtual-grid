using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    /// <summary>
    /// 領域を1つのセルで埋めるレイアウト。
    /// </summary>
    [DebuggerDisplay("{AsDebug}")]
    internal sealed class CellGridLayoutNode
        : IGridLayoutNode
    {
        public object ElementKey { get; }

        public CellGridLayoutNode(object elementKey)
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

        public GridVector Measure(GridMeasure available, GridLayoutContext context)
        {
            var row = available.Row.RowOpt ?? RowIndex.From(1);
            var column = available.Column.ColumnOpt ?? ColumnIndex.From(1);
            return GridVector.Create(row, column);
        }

        public void Arrange(GridRange range, GridLayoutContext context)
        {
        }

        public void Iterate(Action<IGridLayoutNode> action)
        {
        }

        public IGridLayoutNode ToGridLayout()
        {
            return this;
        }
    }
}

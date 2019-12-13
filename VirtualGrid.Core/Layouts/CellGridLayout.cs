using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
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
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    /// <summary>
    /// ノードを縦 (垂直方向) または横 (水平方向) に並べるレイアウト。
    /// </summary>
    [DebuggerDisplay("{AsDebug}")]
    internal sealed class StackGridLayoutNode
        : IGridLayoutNode
    {
        public readonly IGridLayoutNode[] Nodes;

        private readonly bool _horizontal;

        public object ElementKey { get; private set; }

        public StackGridLayoutNode(IGridLayoutNode[] nodes, bool horizontal)
        {
            Nodes = nodes;
            _horizontal = horizontal;
            ElementKey = new ArrayTuple<IGridLayoutNode>(nodes);
        }

        public string AsDebug
        {
            get
            {
                return (_horizontal ? "横" : "縦") + "(" + Nodes.Length.ToString() + ")";
            }
        }

        public GridVector Measure(GridMeasure available, GridLayoutContext context)
        {
            var rowSize = RowIndex.Zero;
            var columnSize = ColumnIndex.Zero;

            if (_horizontal)
            {
                foreach (var node in Nodes)
                {
                    var nodeSize = context.Measure(node, available);

                    available = available.Reduce(nodeSize.Column.AsVector);
                    rowSize = rowSize.Max(nodeSize.Row);
                    columnSize += nodeSize.Column;
                }
            }
            else
            {
                foreach (var node in Nodes)
                {
                    var nodeSize = context.Measure(node, available);

                    available = available.Reduce(nodeSize.Row.AsVector);
                    rowSize += nodeSize.Row;
                    columnSize = columnSize.Max(nodeSize.Column);
                }
            }

            return GridVector.Create(rowSize, columnSize);
        }

        public void Arrange(GridRange range, GridLayoutContext context)
        {
            var start = range.Start;

            if (_horizontal)
            {
                foreach (var node in Nodes)
                {
                    var nodeSize = context.LastMeasure(node.ElementKey);
                    context.Arrange(node, start.To(start + nodeSize).Clip(range));
                    start += nodeSize.Column.AsVector;
                }
            }
            else
            {
                foreach (var node in Nodes)
                {
                    var nodeSize = context.LastMeasure(node.ElementKey);
                    context.Arrange(node, start.To(start + nodeSize).Clip(range));
                    start += nodeSize.Row.AsVector;
                }
            }
        }

        public void Iterate(Action<IGridLayoutNode> action)
        {
            foreach (var node in Nodes)
            {
                action(node);
            }
        }

        public IGridLayoutNode ToGridLayout()
        {
            return this;
        }
    }
}

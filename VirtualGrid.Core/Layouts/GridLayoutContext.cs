using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace VirtualGrid.Layouts
{
    /// <summary>
    /// レイアウト計算のデータを管理する。
    /// </summary>
    public sealed class GridLayoutContext
    {
        private readonly DefaultDictionary<object, GridMeasure> _lastAvailables =
            new DefaultDictionary<object, GridMeasure>(_key => GridMeasure.Zero);

        private readonly DefaultDictionary<object, GridVector> _lastMeasures =
            new DefaultDictionary<object, GridVector>(_key => GridVector.Zero);

        private readonly DefaultDictionary<object, GridRange> _lastArranges =
            new DefaultDictionary<object, GridRange>(_key => GridRange.Zero);

        public GridVector LastMeasure(object elementKey)
        {
            return _lastMeasures[elementKey];
        }

        public GridRange LastArrange(object elementKey)
        {
            return _lastArranges[elementKey];
        }

        public GridVector Measure(IGridLayoutNode layout, GridMeasure available)
        {
            var last = _lastAvailables[layout.ElementKey];
            if (last == available)
                return _lastMeasures[layout.ElementKey];

            var measure = layout.Measure(available, this);
            _lastMeasures[layout.ElementKey] = measure;

            return measure;
        }

        public void Arrange(IGridLayoutNode layout, GridRange range)
        {
            var last = _lastArranges[layout.ElementKey];
            if (last == range)
                return;

            layout.Arrange(range, this);
            _lastArranges[layout.ElementKey] = range;
        }

        public IReadOnlyList<object> Locate(IGridLayoutNode layout, GridVector index)
        {
            var elementKeys = new List<object>();

            Action<IGridLayoutNode> action = null;
            action = node =>
            {
                var range = LastArrange(node.ElementKey);
                if (range.ContainsStrictly(index))
                {
                    elementKeys.Add(node.ElementKey);
                    node.Iterate(action);
                }
            };

            action(layout);

            return elementKeys;
        }

        public GridPivots GetPivots(IGridLayoutNode layout)
        {
            var rowPivotMap = new DefaultDictionary<RowIndex, KeyValuePair<ColumnIndex, object>>(_ =>
                Pair.Create(ColumnIndex.MaxValue, default(object))
            );
            var columnPivotMap = new DefaultDictionary<ColumnIndex, KeyValuePair<RowIndex, object>>(_ =>
                Pair.Create(RowIndex.MaxValue, default(object))
            );

            Action<IGridLayoutNode> action = null;
            action = node =>
            {
                var range = LastArrange(node.ElementKey);
                var start = range.Start;

                if (rowPivotMap[start.Row].Key >= start.Column)
                {
                    rowPivotMap[start.Row] = Pair.Create(start.Column, node.ElementKey);
                }

                if (columnPivotMap[start.Column].Key >= start.Row)
                {
                    columnPivotMap[start.Column] = Pair.Create(start.Row, node.ElementKey);
                }

                node.Iterate(action);
            };

            action(layout);

            var rootRange = LastArrange(layout.ElementKey);

            var rowPivots = Enumerable.Range(0, rootRange.End.Row.Row)
                .Select(row => rowPivotMap[RowIndex.From(row)].Value)
                .ToArray();

            var columnPivots = Enumerable.Range(0, rootRange.End.Column.Column)
                .Select(column => columnPivotMap[ColumnIndex.From(column)].Value)
                .ToArray();

            // FIXME: null は (最も近いピボット, 位置の差) で埋める。
            if (rowPivots.Any(k => k == null) || columnPivots.Any(k => k == null))
            {
                Debug.WriteLine("カラムヘッダー・ローヘッダーの結合セルは未対応です");

                for (var i = 0; i < rowPivots.Length; i++)
                {
                    if (rowPivots[i] == null)
                    {
                        rowPivots[i] = "?_ROW_PIVOT=" + i;
                    }
                }

                for (var i = 0; i < columnPivots.Length; i++)
                {
                    if (columnPivots[i] == null)
                    {
                        columnPivots[i] = "?_COLUMN_PIVOT=" + i;
                    }
                }
            }

            return new GridPivots(rowPivots, columnPivots);
        }

        public void Clear()
        {
            _lastAvailables.Clear();
            _lastMeasures.Clear();
            _lastArranges.Clear();
        }
    }
}

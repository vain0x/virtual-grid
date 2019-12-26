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

        public void Clear()
        {
            _lastAvailables.Clear();
            _lastMeasures.Clear();
            _lastArranges.Clear();
        }
    }
}

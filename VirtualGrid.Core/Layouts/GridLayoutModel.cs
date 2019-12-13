using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    public sealed class GridLayoutModel
    {
        private GridLayoutElement _root = new GridLayoutElement("EMPTY");

        readonly DefaultDictionary<object, GridLayoutElement> _elements =
            new DefaultDictionary<object, GridLayoutElement>(elementKey => new GridLayoutElement(elementKey));

        public GridLayoutElement Touch(object elementKey)
        {
            return _elements[elementKey];
        }

        public object Locate(GridVector index)
        {
            foreach (var element in _elements.Values)
            {
                if (element.LastArrange.ContainsStrictly(index))
                    return element.ElementKey;
            }
            return null;
        }

        internal GridVector Measure(IGridLayout layout, GridMeasure available)
        {
            var measure = layout.Measure(available, this);
            Touch(layout.ElementKey).LastMeasure = measure;
            return measure;
        }

        internal void Arrange(IGridLayout layout, GridRange range)
        {
            layout.Arrange(range, this);
            Touch(layout.ElementKey).LastArrange = range;
        }

        public GridLayoutElement Patch(IGridLayout layout)
        {
            return _root;
        }
    }
}

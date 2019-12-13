using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    public interface IGridLayout
        : IGridLayoutProvider
    {
        object ElementKey { get; }

        GridVector Measure(GridMeasure available, GridLayoutModel model);

        void Arrange(GridRange range, GridLayoutModel model);
    }

    public interface IGridLayoutProvider
    {
        IGridLayout ToGridLayout();
    }

    public static class GridLayout
    {
        public static IGridLayout Empty(object elementKey)
        {
            return new EmptyGridLayout(elementKey);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Rendering
{
    /// <summary>
    /// レンダリング時のデータを管理するもの。
    /// </summary>
    public sealed class GridRenderContext<TProvider>
    {
        // row key, column key, element key
        public readonly List<Tuple<GridPart, object, object, object>> _cells =
            new List<Tuple<GridPart, object, object, object>>();

        public readonly TProvider Provider;

        public GridRenderContext(TProvider provider)
        {
            Provider = provider;
        }

        public void AddCell(GridPart part, object rowKey, object columnKey, object elementKey)
        {
            _cells.Add(Tuple.Create(part, rowKey, columnKey, elementKey));
        }

        public void Clear()
        {
            _cells.Clear();
        }
    }
}

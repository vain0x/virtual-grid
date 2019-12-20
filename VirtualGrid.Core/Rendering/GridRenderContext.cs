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
        public readonly List<GridCellKey> _cells =
            new List<GridCellKey>();

        public readonly TProvider Provider;

        public GridRenderContext(TProvider provider)
        {
            Provider = provider;
        }

        public void AddCell(GridPart part, object rowKey, object columnKey, object elementKey)
        {
            _cells.Add(new GridCellKey(part, rowKey, columnKey, elementKey));
        }

        public void Clear()
        {
            _cells.Clear();
        }
    }
}

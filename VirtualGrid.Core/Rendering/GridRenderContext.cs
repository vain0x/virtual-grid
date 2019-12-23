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
        public readonly TProvider Provider;

        public GridRenderContext(TProvider provider)
        {
            Provider = provider;
        }

        public void AddCell(GridPart part, object rowKey, object columnKey, object elementKey)
        {
        }

        public void Clear()
        {
        }
    }
}

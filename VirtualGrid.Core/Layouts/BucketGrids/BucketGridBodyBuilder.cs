using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Rendering;

namespace VirtualGrid.Layouts.BucketGrids
{
    public sealed class BucketGridBodyBuilder<TProvider>
    {
        private GridRenderContext<TProvider> _context;

        public BucketGridBodyBuilder(GridRenderContext<TProvider> context)
        {
            _context = context;
        }

        public IGridCellAdder<TProvider> At(IGridCellBuilder<TProvider> row, IGridCellBuilder<TProvider> column)
        {
            return new AnonymousGridCellAdder<TProvider>(() =>
            {
                var elementKey = GridElementKey.NewBody(row.ElementKey.RowElementKeyOpt, column.ElementKey.ColumnElementKeyOpt);
                return new IGridCellBuilder<TProvider>(elementKey, _context);
            });
        }

        public void Patch()
        {
            // FIXME:
        }
    }
}

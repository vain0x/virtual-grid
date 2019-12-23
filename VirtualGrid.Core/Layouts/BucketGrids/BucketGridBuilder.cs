using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;
using VirtualGrid.Rendering;

namespace VirtualGrid.Layouts.BucketGrids
{
    public sealed class BucketGridBuilder<TProvider>
    {
        public BucketGridHeaderBuilder<TProvider> RowHeader;

        public BucketGridHeaderBuilder<TProvider> ColumnHeader;

        public BucketGridBodyBuilder<TProvider> Body;

        public BucketGridBuilder(BucketGridLayout layout, GridRenderContext<TProvider> context, IGridHeaderDeltaListener rowHeaderDeltaListener, IGridHeaderDeltaListener columnHeaderDeltaListener)
        {
            RowHeader = new BucketGridHeaderBuilder<TProvider>(layout.RowHeaderNode, context, rowHeaderDeltaListener, isRowHeader: true);

            ColumnHeader = new BucketGridHeaderBuilder<TProvider>(layout.ColumnHeaderNode, context, columnHeaderDeltaListener, isRowHeader: false);

            Body = new BucketGridBodyBuilder<TProvider>(context);
        }

        public void Patch()
        {
            ColumnHeader.Patch();
            RowHeader.Patch();
            Body.Patch();
        }
    }
}

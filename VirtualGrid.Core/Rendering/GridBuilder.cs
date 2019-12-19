using System.Collections.Generic;
using VirtualGrid.Layouts;

namespace VirtualGrid.Rendering
{
    public sealed class GridBuilder<TProvider>
    {
        public GridHeaderBuilder<TProvider> ColumnHeader;

        public GridHeaderBuilder<TProvider> RowHeader;

        public GridBodyBuilder<TProvider> Body;

        public GridBuilder(GridRenderContext<TProvider> context)
        {
            ColumnHeader = new GridHeaderBuilder<TProvider>(new List<IGridLayoutBuilder>(), true, context);

            RowHeader = new GridHeaderBuilder<TProvider>(new List<IGridLayoutBuilder>(), false, context);

            Body = new GridBodyBuilder<TProvider>(context);
        }
    }
}

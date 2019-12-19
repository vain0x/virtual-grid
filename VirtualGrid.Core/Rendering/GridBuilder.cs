using System.Collections.Generic;
using VirtualGrid.Layouts;

namespace VirtualGrid.Rendering
{
    public sealed class GridBuilder
    {
        public GridHeaderBuilder ColumnHeader;

        public GridHeaderBuilder RowHeader;

        public GridBodyBuilder Body;

        public GridBuilder(GridRenderContext context)
        {
            ColumnHeader = new GridHeaderBuilder(new List<IGridLayoutBuilder>(), true, context);

            RowHeader = new GridHeaderBuilder(new List<IGridLayoutBuilder>(), false, context);

            Body = new GridBodyBuilder(context);
        }
    }
}

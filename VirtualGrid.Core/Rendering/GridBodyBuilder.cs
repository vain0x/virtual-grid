using System;

namespace VirtualGrid.Rendering
{
    public sealed class GridBodyBuilder
    {
        private GridRenderContext _context;

        public GridBodyBuilder(GridRenderContext context)
        {
            _context = context;

            ElementKey = "?BODY";
        }

        public object ElementKey { get; set; }

        public IGridCellAdder At(IGridCellBuilder row, IGridCellBuilder column)
        {
            return new AnonymousGridCellAdder(() =>
            {
                var elementKey = Tuple.Create(row.ElementKey, column.ElementKey);
                var cell = new IGridCellBuilder(elementKey, _context);
                _context.AddCell(GridPart.Body, row.ElementKey, column.ElementKey, elementKey);
                return cell;
            });
        }
    }
}

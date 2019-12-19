using System;

namespace VirtualGrid.Rendering
{
    public sealed class GridBodyBuilder<TProvider>
    {
        private GridRenderContext<TProvider> _context;

        public GridBodyBuilder(GridRenderContext<TProvider> context)
        {
            _context = context;

            ElementKey = "?BODY";
        }

        public object ElementKey { get; set; }

        public IGridCellAdder<TProvider> At(IGridCellBuilder<TProvider> row, IGridCellBuilder<TProvider> column)
        {
            return new AnonymousGridCellAdder<TProvider>(() =>
            {
                var elementKey = Tuple.Create(row.ElementKey, column.ElementKey);
                var cell = new IGridCellBuilder<TProvider>(elementKey, _context);
                _context.AddCell(GridPart.Body, row.ElementKey, column.ElementKey, elementKey);
                return cell;
            });
        }
    }
}

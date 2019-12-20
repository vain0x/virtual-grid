namespace VirtualGrid.Rendering
{
    public sealed class GridBodyBuilder<TProvider>
    {
        private GridRenderContext<TProvider> _context;

        public GridBodyBuilder(GridRenderContext<TProvider> context)
        {
            _context = context;
        }

        public IGridCellAdder<TProvider> At(IGridCellBuilder<TProvider> row, IGridCellBuilder<TProvider> column)
        {
            return new GridBodyCellAdder<TProvider>(row.ElementKey, column.ElementKey, _context);
        }
    }

    public struct GridBodyCellAdder<TProvider>
        : IGridCellAdder<TProvider>
    {
        private readonly object _rowElementKey;
        private readonly object _columnElementKey;
        private readonly GridRenderContext<TProvider> _context;

        public GridBodyCellAdder(object rowElementKey, object columnElementKey, GridRenderContext<TProvider> context)
        {
            _rowElementKey = rowElementKey;
            _columnElementKey = columnElementKey;
            _context = context;
        }

        public IGridCellBuilder<TProvider> AddCell()
        {
            var elementKey = new GridBodyCellKey(_rowElementKey, _columnElementKey);
            var cell = new IGridCellBuilder<TProvider>(elementKey, _context);
            _context.AddCell(GridPart.Body, _rowElementKey, _columnElementKey, elementKey);
            return cell;
        }
    }
}

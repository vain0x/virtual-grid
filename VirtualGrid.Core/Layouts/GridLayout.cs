using VirtualGrid.Headers;

namespace VirtualGrid.Layouts
{
    public sealed class GridLayout<TProvider, TRowHeaderDeltaListener, TColumnHeaderDeltaListener>
        where TRowHeaderDeltaListener : IGridHeaderDeltaListener
        where TColumnHeaderDeltaListener : IGridHeaderDeltaListener
    {
        internal readonly GridHeader<TRowHeaderDeltaListener> _rowHeader;

        internal readonly GridHeader<TColumnHeaderDeltaListener> _columnHeader;

        public GridLayout(GridHeader<TRowHeaderDeltaListener> rowHeader, GridHeader<TColumnHeaderDeltaListener> columnHeader)
        {
            _rowHeader = rowHeader;
            _columnHeader = columnHeader;
        }

        public GridLayoutBuilder<TProvider, TRowHeaderDeltaListener, TColumnHeaderDeltaListener> GetBuilder()
        {
            return new GridLayoutBuilder<TProvider, TRowHeaderDeltaListener, TColumnHeaderDeltaListener>(this, _rowHeader.GetBuilder(), _columnHeader.GetBuilder());
        }
    }
}

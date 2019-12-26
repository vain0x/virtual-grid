using VirtualGrid.Headers;

namespace VirtualGrid.Layouts
{
    public sealed class GridLayout<TProvider>
    {
        internal readonly GridHeader _rowHeader;

        internal readonly GridHeader _columnHeader;

        public GridLayout(GridHeader rowHeader, GridHeader columnHeader)
        {
            _rowHeader = rowHeader;
            _columnHeader = columnHeader;
        }

        public GridLayoutBuilder<TProvider> GetBuilder()
        {
            return new GridLayoutBuilder<TProvider>(this, _rowHeader.GetBuilder(), _columnHeader.GetBuilder());
        }
    }
}

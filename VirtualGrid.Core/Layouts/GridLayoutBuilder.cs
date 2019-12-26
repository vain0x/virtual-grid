using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;

namespace VirtualGrid.Layouts
{
    public sealed class GridLayoutBuilder<TProvider>
    {
        private GridLayout<TProvider> _inner;

        internal GridHeaderBuilder _rowHeader;

        internal GridHeaderBuilder _columnHeader;

        public GridLayoutBuilder(GridLayout<TProvider> inner, GridHeaderBuilder rowHeader, GridHeaderBuilder columnHeader)
        {
            _inner = inner;
            _rowHeader = rowHeader;
            _columnHeader = columnHeader;
        }

        public GridRow AddRow(object elementKey)
        {
            _rowHeader.Add(elementKey);
            return GridRow.From(elementKey);
        }

        public GridHeaderList AddRowList(object elementKey)
        {
            return _rowHeader.AddList(elementKey);
        }

        public GridColumn AddColumn(object elementKey)
        {
            _columnHeader.Add(elementKey);
            return GridColumn.From(elementKey);
        }

        public void Patch()
        {
            _inner._columnHeader.Patch(0);
            _inner._rowHeader.Patch(0);
        }
    }
}

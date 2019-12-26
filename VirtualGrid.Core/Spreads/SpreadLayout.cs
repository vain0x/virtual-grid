using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;
using VirtualGrid.Layouts;

namespace VirtualGrid.Spreads
{
    public sealed class SpreadLayout<TProvider>
    {
        public GridLayout<TProvider> RowHeader;

        public GridLayout<TProvider> ColumnHeader;

        public SpreadLayout(GridLayout<TProvider> rowHeader, GridLayout<TProvider> columnHeader)
        {
            RowHeader = rowHeader;
            ColumnHeader = columnHeader;
        }
    }
}

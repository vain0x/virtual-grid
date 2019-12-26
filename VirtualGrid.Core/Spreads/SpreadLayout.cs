using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;
using VirtualGrid.Layouts;

namespace VirtualGrid.Spreads
{
    public sealed class SpreadLayout<TRowHeader, TColumnHeader>
    {
        public TRowHeader RowHeader;

        public TColumnHeader ColumnHeader;

        public SpreadLayout(TRowHeader rowHeader, TColumnHeader columnHeader)
        {
            RowHeader = rowHeader;
            ColumnHeader = columnHeader;
        }
    }

    public static class SpreadLayout
    {
        public static SpreadLayout<TRowHeader, TColumnHeader> Create<TRowHeader, TColumnHeader>(TRowHeader rowHeader, TColumnHeader columnHeader)
        {
            return new SpreadLayout<TRowHeader, TColumnHeader>(rowHeader, columnHeader);
        }
    }
}

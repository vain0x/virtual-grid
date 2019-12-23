using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    public interface IGridLayout
    {
        IGridPartLayout RowHeader { get; }

        IGridPartLayout ColumnHeader { get; }

        IGridPartLayout Body { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    public interface IGridPartLayout
    {
        IEnumerable<GridElementKey> Hit(GridVector index);
    }
}

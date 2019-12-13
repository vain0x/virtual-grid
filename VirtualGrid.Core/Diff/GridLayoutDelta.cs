using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Diff
{
    public sealed class GridLayoutDelta
    {
        public readonly object ElementKey;

        public readonly GridRange? OldOpt;

        public readonly GridRange? NewOpt;

        public GridLayoutDelta(object elementKey, GridRange? oldOpt, GridRange? newOpt)
        {
            ElementKey = elementKey;
            OldOpt = oldOpt;
            NewOpt = newOpt;
        }
    }
}

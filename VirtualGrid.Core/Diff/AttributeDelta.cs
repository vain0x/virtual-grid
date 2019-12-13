using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Diff
{
    internal sealed class AttributeDelta
    {
        public readonly GridRange Range;

        public readonly KeyValuePair<string, object>? OldOpt;

        public readonly KeyValuePair<string, object>? NewOpt;

        public AttributeDelta(GridRange range, KeyValuePair<string, object>? oldOpt, KeyValuePair<string, object>? newOpt)
        {
            Range = range;
            OldOpt = oldOpt;
            NewOpt = newOpt;
        }
    }
}

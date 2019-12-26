using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Spreads
{
    public struct SpreadElementKey
    {
        public readonly SpreadPart Part;

        public readonly GridElementKey ElementKey;

        private SpreadElementKey(SpreadPart part, GridElementKey elementKey)
        {
            Part = part;
            ElementKey = elementKey;
        }

        public static SpreadElementKey Create(SpreadPart part, GridElementKey elementKey)
        {
            return new SpreadElementKey(part, elementKey);
        }

        public static SpreadElementKey NewRowHeader(GridElementKey elementKey)
        {
            return new SpreadElementKey(SpreadPart.RowHeader, elementKey);
        }

        public static SpreadElementKey NewColumnHeader(GridElementKey elementKey)
        {
            return new SpreadElementKey(SpreadPart.ColumnHeader, elementKey);
        }

        public static SpreadElementKey NewBody(GridElementKey elementKey)
        {
            return new SpreadElementKey(SpreadPart.Body, elementKey);
        }
    }
}

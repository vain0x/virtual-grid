using System;
using System.Diagnostics;

namespace VirtualGrid.Layouts
{
    public struct GridColumn
    {
        public readonly object ElementKey;

        private GridColumn(object elementKey)
        {
            Debug.Assert(elementKey != null);

            ElementKey = elementKey;
        }

        public static GridColumn From(object elementKey)
        {
            return new GridColumn(elementKey);
        }
    }
}

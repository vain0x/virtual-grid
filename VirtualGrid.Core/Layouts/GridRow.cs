using System.Diagnostics;

namespace VirtualGrid.Layouts
{
    public struct GridRow
    {
        public readonly object ElementKey;

        private GridRow(object elementKey)
        {
            Debug.Assert(elementKey != null);

            ElementKey = elementKey;
        }

        public static GridRow From(object elementKey)
        {
            return new GridRow(elementKey);
        }
    }
}

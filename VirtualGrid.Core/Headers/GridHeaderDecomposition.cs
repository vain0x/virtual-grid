using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Headers
{
    public struct GridHeaderDecomposition
    {
        public readonly IReadOnlyList<object> ElementKeys;

        public readonly bool IsDirty;

        public GridHeaderDecomposition(IReadOnlyList<object> elementKeys, bool isDirty)
        {
            ElementKeys = elementKeys;
            IsDirty = isDirty;
        }
    }
}

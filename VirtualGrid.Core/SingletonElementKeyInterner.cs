using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    public struct SingletonElementKeyInterner
        : IElementKeyInterner
    {
        private readonly object _elementKey;

        public SingletonElementKeyInterner(object elementKey)
        {
            _elementKey = elementKey;
        }

        public int? TryGetIndex(object elementKey)
        {
            if (!EqualityComparer<object>.Default.Equals(_elementKey, elementKey))
                return null;

            return 0;
        }

        public object TryGetKey(int index)
        {
            return _elementKey;
        }
    }
}

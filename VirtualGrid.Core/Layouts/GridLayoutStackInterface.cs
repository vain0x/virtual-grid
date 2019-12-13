using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VirtualGrid.Layouts
{
    public interface IGridLayoutStack
        : IReadOnlyCollection<IGridLayoutProvider>
        , IGridLayoutProvider
    {
        void Add(IGridLayoutProvider layout);
    }

    internal sealed class StackGridLayoutStack
        : IGridLayoutStack
    {
        private readonly List<IGridLayoutProvider> _items =
            new List<IGridLayoutProvider>();

        private readonly bool _horizontal;

        public StackGridLayoutStack(bool horizontal)
        {
            _horizontal = horizontal;
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public IEnumerator<IGridLayoutProvider> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public void Add(IGridLayoutProvider layout)
        {
            _items.Add(layout);
        }

        public IGridLayout ToGridLayout()
        {
            if (_items.Count == 0)
                throw new NotImplementedException();

            return _items
                .Select(provider => provider.ToGridLayout())
                .Aggregate((first, second) =>
                    _horizontal
                        ? new HorizontalLinkGridLayout(first, second).ToGridLayout()
                        : new VerticalLinkGridLayout(first, second).ToGridLayout()
                );
        }
    }
}

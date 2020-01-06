using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;
using VirtualGrid.Layouts;

namespace VirtualGrid.RowsComponents
{
    public sealed class GridRowsComponent<TRowHeaderDeltaListener, TData>
        : IGridElementResolver<TData>
        where TRowHeaderDeltaListener : IGridHeaderDeltaListener
    {
        private GridHeader<TRowHeaderDeltaListener> _rowHeader;

        private IGridHeaderNode _columnHeader;

        private Dictionary<object, TData> _items =
            new Dictionary<object, TData>();

        private Dictionary<object, GridRowsElement<TData>> _elements =
            new Dictionary<object, GridRowsElement<TData>>();

        private Func<TData> _createFunc;

        private Func<int, object> _getRowKey;

        private List<Action> _diff =
            new List<Action>();

        public GridRowsComponent(GridHeader<TRowHeaderDeltaListener> rowHeader, IGridHeaderNode columnHeader, Func<TData> createFunc, Func<int, object> getRowKey)
        {
            _rowHeader = rowHeader;
            _columnHeader = columnHeader;
            _createFunc = createFunc;
            _getRowKey = getRowKey;
        }

        public GridElementHitResult<TData>? Hit(GridVector index)
        {
            var rowKey = _getRowKey(index.Row.Row);
            if (rowKey == null)
                return null;

            TData data;
            if (!_items.TryGetValue(rowKey, out data))
                return null;

            var columnHit = _columnHeader.Hit(index.Column.Column);
            if (!columnHit.HasValue)
                return null;

            var columnKey = columnHit.Value.ElementKey;
            
            return GridElementHitResult.Create(GridElementKey.Create(rowKey, columnKey), data);
        }

        public RowHeaderBuilder GetBuilder()
        {
            return new RowHeaderBuilder(this, _rowHeader.GetBuilder());
        }

        private void Patch()
        {
            var diff = _diff.ToArray();
            _diff.Clear();

            foreach (var delta in diff)
            {
                delta();
            }
        }

        public struct RowHeaderBuilder
        {
            private GridRowsComponent<TRowHeaderDeltaListener, TData> _parent;

            private GridHeaderBuilder<TRowHeaderDeltaListener> _rowHeader;

            public RowHeaderBuilder(GridRowsComponent<TRowHeaderDeltaListener, TData> parent, GridHeaderBuilder<TRowHeaderDeltaListener> rowHeader)
            {
                _parent = parent;
                _rowHeader = rowHeader;
            }

            public void AddRow(object rowKey, Action<object, GridRowElement<TData>> render)
            {
                _rowHeader.Add(rowKey);

                var data = _parent._createFunc();
                _parent._diff.Add(() =>
                {
                    render(rowKey, new GridRowElement<TData>(GridRow.From(rowKey), data));
                });

                _parent._items.Add(rowKey, data);
            }

            public GridRowsElement<TData> AddRowList(object rowListKey, Action<object, GridRowElement<TData>> render)
            {
                var parent = _parent;

                var rowList = _rowHeader.AddList(rowListKey);
                var element = new GridRowsElement<TData>(
                    rowList,
                    _parent._columnHeader,
                    rowKey =>
                    {
                        var data = parent._createFunc();
                        parent._diff.Add(() =>
                        {
                            render(rowKey, new GridRowElement<TData>(GridRow.From(rowKey), data));
                        });
                        parent._items.Add(rowKey, data);
                        return data;
                    },
                    (rowKey, data) =>
                    {
                        parent._diff.Add(() =>
                        {
                            render(rowKey, new GridRowElement<TData>(GridRow.From(rowKey), data));
                        });
                    },
                    (rowKey, data) =>
                    {
                        parent._items.Remove(rowKey);
                    },
                    _parent._getRowKey,
                    _parent.Patch
                );
                return element;
            }

            public void Patch()
            {
                _rowHeader.Patch(0);
                _parent.Patch();
            }
        }
    }
}

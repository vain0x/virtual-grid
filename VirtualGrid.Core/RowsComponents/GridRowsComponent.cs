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

        private Dictionary<GridRow, GridRowElementData<TData>> _items =
            new Dictionary<GridRow, GridRowElementData<TData>>();

        private Func<GridVector, GridElementKey?> _hitFunc;

        private List<KeyValuePair<GridRow, TData>> _diff =
            new List<KeyValuePair<GridRow, TData>>();

        private IGridRowElementDataProvider<TData> _dataProvider;

        public GridRowsComponent(GridHeader<TRowHeaderDeltaListener> rowHeader, IGridHeaderNode columnHeader, IGridRowElementDataProvider<TData> dataProvider, Func<GridVector, GridElementKey?> hitFunc)
        {
            _rowHeader = rowHeader;
            _columnHeader = columnHeader;
            _dataProvider = dataProvider;
            _hitFunc = hitFunc;
        }

        private void AddItem(GridRow row, Action<object, GridRowElement<TData>> renderFunc)
        {
            var data = _dataProvider.Create();

            _diff.Add(Pair.Create(row, data));
            _items.Add(row, new GridRowElementData<TData>(data, renderFunc));
        }

        private void ChangeItem(GridRow row)
        {
            GridRowElementData<TData> data;
            if (!_items.TryGetValue(row, out data))
                return;

            _diff.Add(Pair.Create(row, data.Data));
        }

        private void RemoveItem(GridRow row)
        {
            GridRowElementData<TData> data;
            if (!_items.TryGetValue(row, out data))
                return;

            _diff.Add(Pair.Create(row, data.Data));
            _items.Remove(row);
        }

        public GridElementHitResult<TData>? Hit(GridVector index)
        {
            var keyOpt = _hitFunc(index);
            if (keyOpt.HasValue)
                return null;

            GridRowElementData<TData> data;
            if (!_items.TryGetValue(GridRow.From(keyOpt.Value.RowElementKey), out data))
                return null;

            return GridElementHitResult.Create(keyOpt.Value, data.Data);
        }

        public Builder GetBuilder()
        {
            return new Builder(this, _rowHeader.GetBuilder());
        }

        private void Patch()
        {
            var diff = _diff.ToArray();
            _diff.Clear();

            foreach (var delta in diff)
            {
                _dataProvider.Update(delta.Value);
            }
        }

        public struct Builder
        {
            private GridRowsComponent<TRowHeaderDeltaListener, TData> _parent;

            private GridHeaderBuilder<TRowHeaderDeltaListener> _rowHeader;

            public Builder(GridRowsComponent<TRowHeaderDeltaListener, TData> parent, GridHeaderBuilder<TRowHeaderDeltaListener> rowHeader)
            {
                _parent = parent;
                _rowHeader = rowHeader;
            }

            public void AddRow(object rowKey, Action<object, GridRowElement<TData>> renderFunc)
            {
                var row = GridRow.From(rowKey);

                _rowHeader.Add(rowKey);
                _parent.AddItem(row, renderFunc);
            }

            public GridRowsElement<TData> AddRowList(object rowListKey, Action<object, GridRowElement<TData>> renderFunc)
            {
                var rowList = _rowHeader.AddList(rowListKey);
                var element = new GridRowsElement<TData>(
                    rowList,
                    _parent._columnHeader,
                    new ElementListener(_parent, renderFunc)
                );
                return element;
            }

            public void Patch()
            {
                _rowHeader.Patch(0);
                _parent.Patch();
            }
        }

        public sealed class ElementListener
            : IGridRowsElementListener
        {
            private GridRowsComponent<TRowHeaderDeltaListener, TData> _parent;

            private Action<object, GridRowElement<TData>> _renderFunc;

            public ElementListener(GridRowsComponent<TRowHeaderDeltaListener, TData> parent, Action<object, GridRowElement<TData>> renderFunc)
            {
                _parent = parent;
                _renderFunc = renderFunc;
            }

            public void OnAdd(object rowKey)
            {
                var row = GridRow.From(rowKey);

                _parent.AddItem(row, _renderFunc);
            }

            public void OnChange(object rowKey)
            {
                var row = GridRow.From(rowKey);

                _parent.ChangeItem(row);
            }

            public void OnRemove(object rowKey)
            {
                var row = GridRow.From(rowKey);

                _parent.RemoveItem(row);
            }

            public void Patch()
            {
                _parent.Patch();
            }
        }
    }
}

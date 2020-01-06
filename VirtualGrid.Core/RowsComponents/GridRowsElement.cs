using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;

namespace VirtualGrid.RowsComponents
{
    // 列全体といくつかの行を占有する要素
    public sealed class GridRowsElement<TData>
        : IGridElementResolver<TData>
    {
        public readonly GridHeaderList RowHeader;

        public readonly IGridHeaderNode ColumnHeader;

        private readonly Dictionary<object, TData> _children =
            new Dictionary<object, TData>();

        private readonly Dictionary<object, GridAttributeDeltaKind> _changes =
            new Dictionary<object, GridAttributeDeltaKind>();

        private readonly Func<object, TData> _createFunc;

        private readonly Action<object, TData> _patchFunc;

        private readonly Action<object, TData> _destroyFunc;

        private readonly Func<int, object> _getRowKey;

        private readonly Action _patch;

        public GridRowsElement(GridHeaderList rowHeader, IGridHeaderNode columnHeader, Func<object, TData> createFunc, Action<object, TData> patchFunc, Action<object, TData> destroyFunc, Func<int, object> getRowKey, Action patch)
        {
            RowHeader = rowHeader;
            ColumnHeader = columnHeader;
            _createFunc = createFunc;
            _patchFunc = patchFunc;
            _destroyFunc = destroyFunc;
            _getRowKey = getRowKey;
            _patch = patch;
        }

        public Builder GetBuilder()
        {
            return new Builder(this, RowHeader.GetBuilder());
        }

        public GridElementHitResult<TData>? Hit(GridVector index)
        {
            var rowKey = _getRowKey(index.Row.Row);

            TData element;
            if (!_children.TryGetValue(rowKey, out element))
                return null;

            var columnHit = ColumnHeader.Hit(index.Column.Column);
            if (!columnHit.HasValue)
                return null;

            var columnKey = columnHit.Value.ElementKey;

            return GridElementHitResult.Create(GridElementKey.Create(rowKey, columnKey), element);
        }

        private void Patch()
        {
            var changes = _changes.ToArray();
            _changes.Clear();

            foreach (var pair in changes)
            {
                var rowKey = pair.Key;

                switch (pair.Value)
                {
                    case GridAttributeDeltaKind.Add:
                        {
                            var element = _createFunc(rowKey);
                            _children.Add(rowKey, element);
                            break;
                        }
                    case GridAttributeDeltaKind.Change:
                        {
                            TData element;
                            if (!_children.TryGetValue(rowKey, out element))
                                continue;

                            _patchFunc(rowKey, element);
                            break;
                        }
                    case GridAttributeDeltaKind.Remove:
                        {
                            TData element;
                            if (!_children.TryGetValue(rowKey, out element))
                                continue;

                            _destroyFunc(pair.Key, element);
                            _children.Remove(rowKey);
                            break;
                        }
                    default:
                        throw new Exception("Unknown GridAttributeDeltaKind");
                }
            }

            _patch();
        }

        public struct Builder
        {
            private GridRowsElement<TData> _parent;

            private GridHeaderListBuilder _rowList;

            public Builder(GridRowsElement<TData> parent, GridHeaderListBuilder rowList)
            {
                _parent = parent;
                _rowList = rowList;
            }

            public void Insert(int index, object rowKey)
            {
                _parent._changes[rowKey] = GridAttributeDeltaKind.Add;
                _rowList.Insert(index, rowKey);
            }

            public void RemoveAt(int index, object rowKey)
            {
                _parent._changes[rowKey] = GridAttributeDeltaKind.Remove;
                _rowList.RemoveAt(index);
            }

            public void Update(object rowKey)
            {
                _parent._changes[rowKey] = GridAttributeDeltaKind.Change;
            }

            public void Patch()
            {
                _rowList.Patch();
                _parent.Patch();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Headers;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public sealed class GridRowElement
    {
        public IGridCellAdder<AttributeBuilder> At(GridColumn column)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class GridVerticalStackElement
        : IGridElement<DataGridViewElementData>
    {
        public GridElementHitResult<DataGridViewElementData>? Hit(GridVector index)
        {
            throw new NotImplementedException();
        }

        internal void Add(object itemRows, Action<object, GridRowElement> row)
        {
            throw new NotImplementedException();
        }
    }

    public struct GridElementHitResult<T>
    {
        public readonly GridVector Index;

        public readonly T Data;

        public GridElementHitResult(GridVector index, T data)
        {
            Index = index;
            Data = data;
        }
    }

    public static class GridElementHitResult
    {
        public static GridElementHitResult<T> Create<T>(GridVector index, T data)
        {
            return new GridElementHitResult<T>(index, data);
        }
    }

    public interface IGridElement<T>
    {
        GridElementHitResult<T>? Hit(GridVector index);
    }

    // 列全体といくつかの行を占有する要素
    public sealed class GridRowsElement<TElement, TListener>
        : IGridElement<TElement>
        where TListener : IGridHeaderDeltaListener
    {
        public readonly GridHeaderList RowHeader;

        private readonly Dictionary<object, TElement> _children;

        private readonly Dictionary<object, GridAttributeDeltaKind> _changes =
            new Dictionary<object, GridAttributeDeltaKind>();

        private readonly Func<object, TElement> _creator;

        private readonly Action<object, TElement> _patch;

        private readonly Action<object, TElement> _destroyer;

        private readonly Func<int, object> _getRowKey;

        public GridRowsElement()
        {
        }

        public GridHeaderListBuilder GetBuilder()
        {
            return RowHeader.GetBuilder();
        }

        public GridElementHitResult<TElement>? Hit(GridVector index)
        {
            var rowKey = _getRowKey(index.Row.Row + RowHeader.Offset);

            TElement element;
            if (!_children.TryGetValue(rowKey, out element))
                return null;

            return GridElementHitResult.Create(GridVector.Zero, element);
        }

        public void Patch()
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
                            var element = _creator(rowKey);
                            _children.Add(rowKey, element);
                            break;
                        }
                    case GridAttributeDeltaKind.Change:
                        {
                            TElement element;
                            if (!_children.TryGetValue(rowKey, out element))
                                continue;

                            _patch(rowKey, element);
                            break;
                        }
                    case GridAttributeDeltaKind.Remove:
                        {
                            TElement element;
                            if (!_children.TryGetValue(rowKey, out element))
                                continue;

                            _destroyer(pair.Key, element);
                            _children.Remove(rowKey);
                            break;
                        }
                    default:
                        throw new Exception("Unknown GridAttributeDeltaKind");
                }
            }
        }

        public struct RowHeaderDeltaListener
            : IGridHeaderDeltaListener
        {
            private GridRowsElement<TElement, TListener> _parent;

            public RowHeaderDeltaListener(GridRowsElement<TElement, TListener> parent)
            {
                _parent = parent;
            }

            public void OnInsert(int index, object elementKey)
            {
                var rowKey = elementKey;
                _parent._changes[rowKey] = GridAttributeDeltaKind.Add;
            }

            public void OnRemove(int index)
            {
                var rowKey = _parent._getRowKey(index);
                _parent._changes[rowKey] = GridAttributeDeltaKind.Remove;
            }
        }
    }

    public struct DataGridViewElementData
        : IGridElement<DataGridViewElementData>
    {
        public readonly GridElementKey Key;

        public readonly AttributeBuilder Attributes;

        public DataGridViewElementData(GridElementKey key, AttributeBuilder attributes)
        {
            Key = key;
            Attributes = attributes;
        }

        public GridElementHitResult<DataGridViewElementData>? Hit(GridVector index)
        {
            return null;
        }
    }

    public sealed class GridElementProvider
    {
    }

    public sealed class DataGridViewGridProvider
    {
        internal readonly DataGridView _dataGridView;

        internal readonly Action<GridElementKey, Action> _dispatch;

        internal object _rowHeaderColumnKey;

        internal object _columnHeaderRowKey;

        internal readonly Dictionary<object, DataGridViewRow> _rowMap =
            new Dictionary<object, DataGridViewRow>();

        internal readonly Dictionary<object, DataGridViewColumn> _columnMap =
            new Dictionary<object, DataGridViewColumn>();

        public DataGridViewRowHeaderPart RowHeader;

        public DataGridViewColumnHeaderPart ColumnHeader;

        public DataGridViewBodyPart Body;

        private IGridElement<DataGridViewElementData> _bodyElement;

        public DataGridViewGridProvider(DataGridView inner, IGridElement<DataGridViewElementData> bodyElement, Action<GridElementKey, Action> dispatch)
        {
            _dataGridView = inner;

            _bodyElement = bodyElement;

            _dispatch = (elementKey, action) =>
            {
                Debug.WriteLine("Dispatch({0}, {1})", elementKey, action);
                dispatch(elementKey, action);
            };

            RowHeader = new DataGridViewRowHeaderPart(this);

            ColumnHeader = new DataGridViewColumnHeaderPart(this);

            Body = new DataGridViewBodyPart(this);

            Initialize();
        }

        private void Initialize()
        {
            _dataGridView.CellClick += OnCellClick;
            _dataGridView.CellValueChanged += OnCellValueChanged;
        }

        public void Dispose()
        {
            _dataGridView.CellClick -= OnCellClick;
            _dataGridView.CellValueChanged -= OnCellValueChanged;
        }

        private void OnCellClickCore(IGridElement<DataGridViewElementData> element, GridVector index)
        {
            var hitOpt = element.Hit(index);
            if (!hitOpt.HasValue)
                return;

            var elementKey = hitOpt.Value.Data.Key;
            var attributes = hitOpt.Value.Data.Attributes;

            // チェックボックスのチェックを実装する。
            // FIXME: セルタイプを見る。
            if (attributes.IsCheckedAttribute.IsAttached(elementKey))
            {
                var isChecked = attributes.IsCheckedAttribute.GetValue(elementKey);
                var action = attributes.OnCheckChangedAttribute.GetValue(elementKey);
                if (action != null)
                {
                    _dispatch(elementKey, () => action(!isChecked));
                }
            }

            {
                var action = attributes.OnClickAttribute.GetValue(elementKey);
                if (action != null)
                {
                    _dispatch(elementKey, action);
                }
            }
        }

        private void OnCellClick(object _sender, DataGridViewCellEventArgs ev)
        {
            if (ev.RowIndex < 0 || ev.ColumnIndex < 0)
                return;

            var rowIndex = RowIndex.From(ev.RowIndex);
            var columnIndex = ColumnIndex.From(ev.ColumnIndex);
            var index = GridVector.Create(rowIndex, columnIndex);

            OnCellClickCore(_bodyElement, index);
        }

        private void OnCellValueChangedCore(IGridElement<DataGridViewElementData> element, GridVector index, object value)
        {
            var hitOpt = element.Hit(index);
            if (!hitOpt.HasValue)
                return;

            var elementKey = hitOpt.Value.Data.Key;
            var attributes = hitOpt.Value.Data.Attributes;

            var text = value as string;
            if (text != null || value == null)
            {
                var action = attributes.OnTextChangedAttribute.GetValue(elementKey);
                if (action != null)
                {
                    _dispatch(elementKey, () => action(text ?? ""));
                }
            }
        }

        private void OnCellValueChanged(object _sender, DataGridViewCellEventArgs ev)
        {
            if (ev.RowIndex < 0 || ev.ColumnIndex < 0)
                return;

            var rowIndex = RowIndex.From(ev.RowIndex);
            var columnIndex = ColumnIndex.From(ev.ColumnIndex);
            var index = GridVector.Create(rowIndex, columnIndex);

            var elementKeyOpt = Body.TryGetKey(index);
            if (!elementKeyOpt.HasValue)
                return;

            var cell = Body.TryGetCell(elementKeyOpt.Value);
            if (cell == null)
                return;

            var value = cell.Value;

            OnCellValueChangedCore(_bodyElement, index, value);
        }
    }
}

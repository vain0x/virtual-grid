using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Abstraction;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;

namespace VirtualGrid.WinFormsDemo
{
    public sealed class DataGridViewGridProvider
        : IGridProvider
    {
        internal readonly DataGridView _inner;

        private readonly IGridLayout _layout;

        private readonly Action<GridElementKey, Action> _dispatch;

        public readonly GridRenderContext<DataGridViewGridProvider> _renderContext;

        public readonly GridDataAttributeProvider<bool, IsCheckedAttributePolicy> IsCheckedAttribute;

        public readonly GridEventAttributeProvider<Action<bool>> OnCheckChangedAttribute;

        public readonly GridEventAttributeProvider<Action> OnClickAttribute;

        public readonly GridEventAttributeProvider<Action<string>> OnTextChangedAttribute;

        public readonly GridDataAttributeProvider<bool, ReadOnlyAttributePolicy> ReadOnlyAttribute;

        public readonly GridDataAttributeProvider<string, TextAttributePolicy> TextAttribute;

        internal readonly Dictionary<object, DataGridViewRow> _rowMap =
            new Dictionary<object, DataGridViewRow>();

        internal readonly Dictionary<object, DataGridViewColumn> _columnMap =
            new Dictionary<object, DataGridViewColumn>();

        public DataGridViewGridProvider(DataGridView inner, IGridLayout layout, Action<GridElementKey, Action> dispatch)
        {
            _inner = inner;

            _layout = layout;

            IsCheckedAttribute = GridDataAttributeProvider.Create(default(bool), new IsCheckedAttributePolicy(this), this);

            OnCheckChangedAttribute = new GridEventAttributeProvider<Action<bool>>();

            OnClickAttribute = new GridEventAttributeProvider<Action>();

            OnTextChangedAttribute = new GridEventAttributeProvider<Action<string>>();

            ReadOnlyAttribute = GridDataAttributeProvider.Create(default(bool), new ReadOnlyAttributePolicy(this), this);

            TextAttribute = GridDataAttributeProvider.Create(default(string), new TextAttributePolicy(this), this);

            _renderContext = new GridRenderContext<DataGridViewGridProvider>(this);

            _dispatch = (elementKey, action) =>
            {
                Debug.WriteLine("Dispatch({0}, {1})", elementKey, action);
                dispatch(elementKey, action);
            };

            SubscribeEvents();
        }

        public bool TryGetLocation(GridElementKey elementKey, out GridLocation location)
        {
            var rowIndexOpt = default(RowIndex?);
            var columnIndexOpt = default(ColumnIndex?);

            if (elementKey.RowElementKeyOpt != null)
            {
                DataGridViewRow row;
                if (_rowMap.TryGetValue(elementKey.RowElementKeyOpt, out row))
                {
                    rowIndexOpt = RowIndex.From(row.Index);
                }
            }

            if (elementKey.ColumnElementKeyOpt != null)
            {
                DataGridViewColumn column;
                if (_columnMap.TryGetValue(elementKey.ColumnElementKeyOpt, out column))
                {
                    columnIndexOpt = ColumnIndex.From(column.Index);
                }
            }

            switch (elementKey.GridPart)
            {
                case GridPart.RowHeader:
                    if (rowIndexOpt.HasValue)
                    {
                        location = GridLocation.NewRowHeader(rowIndexOpt.Value.AsVector);
                        return true;
                    }
                    break;

                case GridPart.ColumnHeader:
                    if (columnIndexOpt.HasValue)
                    {
                        location = GridLocation.NewColumnHeader(columnIndexOpt.Value.AsVector);
                        return true;
                    }
                    break;

                case GridPart.Body:
                    if (rowIndexOpt.HasValue && columnIndexOpt.HasValue)
                    {
                        location = GridLocation.NewBody(GridVector.Create(rowIndexOpt.Value, columnIndexOpt.Value));
                        return true;
                    }
                    break;

                default:
                    throw new Exception("Unknown GridPart");
            }

            location = default(GridLocation);
            return false;
        }

        private void OnCellClick(object _sender, DataGridViewCellEventArgs ev)
        {
            // ヘッダーのイベントは未実装
            if (ev.RowIndex < 0 || ev.ColumnIndex < 0)
                return;

            var row = RowIndex.From(ev.RowIndex);
            var column = ColumnIndex.From(ev.ColumnIndex);
            var index = GridVector.Create(row, column);

            foreach (var elementKey in _layout.Body.Hit(index))
            {
                Debug.WriteLine("Click({0})", elementKey);

                // チェックボックスのチェックを実装する。
                // FIXME: セルタイプを見る。
                if (IsCheckedAttribute.IsAttached(elementKey))
                {
                    var isChecked = IsCheckedAttribute.GetValue(elementKey);
                    var action = OnCheckChangedAttribute.GetValue(elementKey);
                    if (action != null)
                    {
                        _dispatch(elementKey, () => action(!isChecked));
                    }
                }

                {
                    var action = OnClickAttribute.GetValue(elementKey);
                    if (action != null)
                    {
                        _dispatch(elementKey, action);
                    }
                }
            }
        }

        private void SubscribeEvents()
        {
            _inner.CellClick += OnCellClick;

            //_inner.CellValueChanged += (sender, ev) =>
            //{
            //    if (ev.RowIndex < 0 || ev.ColumnIndex < 0)
            //        return;

            //    var value = _inner.Rows[ev.RowIndex].Cells[ev.ColumnIndex].Value;

            //    var row = RowIndex.From(ev.RowIndex);
            //    var column = ColumnIndex.From(ev.ColumnIndex);
            //    var index = GridVector.Create(row, column);

            //    foreach (var pair in _locationMap)
            //    {
            //        if (pair.Value.Part == GridPart.Body && pair.Value.Index == index)
            //        {
            //            var elementKey = pair.Key;

            //            {
            //                var text = value as string;
            //                if (text != null || value == null)
            //                {
            //                    var action = OnTextChangedAttribute.GetValue(elementKey);
            //                    if (action != null)
            //                    {
            //                        _dispatch(elementKey, () => action(text ?? ""));
            //                    }
            //                }
            //            }
            //        }
            //    }
            //};
        }

        public void Render()
        {
            IsCheckedAttribute.ApplyDiff();
            OnCheckChangedAttribute.ApplyDiff();
            OnClickAttribute.ApplyDiff();
            OnTextChangedAttribute.ApplyDiff();
            ReadOnlyAttribute.ApplyDiff();
            TextAttribute.ApplyDiff();
        }
    }
}

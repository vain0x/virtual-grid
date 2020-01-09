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

        private IGridElementResolver<AttributeBuilder> _bodyElement;

        public DataGridViewGridProvider(DataGridView inner, Action<GridElementKey, Action> dispatch)
        {
            _dataGridView = inner;

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

        public void SetBody(IGridElementResolver<AttributeBuilder> bodyElement)
        {
            _bodyElement = bodyElement;
        }

        private void OnCellClickCore(IGridElementResolver<AttributeBuilder> element, GridVector index)
        {
            var hitOpt = element.Hit(index);
            if (!hitOpt.HasValue)
                return;

            var elementKey = hitOpt.Value.Key;
            var attributes = hitOpt.Value.Data;

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

        private void OnCellValueChangedCore(IGridElementResolver<AttributeBuilder> element, GridVector index, object value)
        {
            var hitOpt = element.Hit(index);
            if (!hitOpt.HasValue)
                return;

            var elementKey = hitOpt.Value.Key;
            var attributes = hitOpt.Value.Data;

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

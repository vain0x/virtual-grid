using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public sealed class AttributeBuilder
    {
        private readonly DataGridViewGridProvider _provider;

        private readonly IDataGridViewPart _part;

        public readonly GridDataAttributeProvider<bool, IsCheckedAttributePolicy> IsCheckedAttribute;

        public readonly GridEventAttributeProvider<Action<bool>> OnCheckChangedAttribute;

        public readonly GridEventAttributeProvider<Action> OnClickAttribute;

        public readonly GridEventAttributeProvider<Action<string>> OnTextChangedAttribute;

        public readonly GridDataAttributeProvider<bool, ReadOnlyAttributePolicy> ReadOnlyAttribute;

        public readonly GridDataAttributeProvider<string, TextAttributePolicy> TextAttribute;

        public AttributeBuilder(DataGridViewGridProvider provider, IDataGridViewPart part)
        {
            _provider = provider;

            _part = part;

            IsCheckedAttribute = GridDataAttributeProvider.Create(_part, default(bool), new IsCheckedAttributePolicy());

            OnCheckChangedAttribute = new GridEventAttributeProvider<Action<bool>>();

            OnClickAttribute = new GridEventAttributeProvider<Action>();

            OnTextChangedAttribute = new GridEventAttributeProvider<Action<string>>();

            ReadOnlyAttribute = GridDataAttributeProvider.Create(_part, default(bool), new ReadOnlyAttributePolicy());

            TextAttribute = GridDataAttributeProvider.Create(_part, default(string), new TextAttributePolicy());
        }

        public void Attach()
        {
            _provider._dataGridView.CellClick += OnCellClick;
            _provider._dataGridView.CellValueChanged += OnCellValueChanged;
        }

        public void Detach()
        {
            _provider._dataGridView.CellClick -= OnCellClick;
            _provider._dataGridView.CellValueChanged -= OnCellValueChanged;
        }

        private void OnCellClick(object _sender, DataGridViewCellEventArgs ev)
        {
            var rowIndex = RowIndex.From(ev.RowIndex);
            var columnIndex = ColumnIndex.From(ev.ColumnIndex);
            var index = GridVector.Create(rowIndex, columnIndex);

            var elementKeyOpt = _part.TryGetKey(index);
            if (!elementKeyOpt.HasValue)
                return;

            // チェックボックスのチェックを実装する。
            // FIXME: セルタイプを見る。
            if (IsCheckedAttribute.IsAttached(elementKeyOpt.Value))
            {
                var isChecked = IsCheckedAttribute.GetValue(elementKeyOpt.Value);
                var action = OnCheckChangedAttribute.GetValue(elementKeyOpt.Value);
                if (action != null)
                {
                    _provider._dispatch(elementKeyOpt.Value, () => action(!isChecked));
                }
            }

            {
                var action = OnClickAttribute.GetValue(elementKeyOpt.Value);
                if (action != null)
                {
                    _provider._dispatch(elementKeyOpt.Value, action);
                }
            }
        }

        private void OnCellValueChanged(object _sender, DataGridViewCellEventArgs ev)
        {
            var rowIndex = RowIndex.From(ev.RowIndex);
            var columnIndex = ColumnIndex.From(ev.ColumnIndex);
            var index = GridVector.Create(rowIndex, columnIndex);

            var elementKeyOpt = _part.TryGetKey(index);
            if (!elementKeyOpt.HasValue)
                return;

            var cell = _part.TryGetCell(elementKeyOpt.Value);
            if (cell == null)
                return;

            var value = cell.Value;

            var text = value as string;
            if (text != null || value == null)
            {
                var action = OnTextChangedAttribute.GetValue(elementKeyOpt.Value);
                if (action != null)
                {
                    _provider._dispatch(elementKeyOpt.Value, () => action(text ?? ""));
                }
            }
        }

        public void Patch()
        {
            IsCheckedAttribute.Patch();
            OnCheckChangedAttribute.Patch();
            OnClickAttribute.Patch();
            OnTextChangedAttribute.Patch();
            ReadOnlyAttribute.Patch();
            TextAttribute.Patch();
        }

        public IGridCellAdder<AttributeBuilder> At(GridRow row, GridColumn column)
        {
            return new AnonymousGridCellAdder<AttributeBuilder>(() =>
            {
                return new GridCellBuilder<AttributeBuilder>(GridElementKey.Create(row.ElementKey, column.ElementKey), this);
            });
        }
    }
}

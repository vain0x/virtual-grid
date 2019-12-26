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
        public readonly SpreadPart Part;

        private readonly DataGridViewGridProvider _provider;

        public readonly GridDataAttributeProvider<bool, IsCheckedAttributePolicy> IsCheckedAttribute;

        public readonly GridEventAttributeProvider<Action<bool>> OnCheckChangedAttribute;

        public readonly GridEventAttributeProvider<Action> OnClickAttribute;

        public readonly GridEventAttributeProvider<Action<string>> OnTextChangedAttribute;

        public readonly GridDataAttributeProvider<bool, ReadOnlyAttributePolicy> ReadOnlyAttribute;

        public readonly GridDataAttributeProvider<string, TextAttributePolicy> TextAttribute;

        public AttributeBuilder(SpreadPart part, DataGridViewGridProvider provider)
        {
            Part = part;

            _provider = provider;

            IsCheckedAttribute = GridDataAttributeProvider.Create(Part, default(bool), new IsCheckedAttributePolicy(provider), provider);

            OnCheckChangedAttribute = new GridEventAttributeProvider<Action<bool>>();

            OnClickAttribute = new GridEventAttributeProvider<Action>();

            OnTextChangedAttribute = new GridEventAttributeProvider<Action<string>>();

            ReadOnlyAttribute = GridDataAttributeProvider.Create(Part, default(bool), new ReadOnlyAttributePolicy(provider), provider);

            TextAttribute = GridDataAttributeProvider.Create(Part, default(string), new TextAttributePolicy(provider), provider);
        }

        public void Attach()
        {
            _provider._inner.CellClick += OnCellClick;
            _provider._inner.CellValueChanged += OnCellValueChanged;
        }

        public void Detach()
        {
            _provider._inner.CellClick -= OnCellClick;
            _provider._inner.CellValueChanged -= OnCellValueChanged;
        }

        private void OnCellClick(object _sender, DataGridViewCellEventArgs ev)
        {
            // ヘッダーのイベントは未実装
            if (ev.RowIndex < 0 || ev.ColumnIndex < 0)
                return;

            var rowElementKey = new DataGridViewRowElementKeyInterner(_provider).TryGetKey(ev.RowIndex);
            if (rowElementKey == null)
                return;

            var columnElementKey = new DataGridViewColumnElementKeyInterner(_provider).TryGetKey(ev.ColumnIndex);
            if (columnElementKey == null)
                return;

            foreach (var elementKey in new[] { GridElementKey.Create(rowElementKey, columnElementKey) })
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
                        _provider._dispatch(elementKey, () => action(!isChecked));
                    }
                }

                {
                    var action = OnClickAttribute.GetValue(elementKey);
                    if (action != null)
                    {
                        _provider._dispatch(elementKey, action);
                    }
                }
            }
        }

        private void OnCellValueChanged(object _sender, DataGridViewCellEventArgs ev)
        {
            if (ev.RowIndex < 0 || ev.ColumnIndex < 0)
                return;

            var rowElementKey = new DataGridViewRowElementKeyInterner(_provider).TryGetKey(ev.RowIndex);
            if (rowElementKey == null)
                return;

            var columnElementKey = new DataGridViewColumnElementKeyInterner(_provider).TryGetKey(ev.ColumnIndex);
            if (columnElementKey == null)
                return;

            var value = _provider._inner.Rows[ev.RowIndex].Cells[ev.ColumnIndex].Value;

            foreach (var elementKey in new[] { GridElementKey.Create(rowElementKey, columnElementKey) })
            {
                Debug.WriteLine("ValueChanged({0})", elementKey);

                var text = value as string;
                if (text != null || value == null)
                {
                    var action = OnTextChangedAttribute.GetValue(elementKey);
                    if (action != null)
                    {
                        _provider._dispatch(elementKey, () => action(text ?? ""));
                    }
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

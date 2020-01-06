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
using RowHeaderLayout = VirtualGrid.Layouts.GridLayout<
    VirtualGrid.WinFormsDemo.DataGridViewGridProvider,
    VirtualGrid.WinFormsDemo.DataGridViewRowHeaderPart.RowHeaderDeltaListener,
    VirtualGrid.WinFormsDemo.DataGridViewRowHeaderPart.ColumnHeaderDeltaListener
>;
using ColumnHeaderLayout = VirtualGrid.Layouts.GridLayout<
    VirtualGrid.WinFormsDemo.DataGridViewGridProvider,
    VirtualGrid.WinFormsDemo.DataGridViewColumnHeaderPart.RowHeaderDeltaListener,
    VirtualGrid.WinFormsDemo.DataGridViewColumnHeaderPart.ColumnHeaderDeltaListener
>;
using VirtualGrid.RowsComponents;

namespace VirtualGrid.WinFormsDemo.Examples
{
    public sealed class TodoListView
    {
        private readonly TodoListModel _model;

        private readonly DataGridViewGridProvider _provider;

        private readonly SpreadLayout<RowHeaderLayout, ColumnHeaderLayout> _layout;

        private readonly GridRowsComponent<DataGridViewRowHeaderPart.RowHeaderDeltaListener, AttributeBuilder> _body;

        private GridRowsElement<AttributeBuilder> _items;

        private GridColumn
            _checkBoxColumn,
            _textColumn,
            _addButtonColumn,
            _deleteButtonColumn;

        public TodoListView(TodoListModel model, DataGridView dataGridView, Action<GridElementKey, Action> dispatch)
        {
            _model = model;

            _provider = new DataGridViewGridProvider(dataGridView, dispatch);

            var rowHeader = new RowHeaderLayout(
                new GridHeader<DataGridViewRowHeaderPart.RowHeaderDeltaListener>(
                    "KEY_SPREAD_ROW_HEADER_ROW_HEADER",
                    new DataGridViewRowHeaderPart.RowHeaderDeltaListener(_provider)
                ),
                new GridHeader<DataGridViewRowHeaderPart.ColumnHeaderDeltaListener>(
                    "KEY_SPREAD_ROW_HEADER_COLUMN_HEADER",
                    new DataGridViewRowHeaderPart.ColumnHeaderDeltaListener(_provider)
                ));

            var columnHeader = new ColumnHeaderLayout(
                new GridHeader<DataGridViewColumnHeaderPart.RowHeaderDeltaListener>(
                    "KEY_SPREAD_COLUMN_HEADER_ROW_HEADER",
                    new DataGridViewColumnHeaderPart.RowHeaderDeltaListener(_provider)
                ),
                new GridHeader<DataGridViewColumnHeaderPart.ColumnHeaderDeltaListener>(
                    "KEY_SPREAD_COLUMN_HEADER_COLUMN_HEADER",
                    new DataGridViewColumnHeaderPart.ColumnHeaderDeltaListener(_provider)
                ));

            _layout = SpreadLayout.Create(
                rowHeader,
                columnHeader
            );

            _body = new GridRowsComponent<DataGridViewRowHeaderPart.RowHeaderDeltaListener, AttributeBuilder>(
                rowHeader._rowHeader,
                columnHeader._columnHeader,
                () => new AttributeBuilder(_provider.Body),
                row => _provider.Body.TryGetRowKey(RowIndex.From(row))
            );

            _provider.SetBody(_body);
        }

        public void Initialize()
        {
            InitializeColumnHeader();
            InitializeRowHeader();

            Update();
        }

        private void InitializeColumnHeader()
        {
            var l = _layout.ColumnHeader.GetBuilder();
            var row = l.AddRow("KEY_COLUMN_HEADER_ROW");

            _checkBoxColumn = l.AddColumn("KEY_CHECK_BOX_COLUMN");
            _textColumn = l.AddColumn("KEY_TEXT_COLUMN");
            _addButtonColumn = l.AddColumn("KEY_ADD_BUTTON_COLUMN");
            _deleteButtonColumn = l.AddColumn("KEY_DELETE_BUTTON_COLUMN");
            l.Patch();

            var a = new AttributeBuilder(_provider.ColumnHeader);
            a.At(row, _checkBoxColumn)
                .AddText("選択");

            a.At(row, _textColumn)
                .AddText("テキスト");

            a.At(row, _addButtonColumn)
                .AddText("操作");

            a.At(row, _deleteButtonColumn)
                .AddText("");
            a.Patch();
        }

        private void InitializeRowHeader()
        {
            var rows = _body.GetBuilder();
            _items = rows.AddRowList("KEY_ITEM_ROWS", (item, row) =>
            {
                RenderItem((TodoItem)item, row);
            });

            rows.AddRow("KEY_FOOTER_ROW", (_key, row) =>
            {
                RenderFooter(row);
            });
            rows.Patch();
        }

        private void RenderItem(TodoItem item, GridRowElement<AttributeBuilder> row)
        {
            row.At(_checkBoxColumn)
                .AddCheckBox(item.IsDone)
                .OnCheckChanged(isDone =>
                {
                    _model.SetIsDone(item, isDone);
                });

            row.At(_textColumn)
                .AddEdit(item.Text)
                .OnTextChanged(text =>
                {
                    _model.SetItemText(item, text);
                });

            row.At(_addButtonColumn)
                .AddButton("上に追加")
                .OnClick(() =>
                {
                    _model.InsertBefore(item);
                });

            row.At(_deleteButtonColumn)
                .AddButton("削除")
                .OnClick(() =>
                {
                    _model.Remove(item);
                });

            row.Data.Patch();
        }

        private void RenderFooter(GridRowElement<AttributeBuilder> footer)
        {
            footer.At(_textColumn)
                .AddText("");

            footer.At(_addButtonColumn)
                .AddButton("上に追加")
                .OnClick(() =>
                {
                    _model.InsertLast();
                });

            footer.Data.Patch();
        }

        private void UpdateItems()
        {
            var items = _items.GetBuilder();
            foreach (var delta in _model.DrainDiff())
            {
                if (delta.Kind == "INSERT")
                {
                    items.Insert(delta.Index, delta.Item);
                }
                else if (delta.Kind == "REMOVE")
                {
                    items.RemoveAt(delta.Index, delta.Item);
                }
                else
                {
                    items.Update(delta.Item);
                }
            }
            items.Patch();
        }

        public void Update()
        {
            UpdateItems();
        }
    }
}

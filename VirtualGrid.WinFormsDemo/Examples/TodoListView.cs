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

namespace VirtualGrid.WinFormsDemo.Examples
{
    public sealed class TodoListView
    {
        private readonly TodoListModel _model;

        private readonly DataGridViewGridProvider _provider;

        private readonly SpreadLayout<RowHeaderLayout, ColumnHeaderLayout> _layout;

        private readonly GridRowElementsComponent _body;

        public TodoListView(TodoListModel model, DataGridView dataGridView, Action<GridElementKey, Action> dispatch)
        {
            _model = model;

            _body = new GridRowElementsComponent();

            _provider = new DataGridViewGridProvider(dataGridView, _body, dispatch);

            var rhrh = "KEY_SPREAD_ROW_HEADER_ROW_HEADER";
            var rhch = "KEY_SPREAD_ROW_HEADER_COLUMN_HEADER";
            var chrh = "KEY_SPREAD_COLUMN_HEADER_ROW_HEADER";
            var chch = "KEY_SPREAD_COLUMN_HEADER_COLUMN_HEADER";

            var rowHeader = new RowHeaderLayout(
                new GridHeader<DataGridViewRowHeaderPart.RowHeaderDeltaListener>(
                    rhrh,
                    new DataGridViewRowHeaderPart.RowHeaderDeltaListener(_provider)
                ),
                new GridHeader<DataGridViewRowHeaderPart.ColumnHeaderDeltaListener>(
                    rhch,
                    new DataGridViewRowHeaderPart.ColumnHeaderDeltaListener(_provider)
                ));

            var columnHeader = new ColumnHeaderLayout(
                new GridHeader<DataGridViewColumnHeaderPart.RowHeaderDeltaListener>(
                    chrh,
                    new DataGridViewColumnHeaderPart.RowHeaderDeltaListener(_provider)
                ),
                new GridHeader<DataGridViewColumnHeaderPart.ColumnHeaderDeltaListener>(
                    chch,
                    new DataGridViewColumnHeaderPart.ColumnHeaderDeltaListener(_provider)
                ));

            _layout = SpreadLayout.Create(
                rowHeader,
                columnHeader
            );
        }

        private GridRowElementsComponent _itemRows;

        private GridRow _footerRow;

        private GridColumn
            _checkBoxColumn,
            _textColumn,
            _addButtonColumn,
            _deleteButtonColumn;

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
            //var l = _layout.RowHeader.GetBuilder();
            //_itemRows = l.AddRowList("KEY_ITEM_ROWS");
            //_footerRow = l.AddRow("KEY_FOOTER_ROW");
            //l.Patch();

            var rowElements = _body.GetBuilder();
            rowElements.AddRowList("KEY_ITEM_ROWS", (item, row) => RenderItem((TodoItem)item, row));
            rowElements.AddRow("KEY_FOOTER_ROW", (_key, row) => RenderFooter(row));
            rowElements.Patch();
        }

        private void RenderItem(TodoItem item, DataGridViewRowElement row)
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
        }

        private void RenderFooter(DataGridViewRowElement footer)
        {
            footer.At(_textColumn)
                .AddText("");

            footer.At(_addButtonColumn)
                .AddButton("上に追加")
                .OnClick(() =>
                {
                    _model.InsertLast();
                });
        }

        private void UpdateItems()
        {
            var dirtyItems = _model._dirtyItems.ToArray();
            _model._dirtyItems.Clear();

            var rows = _itemRows.GetBuilder();
            foreach (var delta in dirtyItems)
            {
                if (delta.Kind == "INSERT")
                {
                    rows.Insert(delta.Index, delta.Item);
                }
                else if (delta.Kind == "REMOVE")
                {
                    rows.RemoveAt(delta.Index);
                }
                else
                {
                    rows.Update(delta.Index, delta.Item);
                }
            }
            rows.Patch();
        }

        public void Update()
        {
            UpdateItems();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public TodoListView(TodoListModel model, DataGridViewGridProvider provider)
        {
            _model = model;
            _provider = provider;

            var rhrh = "KEY_SPREAD_ROW_HEADER_ROW_HEADER";
            var rhch = "KEY_SPREAD_ROW_HEADER_COLUMN_HEADER";
            var chrh = "KEY_SPREAD_COLUMN_HEADER_ROW_HEADER";
            var chch = "KEY_SPREAD_COLUMN_HEADER_COLUMN_HEADER";

            var rowHeader = new RowHeaderLayout(
                new GridHeader<DataGridViewRowHeaderPart.RowHeaderDeltaListener>(
                    rhrh,
                    new DataGridViewRowElementKeyInterner(provider),
                    new DataGridViewRowHeaderPart.RowHeaderDeltaListener(provider)
                ),
                new GridHeader<DataGridViewRowHeaderPart.ColumnHeaderDeltaListener>(
                    rhch,
                    new SingletonElementKeyInterner(rhch),
                    new DataGridViewRowHeaderPart.ColumnHeaderDeltaListener(provider)
                ));

            var columnHeader = new ColumnHeaderLayout(
                new GridHeader<DataGridViewColumnHeaderPart.RowHeaderDeltaListener>(
                    chrh,
                    new SingletonElementKeyInterner(chrh),
                    new DataGridViewColumnHeaderPart.RowHeaderDeltaListener(provider)
                ),
                new GridHeader<DataGridViewColumnHeaderPart.ColumnHeaderDeltaListener>(
                    chch,
                    new DataGridViewColumnElementKeyInterner(provider),
                    new DataGridViewColumnHeaderPart.ColumnHeaderDeltaListener(provider)
                ));

            _layout = SpreadLayout.Create(
                rowHeader,
                columnHeader
            );
        }

        private GridHeaderList _itemRows;

        private GridRow _footerRow;

        private GridColumn
            _checkBoxColumn,
            _textColumn,
            _addButtonColumn,
            _deleteButtonColumn;

        private Dictionary<TodoItem, AttributeBuilder> _items =
            new Dictionary<TodoItem, AttributeBuilder>();

        public void Initialize()
        {
            InitializeColumnHeader();
            InitializeRowHeader();
            InitializeBody();

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

            var a = new AttributeBuilder(_provider, _provider.ColumnHeader);
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
            var l = _layout.RowHeader.GetBuilder();
            _itemRows = l.AddRowList("KEY_ITEM_ROWS");
            _footerRow = l.AddRow("KEY_FOOTER_ROW");
            l.Patch();
        }

        public void InitializeBody()
        {
            RenderFooterRow();
        }

        private void RenderItem(GridRow row, TodoItem item)
        {
            AttributeBuilder a;
            if (!_items.TryGetValue(item, out a))
            {
                a = new AttributeBuilder(_provider, _provider.Body);
                _items.Add(item, a);
                a.Attach();
            }

            a.At(row, _checkBoxColumn)
                .AddCheckBox(item.IsDone)
                .OnCheckChanged(isDone =>
                {
                    _model.SetIsDone(item, isDone);
                });

            a.At(row, _textColumn)
                .AddEdit(item.Text)
                .OnTextChanged(text =>
                {
                    _model.SetItemText(item, text);
                });

            a.At(row, _addButtonColumn)
                .AddButton("上に追加")
                .OnClick(() =>
                {
                    _model.InsertBefore(item);
                });

            a.At(row, _deleteButtonColumn)
                .AddButton("削除")
                .OnClick(() =>
                {
                    _model.Remove(item);
                });
            a.Patch();
        }

        private void RenderFooterRow()
        {
            var footer = new AttributeBuilder(_provider, _provider.Body);
            footer.Attach();

            footer.At(_footerRow, _textColumn)
                .AddText("");

            footer.At(_footerRow, _addButtonColumn)
                .AddButton("上に追加")
                .OnClick(() =>
                {
                    _model.InsertLast();
                });
            footer.Patch();
        }

        private void UpdateItems()
        {
            var itemRows = _itemRows.GetBuilder();
            foreach (var delta in _model._dirtyItems)
            {
                if (delta.Kind == "INSERT")
                {
                    itemRows.Insert(delta.Index, delta.Item);
                }
                else if (delta.Kind == "REMOVE")
                {
                    itemRows.Remove(delta.Item);
                }
                else
                {
                    // Pass.
                }
            }
            itemRows.Patch();

            foreach (var delta in _model._dirtyItems.ToArray())
            {
                if (delta.Kind == "REMOVE")
                {
                    AttributeBuilder a;
                    if (_items.TryGetValue(delta.Item, out a))
                    {
                        a.Detach();
                    }
                    _items.Remove(delta.Item);
                }
                else
                {
                    RenderItem(GridRow.From(delta.Item), delta.Item);
                }
            }

            _model._dirtyItems.Clear();
        }

        public void Update()
        {
            UpdateItems();
        }
    }
}

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

namespace VirtualGrid.WinFormsDemo.Examples
{
    public struct SingletonElementKeyInterner
        : IElementKeyInterner
    {
        private readonly object _elementKey;

        public SingletonElementKeyInterner(object elementKey)
        {
            _elementKey = elementKey;
        }

        public int? TryGetIndex(object elementKey)
        {
            if (!EqualityComparer<object>.Default.Equals(_elementKey, elementKey))
                return null;

            return 0;
        }

        public object TryGetKey(int index)
        {
            return _elementKey;
        }
    }

    public sealed class TodoListView
    {
        private readonly TodoListModel _model;

        private readonly DataGridViewGridProvider _provider;

        private readonly SpreadLayout<DataGridViewGridProvider> _layout;

        public TodoListView(TodoListModel model, DataGridViewGridProvider provider)
        {
            _model = model;
            _provider = provider;

            var rhrh = "KEY_SPREAD_ROW_HEADER_ROW_HEADER";
            var rhch = "KEY_SPREAD_ROW_HEADER_COLUMN_HEADER";
            var chrh = "KEY_SPREAD_COLUMN_HEADER_ROW_HEADER";
            var chch = "KEY_SPREAD_COLUMN_HEADER_COLUMN_HEADER";

            var rowHeader = new GridLayout<DataGridViewGridProvider>(
                new GridHeader(
                    rhrh,
                    new DataGridViewRowElementKeyInterner(provider),
                    new Provider.Headers.RowHeaderRowHeaderDeltaListener(provider)
                ),
                new GridHeader(
                    rhch,
                    new SingletonElementKeyInterner(rhch),
                    new Provider.Headers.RowHeaderColumnHeaderDeltaListener()
                ));

            var columnHeader = new GridLayout<DataGridViewGridProvider>(
                new GridHeader(
                    chrh,
                    new SingletonElementKeyInterner(chrh),
                    new Provider.Headers.ColumnHeaderRowHeaderDeltaListener()
                ),
                new GridHeader(
                    chch,
                    new DataGridViewColumnElementKeyInterner(provider),
                    new Provider.Headers.ColumnHeaderColumnHeaderDeltaListener(provider)
                ));

            _layout = new SpreadLayout<DataGridViewGridProvider>(
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

        // FIXME: キーが削除されたら detach する。
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

            var a = new AttributeBuilder(SpreadPart.ColumnHeader, _provider);
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
            AttributeBuilder _body;
            if (!_items.TryGetValue(item, out _body))
            {
                _body = new AttributeBuilder(SpreadPart.Body, _provider);
                _items.Add(item, _body);
                _body.Attach();
            }

            _body.At(row, _checkBoxColumn)
                .AddCheckBox(item.IsDone)
                .OnCheckChanged(isDone =>
                {
                    _model.SetIsDone(item, isDone);
                });

            _body.At(row, _textColumn)
                .AddEdit(item.Text)
                .OnTextChanged(text =>
                {
                    _model.SetItemText(item, text);
                });

            _body.At(row, _addButtonColumn)
                .AddButton("上に追加")
                .OnClick(() =>
                {
                    _model.InsertBefore(item);
                });

            _body.At(row, _deleteButtonColumn)
                .AddButton("削除")
                .OnClick(() =>
                {
                    _model.Remove(item);
                });
            _body.Patch();
        }

        private void RenderFooterRow()
        {
            var footer = new AttributeBuilder(SpreadPart.Body, _provider);
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
                if (delta.Kind != "REMOVE")
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

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

        private readonly GridRowsElement<object, DataGridViewElement> _bodyElement;

        public TodoListView(TodoListModel model, DataGridView dataGridView, Action<GridElementKey, Action> dispatch)
        {
            _model = model;

            _bodyElement = new GridRowsElement<object, DataGridViewElement>(rowHeader);

            _provider = new DataGridViewGridProvider(dataGridView, _bodyElement, dispatch);

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

        private GridHeaderList _itemRows;

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
            var l = _layout.RowHeader.GetBuilder();
            _itemRows = l.AddRowList("KEY_ITEM_ROWS");
            _footerRow = l.AddRow("KEY_FOOTER_ROW");
            l.Patch();
        }

        public void InitializeBody()
        {
            RenderFooterRow();
        }

        private void RenderItem(TodoItem item, AttributeBuilder a)
        {
            var row = GridRow.From(item);

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
        }

        private void RenderFooterRow()
        {
            var footer = new AttributeBuilder(_provider.Body);
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

        #region items

        private Dictionary<TodoItem, AttributeBuilder> _items =
            new Dictionary<TodoItem, AttributeBuilder>();

        private AttributeBuilder TouchElement(TodoItem item)
        {
            AttributeBuilder a;
            if (!_items.TryGetValue(item, out a))
            {
                a = new AttributeBuilder(_provider, _provider.Body);
                _items.Add(item, a);
                a.Attach();
            }

            return a;
        }

        private void DestroyElement(TodoItem item)
        {
            AttributeBuilder a;
            if (_items.TryGetValue(item, out a))
            {
                a.Detach();
            }
            _items.Remove(item);
        }

        #endregion

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
                    itemRows.RemoveAt(delta.Index);
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
                    DestroyElement(delta.Item);
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

        public sealed class TodoItemElementProvider
            : IGridElementProvider<TodoItem, DataGridViewElement>
        {
            private TodoListView _view;

            public DataGridViewElement Create(TodoItem key)
            {
                return new DataGridViewElement(
                    GridElementKey.Create(key, _view._checkBoxColumn),
                    new AttributeBuilder(_view._provider.Body)
                );
            }

            public void Update(TodoItem key, DataGridViewElement item)
            {
                _view.RenderItem(key, item.Attributes);
                item.Attributes.Patch();
            }

            public void Destroy(TodoItem key, DataGridViewElement item)
            {
                item.Attributes.Patch();
                item.Attributes.Patch();
            }
        }
    }

    public interface IGridElementProvider<TKey, T>
    {
        T Create(TKey key);

        void Update(TKey key, T item);

        void Destroy(TKey key, T item);
    }
}

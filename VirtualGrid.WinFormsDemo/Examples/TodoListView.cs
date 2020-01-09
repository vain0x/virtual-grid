using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Layouts;
using VirtualGrid.RowsComponents;
using VirtualGrid.WinFormsDemo.Provider.RowsComponents;

namespace VirtualGrid.WinFormsDemo.Examples
{
    public sealed class TodoListView
    {
        private readonly TodoListModel _model;

        private readonly DataGridViewGridProvider _provider;

        private readonly DataGridViewSpreadLayout _layout;

        private readonly DataGridViewRowsComponent _component;

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

            _layout = new DataGridViewSpreadLayout(_provider);

            _component = new DataGridViewRowsComponent(_layout, _provider);
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
            var rows = _component.GetBuilder();
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

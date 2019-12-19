using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;

namespace VirtualGrid.WinFormsDemo.Examples
{
    public sealed class TodoListView
    {
        private readonly TodoListModel _model;

        private readonly GridBuilder<DataGridViewGridProvider> _h;

        public TodoListView(TodoListModel model, GridBuilder<DataGridViewGridProvider> h)
        {
            _model = model;
            _h = h;
        }

        public void Render()
        {
            // カラムヘッダー
            var ch = _h.ColumnHeader;

            var checkColumn = ch
                .WithKey("?チェック列")
                .AddText("[✔]");

            var textColumn = ch
                .WithKey("?テキスト列")
                .AddText("テキスト");

            var addButtonColumn = ch
                .WithKey("?追加ボタン列")
                .AddText("操作");

            var deleteButtonColumn = ch
                .WithKey("?削除ボタン列")
                .AddText("");

            // ローヘッダーとボディー
            var rh = _h.RowHeader;
            var body = _h.Body;

            foreach (var item in _model.Items)
            {
                var row = rh
                    .WithKey(item)
                    .AddText("");

                body.At(row, checkColumn)
                    .AddCheckBox(item.IsDone)
                    .OnCheckChanged(isDone =>
                    {
                        _model.SetIsDone(item, isDone);
                    });

                body.At(row, textColumn)
                    .AddEdit(item.Text)
                    .OnTextChanged(text =>
                    {
                        _model.SetItemText(item, text);
                    });

                body.At(row, addButtonColumn)
                    .AddText("[上に追加]")
                    .OnClick(() =>
                    {
                        _model.InsertBefore(item);
                    });

                body.At(row, deleteButtonColumn)
                    .AddText("[削除]")
                    .OnClick(() =>
                    {
                        _model.Remove(item);
                    });
            }

            // 新規追加
            {
                var row = rh
                    .WithKey("?新規追加行")
                    .AddText("");

                body.At(row, addButtonColumn)
                    .AddText("[上に追加]")
                    .OnClick(() =>
                    {
                        _model.InsertLast();
                    });

                body.At(row, deleteButtonColumn)
                    .AddText(string.Format("{0}件", _model.NonBlankCount()));
            }
        }
    }
}

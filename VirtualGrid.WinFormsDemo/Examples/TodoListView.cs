using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Layouts;
using VirtualGrid.Models;

namespace VirtualGrid.WinFormsDemo.Examples
{
    public sealed class TodoListView
    {
        private readonly TodoListModel _model;

        private readonly VGridBuilder _h;

        public TodoListView(TodoListModel model, VGridBuilder h)
        {
            _model = model;
            _h = h;
        }

        public IGridLayoutProvider Render()
        {
            var body = _h.NewColumn();

            {
                var row = _h.NewRow();

                row.Add(_h.NewText("[✔]", "?チェック列"));
                row.Add(_h.NewText("テキスト", "?テキスト列"));
                row.Add(_h.NewText("操作", "?追加ボタン列"));
                row.Add(_h.NewText("", "?削除ボタン列"));

                body.Add(row);
            }

            foreach (var item in _model.Items)
            {
                var row = _h.NewRow();

                row.Add(_h.NewText("[ ]"));

                row.Add(_h.NewEdit(item.Text, item).OnTextChanged(text =>
                {
                    _model.SetItemText(item, text);
                }));

                row.Add(_h.NewText("[上に追加]").OnClick(() =>
                {
                    _model.InsertBefore(item);
                }));

                row.Add(_h.NewText("[削除]").OnClick(() =>
                {
                    _model.Remove(item);
                }));

                body.Add(row);
            }

            // 新規追加
            {
                var row = _h.NewRow();

                row.Add(_h.NewText(""));
                row.Add(_h.NewText(""));

                row.Add(_h.NewText("[上に追加]").OnClick(() =>
                {
                    _model.InsertLast();
                }));

                row.Add(_h.NewText(string.Format("{0}件", _model.NonBlankCount()), "?小計"));

                body.Add(row);
            }
            return body;
        }
    }
}

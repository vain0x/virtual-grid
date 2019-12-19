using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;

namespace VirtualGrid.WinFormsDemo.Examples
{
    public sealed class TinyCounterView
    {
        readonly TinyCounterModel _model;

        readonly GridBuilder _h;

        public TinyCounterView(TinyCounterModel model, GridBuilder h)
        {
            _model = model;
            _h = h;
        }

        public void Render()
        {
            var ch = _h.ColumnHeader;
            var rh = _h.RowHeader;
            var body = _h.Body;

            // カラムヘッダー
            var incrementButtonColumn = ch
                .WithKey("?追加ボタン列")
                .AddCell();

            var decrementButtonColumn = ch
                .WithKey("?削除ボタン列")
                .AddCell();

            var countColumn = ch
                .WithKey("?カウント列")
                .AddText("カウント");

            // 1行目
            {
                var row = rh
                    .WithKey("?1行目")
                    .AddCell();

                body.At(row, incrementButtonColumn)
                    .AddButton("[+]")
                    .OnClick(() => _model.Increment());

                body.At(row, decrementButtonColumn)
                    .AddButton("[-]")
                    .OnClick(() => _model.Decrement());

                body.At(row, countColumn)
                    .AddText(_model.Count.ToString());
            }
        }
    }
}

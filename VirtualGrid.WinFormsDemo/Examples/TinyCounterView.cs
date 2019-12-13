using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Layouts;
using VirtualGrid.Models;

namespace VirtualGrid.WinFormsDemo.Examples
{
    public sealed class TinyCounterView
    {
        readonly TinyCounterModel _state;

        readonly VGridBuilder _h;

        public TinyCounterView(TinyCounterModel state, VGridBuilder h)
        {
            _state = state;
            _h = h;
        }

        public IGridLayoutProvider Render()
        {
            var body = _h.NewColumn();
            {
                var row = _h.NewRow();
                row.Add(_h.NewText(""));
                row.Add(_h.NewText(""));
                row.Add(_h.NewText("Count"));
                body.Add(row);
            }

            {
                var increment = _h.NewButton("[+]").OnClick("INCREMENT");
                var decrement = _h.NewButton("[-]").OnClick("DECREMENT");
                var count = _h.NewText(_state.Count.ToString());

                var row = _h.NewRow();
                row.Add(increment);
                row.Add(decrement);
                row.Add(count);
                body.Add(row);
            }
            return body;
        }
    }
}

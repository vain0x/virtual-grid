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
        private readonly TinyCounterModel _model;

        public TinyCounterView(TinyCounterModel model)
        {
            _model = model;
        }

        public void Render()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.WinFormsDemo.Examples
{
    public sealed class TinyCounterModel
    {
        public int Count { get; private set; }

        public void Increment()
        {
            Count++;
        }

        public void Decrement()
        {
            Count--;
        }
    }
}

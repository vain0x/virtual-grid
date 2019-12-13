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

        public void Update(string action)
        {
            if (action == "INCREMENT")
            {
                Increment();
                return;
            }

            if (action == "DECREMENT")
            {
                Decrement();
                return;
            }
        }
    }
}

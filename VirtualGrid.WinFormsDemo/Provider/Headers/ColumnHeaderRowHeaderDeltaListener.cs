using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;

namespace VirtualGrid.WinFormsDemo.Provider.Headers
{
    public struct ColumnHeaderRowHeaderDeltaListener
        : IGridHeaderDeltaListener
    {
        public void OnInsert(object elementKey, int index)
        { 
            Debug.WriteLine("CHCH({0}) insert at {1}", elementKey, index);
        }

        public void OnRemove(int index)
        {
            Debug.WriteLine("CHCH remove at {1}", index);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;

namespace VirtualGrid.WinFormsDemo.Provider.Headers
{
    public struct RowHeaderColumnHeaderDeltaListener
        : IGridHeaderDeltaListener
    {
        public void OnInsert(int index, object elementKey)
        {
            Debug.WriteLine("RowHeaderColumnHeader({0}) insert at ({1})", elementKey, index);
        }

        public void OnRemove(int index)
        {
            Debug.WriteLine("RowHeaderColumnHeader remove at ({0})", index);
        }
    }
}

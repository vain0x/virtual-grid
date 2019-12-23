using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Headers;

namespace VirtualGrid.WinFormsDemo.Provider.Headers
{
    public struct RowHeaderDeltaListener
        : IGridHeaderDeltaListener
    {
        private readonly DataGridViewGridProvider _provider;

        public RowHeaderDeltaListener(DataGridViewGridProvider provider)
        {
            _provider = provider;
        }

        public void OnInsert(object elementKey, int index)
        {
            _provider._inner.Rows.Insert(index, 1);

            var row = _provider._inner.Rows[index];
            _provider._rowMap.Add(elementKey, row);
        }

        public void OnRemove(object elementKey, int index)
        {
            _provider._rowMap.Remove(elementKey);
            _provider._inner.Rows.RemoveAt(index);
        }
    }
}

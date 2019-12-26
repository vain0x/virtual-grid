using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Headers;

namespace VirtualGrid.WinFormsDemo.Provider.Headers
{
    public struct RowHeaderRowHeaderDeltaListener
        : IGridHeaderDeltaListener
    {
        private readonly DataGridViewGridProvider _provider;

        public RowHeaderRowHeaderDeltaListener(DataGridViewGridProvider provider)
        {
            _provider = provider;
        }

        public void OnInsert(object elementKey, int index)
        {
            Debug.Assert(elementKey != null);

            _provider._inner.Rows.Insert(index, 1);

            var row = _provider._inner.Rows[index];
            _provider._rowMap.Add(elementKey, row);
            row.Tag = elementKey;
        }

        public void OnRemove(int index)
        {
            var row = _provider._inner.Rows[index];
            var elementKey = row.Tag;

            _provider._rowMap.Remove(elementKey);
            _provider._inner.Rows.RemoveAt(index);
        }
    }
}

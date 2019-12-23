using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Headers;

namespace VirtualGrid.WinFormsDemo.Provider.Headers
{
    public struct ColumnHeaderDeltaListener
        : IGridHeaderDeltaListener
    {
        private readonly DataGridViewGridProvider _provider;

        public ColumnHeaderDeltaListener(DataGridViewGridProvider provider)
        {
            _provider = provider;
        }

        public void OnInsert(object elementKey, int index)
        {
            _provider._inner.Columns.Insert(index, new DataGridViewColumn()
            {
                HeaderText = "",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 100,
            });

            var column = _provider._inner.Columns[index];
            _provider._columnMap.Add(elementKey, column);
        }

        public void OnRemove(object elementKey, int index)
        {
            _provider._columnMap.Remove(elementKey);
            _provider._inner.Columns.RemoveAt(index);
        }
    }
}

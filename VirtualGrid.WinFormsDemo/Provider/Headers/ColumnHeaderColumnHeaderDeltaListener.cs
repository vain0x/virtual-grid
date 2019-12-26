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
    public struct ColumnHeaderColumnHeaderDeltaListener
        : IGridHeaderDeltaListener
    {
        private readonly DataGridViewGridProvider _provider;

        public ColumnHeaderColumnHeaderDeltaListener(DataGridViewGridProvider provider)
        {
            _provider = provider;
        }

        public void OnInsert(int index, object elementKey)
        {
            Debug.Assert(elementKey != null);

            _provider._inner.Columns.Insert(index, new DataGridViewColumn()
            {
                HeaderText = "",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 100,
            });

            var column = _provider._inner.Columns[index];
            _provider._columnMap.Add(elementKey, column);
            column.Tag = elementKey;
        }

        public void OnRemove(int index)
        {
            var column = _provider._inner.Columns[index];
            var elementKey = column.Tag;

            _provider._columnMap.Remove(elementKey);
            _provider._inner.Columns.RemoveAt(index);
        }
    }
}

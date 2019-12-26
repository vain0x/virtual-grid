using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Headers;

namespace VirtualGrid.WinFormsDemo
{
    public struct DataGridViewRowHeaderPart
        : IDataGridViewPart
    {
        private readonly DataGridViewGridProvider _provider;

        public DataGridViewRowHeaderPart(DataGridViewGridProvider provider)
        {
            _provider = provider;
        }

        public DataGridViewCell TryGetCell(GridElementKey elementKey)
        {
            DataGridViewRow row;
            if (!_provider._rowMap.TryGetValue(elementKey.RowElementKey, out row))
                return null;

            return row.HeaderCell;
        }

        public GridElementKey? TryGetKey(GridVector index)
        {
            if (index.Row >= _provider._dataGridView.RowCount)
                return null;

            var row = _provider._dataGridView.Rows[index.Row.Row];
            var rowKey = row.Tag;

            var columnKey = _provider._rowHeaderColumnKey;
            return GridElementKey.Create(rowKey, columnKey);
        }

        public struct RowHeaderDeltaListener
            : IGridHeaderDeltaListener
        {
            private DataGridViewGridProvider _provider;

            private DataGridView _dataGridView
            {
                get
                {
                    return _provider._dataGridView;
                }
            }

            public RowHeaderDeltaListener(DataGridViewGridProvider provider)
            {
                _provider = provider;
            }

            public void OnInsert(int index, object elementKey)
            {
                Debug.Assert(elementKey != null);

                _dataGridView.Rows.Insert(index, 1);

                var row = _dataGridView.Rows[index];
                _provider._rowMap.Add(elementKey, row);
                row.Tag = elementKey;
            }

            public void OnRemove(int index)
            {
                var row = _dataGridView.Rows[index];
                var elementKey = row.Tag;

                _provider._rowMap.Remove(elementKey);
                _provider._dataGridView.Rows.RemoveAt(index);
            }
        }

        public struct ColumnHeaderDeltaListener
            : IGridHeaderDeltaListener
        {
            private DataGridViewGridProvider _provider;

            public ColumnHeaderDeltaListener(DataGridViewGridProvider provider)
            {
                _provider = provider;
            }

            public void OnInsert(int index, object elementKey)
            {
                _provider._rowHeaderColumnKey = elementKey;
            }

            public void OnRemove(int index)
            {
            }
        }
    }
}

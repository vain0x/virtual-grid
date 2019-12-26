using System.Diagnostics;
using System.Windows.Forms;
using VirtualGrid.Headers;

namespace VirtualGrid.WinFormsDemo
{
    public struct DataGridViewColumnHeaderPart
        : IDataGridViewPart
    {
        private readonly DataGridViewGridProvider _provider;

        public DataGridViewColumnHeaderPart(DataGridViewGridProvider provider)
        {
            _provider = provider;
        }

        public DataGridViewCell TryGetCell(GridElementKey elementKey)
        {
            DataGridViewColumn column;
            if (!_provider._columnMap.TryGetValue(elementKey.ColumnElementKey, out column))
                return null;

            return column.HeaderCell;
        }

        public GridElementKey? TryGetKey(GridVector index)
        {
            if (index.Column >= _provider._dataGridView.ColumnCount)
                return null;

            var column = _provider._dataGridView.Columns[index.Column.Column];
            var columnKey = column.Tag;

            var rowKey = _provider._columnHeaderRowKey;
            return GridElementKey.Create(rowKey, columnKey);
        }

        public struct RowHeaderDeltaListener
            : IGridHeaderDeltaListener
        {
            private DataGridViewGridProvider _provider;

            public RowHeaderDeltaListener(DataGridViewGridProvider provider)
            {
                _provider = provider;
            }

            public void OnInsert(int index, object elementKey)
            {
                _provider._columnHeaderRowKey = elementKey;
            }

            public void OnRemove(int index)
            {
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

            private DataGridView _dataGridView
            {
                get
                {
                    return _provider._dataGridView;
                }
            }

            public void OnInsert(int index, object elementKey)
            {
                Debug.Assert(elementKey != null);

                _dataGridView.Columns.Insert(index, new DataGridViewColumn()
                {
                    HeaderText = "",
                    CellTemplate = new DataGridViewTextBoxCell(),
                    Width = 100,
                });

                var column = _dataGridView.Columns[index];
                _provider._columnMap.Add(elementKey, column);
                column.Tag = elementKey;
            }

            public void OnRemove(int index)
            {
                var column = _dataGridView.Columns[index];
                var elementKey = column.Tag;

                _provider._columnMap.Remove(elementKey);
                _dataGridView.Columns.RemoveAt(index);
            }
        }
    }
}

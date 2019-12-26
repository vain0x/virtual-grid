using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public struct DataGridViewBodyPart
        : IDataGridViewPart
    {
        private DataGridViewGridProvider _provider;

        public DataGridViewBodyPart(DataGridViewGridProvider provider)
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

        public DataGridViewCell TryGetCell(GridElementKey elementKey)
        {
            DataGridViewRow row;
            if (!_provider._rowMap.TryGetValue(elementKey.RowElementKey, out row))
                return null;

            DataGridViewColumn column;
            if (!_provider._columnMap.TryGetValue(elementKey.ColumnElementKey, out column))
                return null;

            return row.Cells[column.Index];
        }

        public GridElementKey? TryGetKey(GridVector index)
        {
            if (index.Row >= _dataGridView.RowCount || index.Column >= _dataGridView.ColumnCount)
                return null;

            var rowKey = _dataGridView.Rows[index.Row.Row].Tag;
            var columnKey = _dataGridView.Columns[index.Column.Column].Tag;
            return GridElementKey.Create(rowKey, columnKey);
        }
    }
}

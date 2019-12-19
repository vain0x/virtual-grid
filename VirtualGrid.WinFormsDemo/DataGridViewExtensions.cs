using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public static class DataGridViewExtensions
    {
        private static bool GetColumnHeaderCell(DataGridView self, GridVector index, out DataGridViewCell cell)
        {
            if (index.Column >= self.ColumnCount)
            {
                Debug.WriteLine("Invalid column {0} >= {1}", index.Column, self.ColumnCount);
                cell = null;
                return false;
            }

            cell = self.Columns[index.Column.Column].HeaderCell;
            return true;
        }

        private static bool GetRowHeaderCell(DataGridView self, GridVector index, out DataGridViewCell cell)
        {
            if (index.Row >= self.RowCount)
            {
                Debug.WriteLine("Invalid row {0} >= {1}", index.Row, self.RowCount);
                cell = null;
                return false;
            }

            cell = self.Rows[index.Row.Row].HeaderCell;
            return true;
        }

        private static bool GetBodyCell(DataGridView self, GridVector index, out DataGridViewCell cell)
        {
            if (index.Row >= self.RowCount)
            {
                Debug.WriteLine("Invalid row {0} >= {1}", index.Row, self.RowCount);
                cell = null;
                return false;
            }

            if (index.Column >= self.ColumnCount)
            {
                Debug.WriteLine("Invalid column {0} >= {1}", index.Column, self.ColumnCount);
                cell = null;
                return false;
            }

            cell = self.Rows[index.Row.Row].Cells[index.Column.Column];
            return true;
        }

        public static bool GetCell(this DataGridView self, GridLocation location, out DataGridViewCell cell)
        {
            switch (location.Part)
            {
                case GridPart.ColumnHeader:
                    return GetColumnHeaderCell(self, location.Index, out cell);

                case GridPart.RowHeader:
                    return GetRowHeaderCell(self, location.Index, out cell);

                case GridPart.Body:
                    return GetBodyCell(self, location.Index, out cell);

                default:
                    throw new Exception("Unknown GridPart");
            }
        }
    }
}

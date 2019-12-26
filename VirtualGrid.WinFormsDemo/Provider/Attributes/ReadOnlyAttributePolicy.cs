using System;
using System.Diagnostics;
using System.Windows.Forms;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public struct ReadOnlyAttributePolicy
        : IDataAttributePolicy<bool>
    {
        public bool DefaultValue
        {
            get
            {
                return false;
            }
        }

        public void OnChange(DataGridViewCell cell, bool oldValue, bool newValue)
        {
            // ヘッダーセルの ReadOnly は読み取り専用なので変更しない。
            if (cell.RowIndex < 0 || cell.ColumnIndex < 0)
                return;

            cell.ReadOnly = newValue;
        }
    }
}

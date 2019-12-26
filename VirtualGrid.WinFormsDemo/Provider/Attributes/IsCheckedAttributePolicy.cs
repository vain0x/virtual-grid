using System;
using System.Diagnostics;
using System.Windows.Forms;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public struct IsCheckedAttributePolicy
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
            cell.Value = newValue ? "[x]" : "[ ]";
        }
    }
}

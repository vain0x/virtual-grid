using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public struct IsCheckedAttributePolicy
        : IGridDataAttributePolicy<bool>
    {
        private readonly DataGridViewGridProvider _provider;

        public IsCheckedAttributePolicy(DataGridViewGridProvider provider)
        {
            _provider = provider;
        }

        public bool DefaultValue
        {
            get
            {
                return false;
            }
        }

        public void OnChange(GridElementKey elementKey, GridLocation location, bool oldValue, bool newValue)
        {
            DataGridViewCell cell;
            if (!_provider._inner.GetCell(location, out cell))
                return;

            cell.Value = newValue ? "[x]" : "[ ]";
        }
    }
}

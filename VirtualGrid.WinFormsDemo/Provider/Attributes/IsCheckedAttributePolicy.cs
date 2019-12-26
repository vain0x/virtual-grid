using System;
using System.Diagnostics;
using System.Windows.Forms;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public struct IsCheckedAttributePolicy
        : IDataAttributePolicy<bool>
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

        public void OnChange(SpreadElementKey elementKey, SpreadLocation location, bool oldValue, bool newValue)
        {
            DataGridViewCell cell;
            if (!_provider._inner.GetCell(location, out cell))
                return;

            cell.Value = newValue ? "[x]" : "[ ]";
        }
    }
}

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    /// <summary>
    /// セルの IsChecked 属性
    /// </summary>
    public sealed class IsCheckedAttributeProvider
        : GridAttributeProviderBase<bool>
    {
        public IsCheckedAttributeProvider(DataGridViewGridProvider provider)
            : base(provider, false)
        {
        }

        public override void OnChange(object elementKey, GridLocation location, bool oldValue, bool newValue)
        {
            DataGridViewCell cell;
            if (!_provider._inner.GetCell(location, out cell))
                return;

            cell.Value = newValue ? "[x]" : "[ ]";
        }
    }
}

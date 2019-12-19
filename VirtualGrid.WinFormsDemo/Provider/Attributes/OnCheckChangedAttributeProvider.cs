using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public sealed class OnCheckChangedAttributeProvider
        : GridAttributeProviderBase<Action<bool>>
    {
        public OnCheckChangedAttributeProvider(DataGridViewGridProvider provider)
            : base(provider, null)
        {
        }

        public override void OnChange(object elementKey, GridLocation location, Action<bool> oldValue, Action<bool> newValue)
        {
        }
    }
}

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public sealed class OnTextChangedAttributeProvider
        : GridAttributeProviderBase<Action<string>>
    {
        public OnTextChangedAttributeProvider(DataGridViewGridProvider provider)
            : base(provider, null)
        {
        }

        public override void OnChange(object elementKey, GridLocation location, Action<string> oldValue, Action<string> newValue)
        {
        }
    }
}

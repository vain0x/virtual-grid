using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public sealed class OnClickAttributeProvider
        : GridAttributeProviderBase<Action>
    {
        public OnClickAttributeProvider(DataGridViewGridProvider provider)
            : base(provider, null)
        {
        }

        public override void ApplyDiff()
        {
            MarkAsClean();
        }

        public override void OnChange(object elementKey, GridLocation location, Action oldValue, Action newValue)
        {
        }
    }
}

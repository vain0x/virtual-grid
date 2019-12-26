using System.Diagnostics;
using System.Windows.Forms;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public struct ReadOnlyAttributePolicy
        : IDataAttributePolicy<bool>
    {
        private readonly DataGridViewGridProvider _provider;

        public ReadOnlyAttributePolicy(DataGridViewGridProvider provider)
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
            if (location.Part != SpreadPart.Body)
                return;

            DataGridViewCell cell;
            if (!_provider._inner.GetCell(location, out cell))
                return;

            Debug.WriteLine("ReadOnly {0} {1} value={2}", elementKey, location, newValue);
            cell.ReadOnly = newValue;
        }
    }
}

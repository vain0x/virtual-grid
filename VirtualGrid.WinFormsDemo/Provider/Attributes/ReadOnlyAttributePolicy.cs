using System.Diagnostics;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public struct ReadOnlyAttributePolicy
        : IGridDataAttributePolicy<bool>
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

        public void OnChange(GridElementKey elementKey, GridLocation location, bool oldValue, bool newValue)
        {
            if (location.Part != GridPart.Body)
                return;

            DataGridViewCell cell;
            if (!_provider._inner.GetCell(location, out cell))
                return;

            Debug.WriteLine("ReadOnly {0} {1} value={2}", elementKey, location, newValue);
            cell.ReadOnly = newValue;
        }
    }
}

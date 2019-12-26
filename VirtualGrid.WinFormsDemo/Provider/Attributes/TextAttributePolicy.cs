using System.Diagnostics;
using System.Windows.Forms;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public struct TextAttributePolicy
        : IDataAttributePolicy<string>
    {
        private DataGridViewGridProvider _provider;

        public TextAttributePolicy(DataGridViewGridProvider provider)
        {
            _provider = provider;
        }

        public string DefaultValue
        {
            get
            {
                return "";
            }
        }

        public void OnChange(SpreadElementKey elementKey, SpreadLocation location, string oldValue, string newValue)
        {
            DataGridViewCell cell;
            if (!_provider._inner.GetCell(location, out cell))
                return;

            Debug.WriteLine("Text {0} {1} value={2}", elementKey, location, newValue);
            cell.Value = newValue ?? "";
        }
    }
}

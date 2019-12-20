using System.Diagnostics;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public struct TextAttributePolicy
        : IGridDataAttributePolicy<string>
    {
        private readonly DataGridViewGridProvider _provider;

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

        public void OnChange(object elementKey, GridLocation location, string oldValue, string newValue)
        {
            DataGridViewCell cell;
            if (!_provider._inner.GetCell(location, out cell))
                return;

            Debug.WriteLine("Text {0} {1} value={2}", elementKey, location, newValue);
            cell.Value = newValue ?? "";
        }
    }
}

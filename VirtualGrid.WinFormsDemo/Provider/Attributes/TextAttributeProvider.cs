using System.Diagnostics;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    /// <summary>
    /// セルの Text 属性
    /// </summary>
    public sealed class TextAttributeProvider
        : GridAttributeProviderBase<string>
    {
        public TextAttributeProvider(DataGridViewGridProvider provider)
            : base(provider, "")
        {
        }

        public override void OnChange(object elementKey, GridLocation location, string oldValue, string newValue)
        {
            DataGridViewCell cell;
            if (!_provider._inner.GetCell(location, out cell))
                return;

            Debug.WriteLine("Text {0} {1} value={2}", elementKey, location, newValue);
            cell.Value = newValue ?? "";
        }
    }
}

using System.Diagnostics;
using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    /// <summary>
    /// セルの ReadOnly 属性
    /// </summary>
    public sealed class ReadOnlyAttributeProvider
        : GridAttributeProviderBase<bool>
    {
        public ReadOnlyAttributeProvider(DataGridViewGridProvider provider)
            : base(provider, false)
        {
        }

        public override void OnChange(object elementKey, GridLocation location, bool oldValue, bool newValue)
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

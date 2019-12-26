using System.Diagnostics;
using System.Windows.Forms;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public struct TextAttributePolicy
        : IDataAttributePolicy<string>
    {
        public string DefaultValue
        {
            get
            {
                return "";
            }
        }

        public void OnChange(DataGridViewCell cell, string oldValue, string newValue)
        {
            cell.Value = newValue ?? "";
        }
    }
}

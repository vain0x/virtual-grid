using System.Windows.Forms;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public interface IDataAttributePolicy<T>
    {
        T DefaultValue { get; }

        void OnChange(DataGridViewCell cell, T oldValue, T newValue);
    }
}

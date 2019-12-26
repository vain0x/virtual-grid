using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public interface IDataGridViewPart
    {
        DataGridViewCell TryGetCell(GridElementKey elementKey);

        GridElementKey? TryGetKey(GridVector index);
    }
}

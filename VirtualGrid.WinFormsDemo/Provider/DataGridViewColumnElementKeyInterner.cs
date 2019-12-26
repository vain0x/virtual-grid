using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public struct DataGridViewColumnElementKeyInterner
        : IElementKeyInterner
    {
        private DataGridViewGridProvider _provider;

        public DataGridViewColumnElementKeyInterner(DataGridViewGridProvider provider)
        {
            _provider = provider;
        }

        public int? TryGetIndex(object elementKey)
        {
            DataGridViewColumn column;
            if (!_provider._columnMap.TryGetValue(elementKey, out column))
                return null;

            return column.Index;
        }

        public object TryGetKey(int index)
        {
            if ((uint)index >= _provider._dataGridView.Columns.Count)
                return null;

            return _provider._dataGridView.Columns[index].Tag;
        }
    }
}

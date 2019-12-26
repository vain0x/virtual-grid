using System.Windows.Forms;

namespace VirtualGrid.WinFormsDemo
{
    public struct DataGridViewRowElementKeyInterner
        : IElementKeyInterner
    {
        private DataGridViewGridProvider _provider;

        public DataGridViewRowElementKeyInterner(DataGridViewGridProvider provider)
        {
            _provider = provider;
        }

        public int? TryGetIndex(object elementKey)
        {
            DataGridViewRow row;
            if (!_provider._rowMap.TryGetValue(elementKey, out row))
                return null;

            return row.Index;
        }

        public object TryGetKey(int index)
        {
            if ((uint)index >= _provider._inner.Rows.Count)
                return null;

            return _provider._inner.Rows[index].Tag;
        }
    }
}

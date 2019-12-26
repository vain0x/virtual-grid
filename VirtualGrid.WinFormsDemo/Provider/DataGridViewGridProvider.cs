using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public sealed class DataGridViewGridProvider
    {
        internal readonly DataGridView _dataGridView;

        internal readonly Action<GridElementKey, Action> _dispatch;

        internal object _rowHeaderColumnKey;

        internal object _columnHeaderRowKey;

        internal readonly Dictionary<object, DataGridViewRow> _rowMap =
            new Dictionary<object, DataGridViewRow>();

        internal readonly Dictionary<object, DataGridViewColumn> _columnMap =
            new Dictionary<object, DataGridViewColumn>();

        public DataGridViewRowHeaderPart RowHeader;

        public DataGridViewColumnHeaderPart ColumnHeader;

        public DataGridViewBodyPart Body;

        public DataGridViewGridProvider(DataGridView inner, Action<GridElementKey, Action> dispatch)
        {
            _dataGridView = inner;

            _dispatch = (elementKey, action) =>
            {
                Debug.WriteLine("Dispatch({0}, {1})", elementKey, action);
                dispatch(elementKey, action);
            };

            RowHeader = new DataGridViewRowHeaderPart(this);

            ColumnHeader = new DataGridViewColumnHeaderPart(this);

            Body = new DataGridViewBodyPart(this);
        }
    }
}

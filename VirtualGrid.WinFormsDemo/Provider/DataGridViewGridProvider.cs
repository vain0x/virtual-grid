using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Abstraction;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{

    public sealed class DataGridViewGridProvider
        : IGridProvider
    {
        internal readonly DataGridView _inner;

        internal readonly Action<GridElementKey, Action> _dispatch;

        internal readonly Dictionary<object, DataGridViewRow> _rowMap =
            new Dictionary<object, DataGridViewRow>();

        internal readonly Dictionary<object, DataGridViewColumn> _columnMap =
            new Dictionary<object, DataGridViewColumn>();

        public DataGridViewGridProvider(DataGridView inner, Action<GridElementKey, Action> dispatch)
        {
            _inner = inner;

            _dispatch = (elementKey, action) =>
            {
                Debug.WriteLine("Dispatch({0}, {1})", elementKey, action);
                dispatch(elementKey, action);
            };
        }

        public bool TryGetLocation(SpreadElementKey elementKey, out SpreadLocation location)
        {
            var rowIndexOpt = default(RowIndex?);
            var columnIndexOpt = default(ColumnIndex?);

            DataGridViewRow row;
            if (_rowMap.TryGetValue(elementKey.ElementKey.RowElementKey, out row))
            {
                rowIndexOpt = RowIndex.From(row.Index);
            }

            DataGridViewColumn column;
            if (_columnMap.TryGetValue(elementKey.ElementKey.ColumnElementKey, out column))
            {
                columnIndexOpt = ColumnIndex.From(column.Index);
            }

            switch (elementKey.Part)
            {
                case SpreadPart.RowHeader:
                    {
                        if (rowIndexOpt.HasValue)
                        {
                            location = SpreadLocation.NewRowHeader(rowIndexOpt.Value.AsVector);
                            return true;
                        }
                        break;
                    }

                case SpreadPart.ColumnHeader:
                    {
                        if (columnIndexOpt.HasValue)
                        {
                            location = SpreadLocation.NewColumnHeader(columnIndexOpt.Value.AsVector);
                            return true;
                        }
                        break;
                    }

                case SpreadPart.Body:
                    {
                        if (rowIndexOpt.HasValue && columnIndexOpt.HasValue)
                        {
                            location = SpreadLocation.NewBody(GridVector.Create(rowIndexOpt.Value, columnIndexOpt.Value));
                            return true;
                        }
                        break;
                    }

                default:
                    throw new Exception("Unknown SpreadPart");
            }

            location = default(SpreadLocation);
            return false;
        }
    }
}

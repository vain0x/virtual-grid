using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;
using RowHeaderLayout = VirtualGrid.Layouts.GridLayout<
    VirtualGrid.WinFormsDemo.DataGridViewGridProvider,
    VirtualGrid.WinFormsDemo.DataGridViewRowHeaderPart.RowHeaderDeltaListener,
    VirtualGrid.WinFormsDemo.DataGridViewRowHeaderPart.ColumnHeaderDeltaListener
>;
using ColumnHeaderLayout = VirtualGrid.Layouts.GridLayout<
    VirtualGrid.WinFormsDemo.DataGridViewGridProvider,
    VirtualGrid.WinFormsDemo.DataGridViewColumnHeaderPart.RowHeaderDeltaListener,
    VirtualGrid.WinFormsDemo.DataGridViewColumnHeaderPart.ColumnHeaderDeltaListener
>;

namespace VirtualGrid.WinFormsDemo.Provider.RowsComponents
{
    public sealed class DataGridViewSpreadLayout
    {
        public readonly RowHeaderLayout RowHeader;

        public readonly ColumnHeaderLayout ColumnHeader;

        public DataGridViewSpreadLayout(DataGridViewGridProvider provider)
        {
            RowHeader = new RowHeaderLayout(
                new GridHeader<DataGridViewRowHeaderPart.RowHeaderDeltaListener>(
                    "KEY_SPREAD_ROW_HEADER_ROW_HEADER",
                    new DataGridViewRowHeaderPart.RowHeaderDeltaListener(provider)
                ),
                new GridHeader<DataGridViewRowHeaderPart.ColumnHeaderDeltaListener>(
                    "KEY_SPREAD_ROW_HEADER_COLUMN_HEADER",
                    new DataGridViewRowHeaderPart.ColumnHeaderDeltaListener(provider)
                ));

            ColumnHeader = new ColumnHeaderLayout(
                new GridHeader<DataGridViewColumnHeaderPart.RowHeaderDeltaListener>(
                    "KEY_SPREAD_COLUMN_HEADER_ROW_HEADER",
                    new DataGridViewColumnHeaderPart.RowHeaderDeltaListener(provider)
                ),
                new GridHeader<DataGridViewColumnHeaderPart.ColumnHeaderDeltaListener>(
                    "KEY_SPREAD_COLUMN_HEADER_COLUMN_HEADER",
                    new DataGridViewColumnHeaderPart.ColumnHeaderDeltaListener(provider)
                ));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;

namespace VirtualGrid.RowsComponents
{
    public sealed class GridRowElement<TData>
    {
        private GridRow _row;

        public TData Data;

        public GridRowElement(GridRow row, TData data)
        {
            _row = row;
            Data = data;
        }

        public IGridCellAdder<TData> At(GridColumn column)
        {
            return new AnonymousGridCellAdder<TData>(() =>
            {
                return new GridCellBuilder<TData>(GridElementKey.Create(_row.ElementKey, column.ElementKey), Data);
            });
        }
    }
}

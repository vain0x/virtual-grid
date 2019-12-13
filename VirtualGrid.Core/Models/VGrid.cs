using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Layouts;

namespace VirtualGrid.Models
{
    public struct VGrid
        : IEquatable<VGrid>
    {
        public readonly VCell[] Cells;

        public readonly IGridLayout ColumnHeader;

        public readonly IGridLayout RowHeader;

        public readonly IGridLayout Body;

        private VGrid(VCell[] cells, IGridLayout columnHeader, IGridLayout rowHeader, IGridLayout body)
        {
            Cells = cells;
            ColumnHeader = columnHeader;
            RowHeader = rowHeader;
            Body = body;
        }

        public static readonly VGrid Empty =
            new VGrid(
                Array.Empty<VCell>(),
                GridLayout.Empty("?_EMPTY_COLUMN_HEADER"),
                GridLayout.Empty("?_EMPTY_ROW_HEADER"),
                GridLayout.Empty("?_EMPTY_BODY")
            );

        public VGrid WithCells(VCell[] cells)
        {
            return new VGrid(cells, ColumnHeader, RowHeader, Body);
        }

        public VGrid WithColumnHeader(IGridLayout columnHeader)
        {
            return new VGrid(Cells, columnHeader, RowHeader, Body);
        }

        public VGrid WithRowHeader(IGridLayout rowHeader)
        {
            return new VGrid(Cells, ColumnHeader, rowHeader, Body);
        }

        public VGrid WithBody(IGridLayoutProvider body)
        {
            return new VGrid(Cells, ColumnHeader, RowHeader, body.ToGridLayout());
        }

        public VCell FindCell(object elementKey)
        {
            return Cells.FirstOrDefault(cell => EqualityComparer<object>.Default.Equals(cell.ElementKey, elementKey));
        }

        public override bool Equals(object obj)
        {
            return obj is VGrid grid && Equals(grid);
        }

        public bool Equals(VGrid other)
        {
            return EqualityComparer<VCell[]>.Default.Equals(Cells, other.Cells) &&
                   EqualityComparer<IGridLayout>.Default.Equals(ColumnHeader, other.ColumnHeader) &&
                   EqualityComparer<IGridLayout>.Default.Equals(RowHeader, other.RowHeader) &&
                   EqualityComparer<IGridLayout>.Default.Equals(Body, other.Body);
        }

        public override int GetHashCode()
        {
            var hashCode = 779191759;
            hashCode = hashCode * -1521134295 + EqualityComparer<VCell[]>.Default.GetHashCode(Cells);
            hashCode = hashCode * -1521134295 + EqualityComparer<IGridLayout>.Default.GetHashCode(ColumnHeader);
            hashCode = hashCode * -1521134295 + EqualityComparer<IGridLayout>.Default.GetHashCode(RowHeader);
            hashCode = hashCode * -1521134295 + EqualityComparer<IGridLayout>.Default.GetHashCode(Body);
            return hashCode;
        }

        public static bool operator ==(VGrid left, VGrid right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VGrid left, VGrid right)
        {
            return !(left == right);
        }
    }
}

using System;

namespace VirtualGrid.Rendering
{
    public interface IGridCellAdder
    {
        IGridCellBuilder AddCell();
    }

    internal sealed class AnonymousGridCellAdder
        : IGridCellAdder
    {
        private Func<IGridCellBuilder> _newCell;

        public AnonymousGridCellAdder(Func<IGridCellBuilder> newCell)
        {
            _newCell = newCell;
        }

        public IGridCellBuilder AddCell()
        {
            return _newCell();
        }
    }
}

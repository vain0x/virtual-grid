using System;

namespace VirtualGrid.Rendering
{
    public interface IGridCellAdder<TProvider>
    {
        IGridCellBuilder<TProvider> AddCell();
    }

    internal sealed class AnonymousGridCellAdder<TProvider>
        : IGridCellAdder<TProvider>
    {
        private Func<IGridCellBuilder<TProvider>> _newCell;

        public AnonymousGridCellAdder(Func<IGridCellBuilder<TProvider>> newCell)
        {
            _newCell = newCell;
        }

        public IGridCellBuilder<TProvider> AddCell()
        {
            return _newCell();
        }
    }
}

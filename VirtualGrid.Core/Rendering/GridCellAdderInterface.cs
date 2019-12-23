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
        Func<IGridCellBuilder<TProvider>> _addCell;

        public AnonymousGridCellAdder(Func<IGridCellBuilder<TProvider>> addCell)
        {
            _addCell = addCell;
        }

        public IGridCellBuilder<TProvider> AddCell()
        {
            return _addCell();
        }
    }
}

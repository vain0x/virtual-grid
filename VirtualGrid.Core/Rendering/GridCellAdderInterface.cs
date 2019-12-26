using System;

namespace VirtualGrid.Rendering
{
    public interface IGridCellAdder<TProvider>
    {
        GridCellBuilder<TProvider> AddCell();

        GridElementBuilder<TProvider> AddElement();
    }

    public sealed class AnonymousGridCellAdder<TProvider>
        : IGridCellAdder<TProvider>
    {
        Func<GridCellBuilder<TProvider>> _addCell;

        public AnonymousGridCellAdder(Func<GridCellBuilder<TProvider>> addCell)
        {
            _addCell = addCell;
        }

        public GridCellBuilder<TProvider> AddCell()
        {
            return _addCell();
        }

        public GridElementBuilder<TProvider> AddElement()
        {
            throw new NotImplementedException();
        }
    }

    public struct GridElementBuilder<TProvider>
    {
        public GridElementBuilder<TProvider> OnCreateOrUpdate(Action action)
        {
            return this;
        }
    }
}

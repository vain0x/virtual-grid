using System;

namespace VirtualGrid.Rendering
{
    public interface IGridCellAdder<TProvider>
    {
        IGridCellBuilder<TProvider> AddCell();
    }
}

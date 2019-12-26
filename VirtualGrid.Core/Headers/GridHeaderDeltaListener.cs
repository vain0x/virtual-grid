using System.Collections.Generic;

namespace VirtualGrid.Headers
{
    public interface IGridHeaderDeltaListener
    {
        // FIXME: 複数個の一括挿入に対応
        void OnInsert(int index, object elementKey);

        void OnRemove(int index);
    }
}

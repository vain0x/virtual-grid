namespace VirtualGrid.Headers
{
    public interface IGridHeaderDeltaListener
    {
        // FIXME: 複数個の一括挿入に対応
        void OnInsert(object elementKey, int index);

        void OnRemove(int index);
    }
}

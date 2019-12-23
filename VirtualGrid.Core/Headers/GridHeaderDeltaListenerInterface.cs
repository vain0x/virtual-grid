namespace VirtualGrid.Headers
{
    public interface IGridHeaderDeltaListener
    { 
        void OnInsert(object elementKey, int index);

        void OnRemove(object elementKey, int index);
    }
}

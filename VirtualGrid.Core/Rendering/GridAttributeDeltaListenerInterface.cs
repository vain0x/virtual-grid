namespace VirtualGrid.Rendering
{
    public interface IGridAttributeDeltaListener<T>
    {
        void OnAdd(GridElementKey elementKey, T newValue);

        void OnChange(GridElementKey elementKey, T oldValue, T newValue);

        void OnRemove(GridElementKey elementKey, T oldValue);
    }
}

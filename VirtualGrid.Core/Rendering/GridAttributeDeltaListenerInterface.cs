namespace VirtualGrid.Rendering
{
    public interface IGridAttributeDeltaListener<T>
    {
        void OnAdd(object elementKey, T newValue);

        void OnChange(object elementKey, T oldValue, T newValue);

        void OnRemove(object elementKey, T oldValue);
    }
}

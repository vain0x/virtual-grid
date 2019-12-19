namespace VirtualGrid.Rendering
{
    public interface IGridAttributeProvider<T>
    {
        T DefaultValue { get; }

        void OnAdd(object elementKey, GridLocation location, T newValue);

        void OnChange(object elementKey, GridLocation location, T oldValue, T newValue);

        void OnRemove(object elementKey, GridLocation location, T oldValue);

        void MarkAsClean();
    }
}

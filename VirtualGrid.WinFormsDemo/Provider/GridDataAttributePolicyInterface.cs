namespace VirtualGrid.WinFormsDemo
{
    public interface IGridDataAttributePolicy<T>
    {
        T DefaultValue { get; }

        void OnChange(object elementKey, GridLocation location, T oldValue, T newValue);
    }
}

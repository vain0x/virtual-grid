namespace VirtualGrid.WinFormsDemo
{
    public interface IGridDataAttributePolicy<T>
    {
        T DefaultValue { get; }

        void OnChange(GridElementKey elementKey, GridLocation location, T oldValue, T newValue);
    }
}

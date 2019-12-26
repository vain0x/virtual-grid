using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public interface IDataAttributePolicy<T>
    {
        T DefaultValue { get; }

        void OnChange(SpreadElementKey elementKey, SpreadLocation location, T oldValue, T newValue);
    }
}

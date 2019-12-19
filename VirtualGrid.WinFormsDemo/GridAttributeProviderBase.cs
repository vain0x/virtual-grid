using VirtualGrid.Rendering;

namespace VirtualGrid.WinFormsDemo
{
    /// <summary>
    /// セルの属性の実装を共通化するためのもの
    /// </summary>
    public abstract class GridAttributeProviderBase<T>
        : IGridAttributeProvider<T>
    {
        protected readonly GridAttributeData<T> _data;

        protected readonly DataGridViewGridProvider _provider;

        protected GridAttributeProviderBase(DataGridViewGridProvider provider, T defaultValue)
        {
            _data = new GridAttributeData<T>(defaultValue);
            _provider = provider;
        }

        public T DefaultValue
        {
            get
            {
                return _data.DefaultValue;
            }
        }

        public void SetValue(object elementKey, T value)
        {
            _data.SetValue(elementKey, value);
        }

        public virtual void OnAdd(object elementKey, GridLocation location, T newValue)
        {
            OnChange(elementKey, location, DefaultValue, newValue);
        }

        public virtual void OnChange(object elementKey, GridLocation location, T oldValue, T newValue)
        {
        }

        public virtual void OnRemove(object elementKey, GridLocation location, T oldValue)
        {
        }

        public virtual void ApplyDiff()
        {
            new GridAttributeDataDiffer<T>(this, _data, _provider).ApplyDiff();
        }

        public virtual void MarkAsClean()
        {
            _data.MarkAsClean();
        }
    }
}

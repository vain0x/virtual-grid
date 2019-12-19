using System.Diagnostics;
using VirtualGrid.Rendering;

namespace VirtualGrid.WinFormsDemo
{
    /// <summary>
    /// セルの属性の実装を共通化するためのもの
    /// </summary>
    public abstract class GridAttributeProviderBase<T>
        : IGridAttributeDeltaListener<T>
    {
        protected readonly GridAttributeData<T> _data;

        protected readonly DataGridViewGridProvider _provider;

        protected GridAttributeProviderBase(DataGridViewGridProvider provider, T defaultValue)
        {
            _data = new GridAttributeData<T>(defaultValue);
            _provider = provider;
        }

        public bool IsAttached(object elementKey)
        {
            return _data.IsAttached(elementKey);
        }

        public T GetValue(object elementKey)
        {
            return _data.GetOldValue(elementKey);
        }

        public void SetValue(object elementKey, T value)
        {
            _data.SetValue(elementKey, value);
        }

        public virtual void OnAdd(object elementKey, GridLocation location, T newValue)
        {
            OnChange(elementKey, location, _data.DefaultValue, newValue);
        }

        public abstract void OnChange(object elementKey, GridLocation location, T oldValue, T newValue);

        public virtual void OnRemove(object elementKey, GridLocation location, T oldValue)
        {
        }

        public virtual void ApplyDiff()
        {
            new GridAttributeDataDiffer<T>(_data, this).ApplyDiff();
        }

        public virtual void MarkAsClean()
        {
            _data.MarkAsClean();
        }

        private bool TryGetLocation(object elementKey, out GridLocation location)
        {
            if (!_provider._locationMap.TryGetValue(elementKey, out location))
            {
                Debug.WriteLine("Cell location unknown ({0})", elementKey);
                return false;
            }
            return true;
        }

        void IGridAttributeDeltaListener<T>.OnAdd(object elementKey, T newValue)
        {
            GridLocation location;
            if (TryGetLocation(elementKey, out location))
            {
                OnAdd(elementKey, location, newValue);
            }
        }

        void IGridAttributeDeltaListener<T>.OnChange(object elementKey, T oldValue, T newValue)
        {
            GridLocation location;
            if (TryGetLocation(elementKey, out location))
            {
                OnChange(elementKey, location, oldValue, newValue);
            }
        }

        void IGridAttributeDeltaListener<T>.OnRemove(object elementKey, T oldValue)
        {
            GridLocation location;
            if (TryGetLocation(elementKey, out location))
            {
                OnRemove(elementKey, location, oldValue);
            }
        }
    }
}

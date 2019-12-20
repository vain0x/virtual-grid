using System.Diagnostics;
using VirtualGrid.Rendering;

namespace VirtualGrid.WinFormsDemo
{
    /// <summary>
    /// データ属性の実装を共通化するためのもの
    /// </summary>
    public sealed class GridDataAttributeProvider<T, TPolicy>
        where TPolicy : struct, IGridDataAttributePolicy<T>
    {
        private readonly GridAttributeData<T> _data;

        private readonly TPolicy _policy;

        private readonly DataGridViewGridProvider _provider;

        public GridDataAttributeProvider(TPolicy policy, DataGridViewGridProvider provider)
        {
            _data = new GridAttributeData<T>(policy.DefaultValue);
            _policy = policy;
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

        public void ApplyDiff()
        {
            GridAttributeDataDiffer.Create(_data, new DeltaListener(this)).ApplyDiff();
            _data.MarkAsClean();
        }

        public void MarkAsClean()
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

        public struct DeltaListener
            : IGridAttributeDeltaListener<T>
        {
            private readonly GridDataAttributeProvider<T, TPolicy> _parent;

            public DeltaListener(GridDataAttributeProvider<T, TPolicy> parent)
            {
                _parent = parent;
            }

            public void OnAdd(object elementKey, T newValue)
            {
                GridLocation location;
                if (_parent.TryGetLocation(elementKey, out location))
                {
                    _parent._policy.OnChange(elementKey, location, _parent._data.DefaultValue, newValue);
                }
            }

            public void OnChange(object elementKey, T oldValue, T newValue)
            {
                GridLocation location;
                if (_parent.TryGetLocation(elementKey, out location))
                {
                    _parent._policy.OnChange(elementKey, location, oldValue, newValue);
                }
            }

            public void OnRemove(object elementKey, T oldValue)
            {
            }
        }
    }

    public static class GridDataAttributeProvider
    {
        public static GridDataAttributeProvider<T, TPolicy> Create<T, TPolicy>(T _default, TPolicy policy, DataGridViewGridProvider provider)
            where TPolicy : struct, IGridDataAttributePolicy<T>
        {
            return new GridDataAttributeProvider<T, TPolicy>(policy, provider);
        }
    }
}

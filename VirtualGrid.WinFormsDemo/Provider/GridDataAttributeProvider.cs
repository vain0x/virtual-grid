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

        public bool IsAttached(GridElementKey elementKey)
        {
            return _data.IsAttached(elementKey);
        }

        public T GetValue(GridElementKey elementKey)
        {
            return _data.GetOldValue(elementKey);
        }

        public void SetValue(GridElementKey elementKey, T value)
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

        public struct DeltaListener
            : IGridAttributeDeltaListener<T>
        {
            private readonly GridDataAttributeProvider<T, TPolicy> _parent;

            public DeltaListener(GridDataAttributeProvider<T, TPolicy> parent)
            {
                _parent = parent;
            }

            public void OnAdd(GridElementKey elementKey, T newValue)
            {
                GridLocation location;
                if (_parent._provider.TryGetLocation(elementKey, out location))
                {
                    _parent._policy.OnChange(elementKey, location, _parent._data.DefaultValue, newValue);
                }
            }

            public void OnChange(GridElementKey elementKey, T oldValue, T newValue)
            {
                GridLocation location;
                if (_parent._provider.TryGetLocation(elementKey, out location))
                {
                    _parent._policy.OnChange(elementKey, location, oldValue, newValue);
                }
            }

            public void OnRemove(GridElementKey elementKey, T oldValue)
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

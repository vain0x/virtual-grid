using System.Diagnostics;
using VirtualGrid.Rendering;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    /// <summary>
    /// データ属性の実装を共通化するためのもの
    /// </summary>
    public sealed class GridDataAttributeProvider<T, TPolicy>
        where TPolicy : struct, IDataAttributePolicy<T>
    {
        private SpreadPart _part;

        private GridAttributeData<T> _data;

        private TPolicy _policy;

        private DataGridViewGridProvider _provider;

        public GridDataAttributeProvider(SpreadPart part, TPolicy policy, DataGridViewGridProvider provider)
        {
            _part = part;
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

        public void Patch()
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
                var spreadElementKey = SpreadElementKey.Create(_parent._part, elementKey);

                SpreadLocation location;
                if (_parent._provider.TryGetLocation(spreadElementKey, out location))
                {
                    _parent._policy.OnChange(spreadElementKey, location, _parent._data.DefaultValue, newValue);
                }
            }

            public void OnChange(GridElementKey elementKey, T oldValue, T newValue)
            {
                var spreadElementKey = SpreadElementKey.Create(_parent._part, elementKey);

                SpreadLocation location;
                if (_parent._provider.TryGetLocation(spreadElementKey, out location))
                {
                    _parent._policy.OnChange(spreadElementKey, location, oldValue, newValue);
                }
            }

            public void OnRemove(GridElementKey elementKey, T oldValue)
            {
            }
        }
    }

    public static class GridDataAttributeProvider
    {
        public static GridDataAttributeProvider<T, TPolicy> Create<T, TPolicy>(SpreadPart part, T _default, TPolicy policy, DataGridViewGridProvider provider)
            where TPolicy : struct, IDataAttributePolicy<T>
        {
            return new GridDataAttributeProvider<T, TPolicy>(part, policy, provider);
        }
    }
}

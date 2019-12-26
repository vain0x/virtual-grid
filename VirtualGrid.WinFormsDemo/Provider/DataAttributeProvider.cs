using System.Diagnostics;
using VirtualGrid.Rendering;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    /// <summary>
    /// データ属性の実装を共通化するためのもの
    /// </summary>
    public sealed class GridDataAttributeProvider<T, TPolicy>
        where TPolicy : IDataAttributePolicy<T>
    {
        private IDataGridViewPart _part;

        private TPolicy _policy;

        private GridAttributeData<T> _data;

        public GridDataAttributeProvider(IDataGridViewPart part, TPolicy policy)
        {
            _part = part;
            _policy = policy;

            _data = new GridAttributeData<T>(policy.DefaultValue);
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
                var cell = _parent._part.TryGetCell(elementKey);
                if (cell == null)
                    return;

                _parent._policy.OnChange(cell, _parent._data.DefaultValue, newValue);
            }

            public void OnChange(GridElementKey elementKey, T oldValue, T newValue)
            {
                var cell = _parent._part.TryGetCell(elementKey);
                if (cell == null)
                    return;
                
                _parent._policy.OnChange(cell, oldValue, newValue);
            }

            public void OnRemove(GridElementKey elementKey, T oldValue)
            {
            }
        }
    }

    public static class GridDataAttributeProvider
    {
        public static GridDataAttributeProvider<T, TPolicy> Create<T, TPolicy>(IDataGridViewPart part, T _default, TPolicy policy)
            where TPolicy : IDataAttributePolicy<T>
        {
            return new GridDataAttributeProvider<T, TPolicy>(part, policy);
        }
    }
}

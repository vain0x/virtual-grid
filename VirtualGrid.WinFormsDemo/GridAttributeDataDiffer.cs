using System;
using System.Diagnostics;
using VirtualGrid.Rendering;

namespace VirtualGrid.WinFormsDemo
{
    public sealed class GridAttributeDataDiffer<T>
    {
        private IGridAttributeProvider<T> _attribute;

        private GridAttributeData<T> _data;

        private DataGridViewGridProvider _provider;

        public GridAttributeDataDiffer(IGridAttributeProvider<T> attribute, GridAttributeData<T> data, DataGridViewGridProvider provider)
        {
            _attribute = attribute;
            _data = data;
            _provider = provider;
        }

        void ApplyDiffOnKey(object elementKey)
        {
            GridAttributeDeltaKind kind;
            T oldValue;
            T newValue;

            if (!_data.TryGetDelta(elementKey, out kind, out oldValue, out newValue))
                return;

            GridLocation location;
            if (!_provider._locationMap.TryGetValue(elementKey, out location))
            {
                Debug.WriteLine("Cell location unknown ({0})", elementKey);
                return;
            }

            switch (kind)
            {
                case GridAttributeDeltaKind.Add:
                    _attribute.OnAdd(elementKey, location, newValue);
                    return;

                case GridAttributeDeltaKind.Change:
                    _attribute.OnChange(elementKey, location, oldValue, newValue);
                    return;

                case GridAttributeDeltaKind.Remove:
                    _attribute.OnRemove(elementKey, location, oldValue);
                    return;

                default:
                    throw new InvalidOperationException("Unknown GridAttributeDeltaKind");
            }
        }

        public void ApplyDiff()
        {
            foreach (var elementKey in _data.OldKeys)
            {
                ApplyDiffOnKey(elementKey);
            }

            foreach (var elementKey in _data.NewKeys)
            {
                ApplyDiffOnKey(elementKey);
            }

            _attribute.MarkAsClean();
        }
    }
}

using System;

namespace VirtualGrid.Rendering
{
    public struct GridAttributeDataDiffer<T>
    {
        private readonly GridAttributeData<T> _data;

        private readonly IGridAttributeDeltaListener<T> _listener;

        public GridAttributeDataDiffer(GridAttributeData<T> data, IGridAttributeDeltaListener<T> listener)
        {
            _data = data;
            _listener = listener;
        }

        private void ApplyDiffOnKey(object elementKey)
        {
            GridAttributeDeltaKind kind;
            T oldValue;
            T newValue;

            if (!_data.TryGetDelta(elementKey, out kind, out oldValue, out newValue))
                return;

            switch (kind)
            {
                case GridAttributeDeltaKind.Add:
                    _listener.OnAdd(elementKey, newValue);
                    return;

                case GridAttributeDeltaKind.Change:
                    _listener.OnChange(elementKey, oldValue, newValue);
                    return;

                case GridAttributeDeltaKind.Remove:
                    _listener.OnRemove(elementKey, oldValue);
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
        }
    }
}

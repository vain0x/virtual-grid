using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Rendering
{
    /// <summary>
    /// レンダリング時のデータを管理するもの。
    /// </summary>
    public sealed class GridRenderContext<TProvider>
    {
        // row key, column key, element key
        public readonly List<Tuple<GridPart, object, object, object>> _cells =
            new List<Tuple<GridPart, object, object, object>>();

        internal readonly DefaultDictionary<Tuple<object, string>, object> Attributes =
            new DefaultDictionary<Tuple<object, string>, object>(_ => null);

        public readonly TProvider Provider;

        public GridRenderContext(TProvider provider)
        {
            Provider = provider;
        }

        public void AddCell(GridPart part, object rowKey, object columnKey, object elementKey)
        {
            _cells.Add(Tuple.Create(part, rowKey, columnKey, elementKey));
        }

        public void SetElementAttribute(object elementKey, string attribute, object value)
        {
            Attributes[Tuple.Create(elementKey, attribute)] = value;
        }

        public T GetElementAttribute<T>(object elementKey, string attribute, T defaultValue)
        {
            var value = Attributes[Tuple.Create(elementKey, attribute)];
            return value != null && value is T
                ? (T)value
                : defaultValue;
        }

        public GridAttributeBinding[] GetAttributeBindings()
        {
            return Attributes
                .Select(pair => new GridAttributeBinding(pair.Key.Item1, pair.Key.Item2, pair.Value))
                .OrderBy(binding => EqualityComparer<object>.Default.GetHashCode(binding.ElementKey))
                .ThenBy(binding => binding.Attribute)
                .ToArray();
        }

        public void Clear()
        {
            _cells.Clear();
            Attributes.Clear();
        }
    }
}

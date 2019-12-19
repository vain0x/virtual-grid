using System;

namespace VirtualGrid.Rendering
{
    /// <summary>
    /// 属性への値の割り当てを表す。
    /// </summary>
    public struct GridAttributeBinding
    {
        public readonly object ElementKey;

        public readonly string Attribute;

        public readonly object Value;

        public object GetKey()
        {
            return Tuple.Create(ElementKey, Attribute);
        }

        public GridAttributeBinding(object elementKey, string attribute, object value)
        {
            ElementKey = elementKey;
            Attribute = attribute;
            Value = value;
        }
    }
}

using System;
using System.Diagnostics;

namespace VirtualGrid.Rendering
{
    [DebuggerDisplay("{AsDebug}")]
    public struct GridAttributeDelta
    {
        public readonly GridAttributeDeltaKind Kind;

        public readonly object ElementKey;

        public readonly string Attribute;

        public readonly object Value;

        public string AsDebug
        {
            get
            {
                switch (Kind)
                {
                    case GridAttributeDeltaKind.Add:
                        return string.Format("Cell({0}).{1} Add({2})", ElementKey, Attribute, Value);

                    case GridAttributeDeltaKind.Remove:
                        return string.Format("Cell({0}).{1} Remove", ElementKey, Attribute, Value);

                    case GridAttributeDeltaKind.Change:
                        return string.Format("Cell({0}).{1} Change({2})", ElementKey, Attribute, Value);

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public static GridAttributeDelta NewAdd(GridAttributeBinding binding)
        {
            return new GridAttributeDelta(GridAttributeDeltaKind.Add, binding.ElementKey, binding.Attribute, binding.Value);
        }

        public static GridAttributeDelta NewRemove(GridAttributeBinding binding)
        {
            return new GridAttributeDelta(GridAttributeDeltaKind.Remove, binding.ElementKey, binding.Attribute, binding.Value);
        }

        public static GridAttributeDelta NewChange(GridAttributeBinding binding)
        {
            return new GridAttributeDelta(GridAttributeDeltaKind.Change, binding.ElementKey, binding.Attribute, binding.Value);
        }

        public GridAttributeDelta(GridAttributeDeltaKind kind, object elementKey, string attribute, object value)
        {
            Kind = kind;
            ElementKey = elementKey;
            Attribute = attribute;
            Value = value;
        }
    }
}

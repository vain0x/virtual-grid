using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    [DebuggerDisplay("[Length = {Count}]")]
    public sealed class ArrayTuple<T>
        : IEquatable<ArrayTuple<T>>
    {
        readonly T[] _inner;

        public ArrayTuple(T[] inner)
        {
            _inner = inner;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ArrayTuple<T>);
        }

        public bool Equals(ArrayTuple<T> other)
        {
            return other != null && (_inner == other._inner || _inner.SequenceEqual(other._inner));
        }

        public override int GetHashCode()
        {
            var hashCode = -678005361;
            foreach (var item in _inner)
            {
                hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(item);
            }
            return hashCode;
        }

        public static bool operator ==(ArrayTuple<T> left, ArrayTuple<T> right)
        {
            return EqualityComparer<ArrayTuple<T>>.Default.Equals(left, right);
        }

        public static bool operator !=(ArrayTuple<T> left, ArrayTuple<T> right)
        {
            return !(left == right);
        }
    }

    public static class ArrayTuple
    {
        public static ArrayTuple<T> Empty<T>()
        {
            return new ArrayTuple<T>(Array.Empty<T>());
        }

        public static ArrayTuple<T> From<T>(T[] array)
        {
            return new ArrayTuple<T>(array);
        }

        public static ArrayTuple<T> From<T>(IEnumerable<T> source)
        {
            return From(source.ToArray());
        }
    }
}

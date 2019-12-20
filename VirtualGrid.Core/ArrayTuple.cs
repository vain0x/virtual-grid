using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    /// <summary>
    /// 配列に構造的同値性を入れたもの。(各要素の値が等しい ArrayTuple 同士は等しいとみなす。)
    /// </summary>
    [DebuggerDisplay("[Length = {Count}]")]
    public sealed class ArrayTuple<T>
        : IEquatable<ArrayTuple<T>>
    {
        private readonly T[] _inner;

        private int _hashCode;

        public ArrayTuple(T[] inner)
        {
            _inner = inner;
            _hashCode = 0;
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
            if (_hashCode == 0)
            {
                var hashCode = -678005361;
                foreach (var item in _inner)
                {
                    hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(item);
                }

                _hashCode = hashCode;
            }
            return _hashCode;
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
            if (array == null)
                throw new ArgumentNullException("array");

            return new ArrayTuple<T>(array);
        }

        public static ArrayTuple<T> From<T>(IEnumerable<T> source)
        {
            var array = source as T[];
            if (array != null)
                return From(array);

            return From(source.ToArray());
        }
    }
}

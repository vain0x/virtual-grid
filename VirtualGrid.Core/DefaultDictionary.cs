using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    /// <summary>
    /// 既定値を持つ辞書 (連想配列)
    /// </summary>
    [DebuggerDisplay("[Count = {Count}]")]
    internal sealed class DefaultDictionary<TKey, TValue>
        : IReadOnlyDictionary<TKey, TValue>
        , IDictionary<TKey, TValue>
    {
        readonly Dictionary<TKey, TValue> _inner = new Dictionary<TKey, TValue>();

        /// <summary>
        /// 既定値を作成する関数
        /// </summary>
        readonly Func<TKey, TValue> _defaultFactory;

        public DefaultDictionary(Func<TKey, TValue> defaultFactory)
        {
            _defaultFactory = defaultFactory;
        }

        public int Count
        {
            get
            {
                return _inner.Count;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return ((IDictionary<TKey, TValue>)_inner).Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return ((IDictionary<TKey, TValue>)_inner).Values;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IDictionary<TKey, TValue>)_inner).IsReadOnly;
            }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get
            {
                return ((IReadOnlyDictionary<TKey, TValue>)_inner).Keys;
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get
            {
                return ((IReadOnlyDictionary<TKey, TValue>)_inner).Values;
            }
        }

        /// <summary>
        /// キーに対応する要素を取得または設定する。
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!_inner.TryGetValue(key, out value))
                {
                    value = _defaultFactory(key);
                }
                return value;
            }
            set
            {
                if (_inner.ContainsKey(key))
                {
                    _inner[key] = value;
                }
                else
                {
                    _inner.Add(key, value);
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _inner.ContainsKey(key);
        }

        public TValue Touch(TKey key)
        {
            TValue value;
            if (!_inner.TryGetValue(key, out value))
            {
                value = _defaultFactory(key);
                _inner.Add(key, value);
            }
            return value;
        }

        public void Add(TKey key, TValue value)
        {
            _inner.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return _inner.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _inner.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)_inner).Add(item);
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_inner).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_inner).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_inner).Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)_inner).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)_inner).GetEnumerator();
        }
    }

    internal static class DefaultDictionary
    {
        public static DefaultDictionary<TKey, TValue> CreateWithDefaultValue<TKey, TValue>(TValue defaultValue)
        {
            return new DefaultDictionary<TKey, TValue>(_key => defaultValue);
        }
    }
}

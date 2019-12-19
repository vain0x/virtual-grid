using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Rendering
{
    /// <summary>
    /// セルの属性のデータを持つもの
    ///
    /// <para>
    /// NOTE: int や DateTime などの struct 型の値を object 型にキャストするとボックス化処理が発生し、性能が低下する。
    ///       ここでは値を常に型 T で持つことでボックス化を回避している。
    /// </para>
    /// </summary>
    /// <typeparam name="T">
    /// 属性値の型
    /// </typeparam>
    public sealed class GridAttributeData<T>
    {
        // FIXME: Dictionary は遅い？
        private Dictionary<object, T> _old =
            new Dictionary<object, T>();

        private Dictionary<object, T> _new =
            new Dictionary<object, T>();

        private readonly HashSet<object> _newKeys =
            new HashSet<object>();

        public readonly T DefaultValue;

        public IEnumerable<object> OldKeys
        {
            get
            {
                return _old.Keys;
            }
        }

        public IEnumerable<object> NewKeys
        {
            get
            {
                return _newKeys;
            }
        }

        public GridAttributeData(T defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public void SetValue(object elementKey, T value)
        {
            _new[elementKey] = value;

            if (!_old.ContainsKey(elementKey))
            {
                _newKeys.Add(elementKey);
            }
        }

        public T GetOldValue(object elementKey)
        {
            T value;
            return _old.TryGetValue(elementKey, out value) ? value : DefaultValue;
        }

        public T GetNewValue(object elementKey)
        {
            T value;
            return _new.TryGetValue(elementKey, out value) ? value : DefaultValue;
        }

        public bool TryGetDelta(object elementKey, out GridAttributeDeltaKind kind, out T oldValue, out T newValue)
        {
            if (_old.TryGetValue(elementKey, out oldValue))
            {
                if (_new.TryGetValue(elementKey, out newValue))
                {
                    // old, new の両方に存在するなら値が異なるときだけ差分があって、値が変更されたとみなす。
                    kind = GridAttributeDeltaKind.Change;
                    return !EqualityComparer<T>.Default.Equals(oldValue, newValue);
                }

                // old にあって new にないなら差分があって、削除されたとみなす。
                kind = GridAttributeDeltaKind.Remove;
                return true;
            }
            else
            {
                // old にないなら new があるときだけ差分があって、追加されたとみなす。
                kind = GridAttributeDeltaKind.Add;
                return _new.TryGetValue(elementKey, out newValue);
            }
        }

        public void MarkAsClean()
        {
            var t = _old;
            _old = _new;
            _new = t;

            _new.Clear();
            _newKeys.Clear();
        }
    }
}

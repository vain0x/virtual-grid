using System;
using System.Diagnostics;
using VirtualGrid.Rendering;

namespace VirtualGrid.WinFormsDemo
{
    /// <summary>
    /// イベント属性の実装を共通化するためのもの
    /// </summary>
    public sealed class GridEventAttributeProvider<T>
    {
        private readonly GridAttributeData<T> _data;

        public GridEventAttributeProvider()
        {
            _data = new GridAttributeData<T>(default(T));
        }

        public T GetValue(object elementKey)
        {
            return _data.GetOldValue(elementKey);
        }

        public void SetValue(object elementKey, T value)
        {
            _data.SetValue(elementKey, value);
        }

        public void ApplyDiff()
        {
            _data.MarkAsClean();
        }
    }
}

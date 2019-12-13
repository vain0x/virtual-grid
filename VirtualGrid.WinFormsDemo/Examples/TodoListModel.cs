using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.WinFormsDemo.Examples
{
    public sealed class TodoListModel
    {
        private readonly List<TodoItem> _items =
            new List<TodoItem>();

        public IReadOnlyList<TodoItem> Items
        {
            get
            {
                return _items;
            }
        }

        public TodoListModel()
        {
            EnsureLastBlank();
        }

        /// <summary>
        /// 空欄でない項目の件数
        /// </summary>
        public int NonBlankCount()
        {
            return Items.Count(item => !item.IsBlank);
        }

        public void InsertBefore(TodoItem sibling)
        {
            var index = _items.FindIndex(item => item == sibling);
            if (index < 0)
            {
                InsertLast();
                return;
            }

            _items.Insert(index, new TodoItem());

            EnsureLastBlank();
        }

        public void InsertLast()
        {
            _items.Add(new TodoItem());

            EnsureLastBlank();
        }

        public void Remove(TodoItem item)
        {
            _items.Remove(item);

            EnsureLastBlank();
        }

        public void SetItemText(TodoItem item, string text)
        {
            item.Text = text;

            EnsureLastBlank();
        }

        /// <summary>
        /// 常に末尾に1個の空欄があるようにする。
        /// </summary>
        private void EnsureLastBlank()
        {
            if (_items.Count == 0 || !_items[_items.Count - 1].IsBlank)
            {
                InsertLast();
            }
        }
    }

    public sealed class TodoItem
    {
        public bool IsBlank
        {
            get
            {
                return string.IsNullOrWhiteSpace(Text);
            }
        }

        public string Text { get; set; }
    }
}

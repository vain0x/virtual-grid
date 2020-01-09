using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.WinFormsDemo.Examples
{
    public sealed class TodoListDelta
    {
        public readonly string Kind;

        public readonly TodoItem Item;

        public readonly int Index;

        public TodoListDelta(string kind, TodoItem item, int index)
        {
            Kind = kind;
            Item = item;
            Index = index;
        }
    }

    public sealed class TodoListModel
    {
        private readonly List<TodoItem> _items =
            new List<TodoItem>();

        private readonly List<TodoListDelta> _diff =
            new List<TodoListDelta>();

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
                return;

            var newItem = new TodoItem();
            _items.Insert(index, newItem);
            _diff.Add(new TodoListDelta("INSERT", newItem, index));

            EnsureLastBlank();
        }

        public void InsertLast()
        {
            var newItem = new TodoItem();
            _items.Add(newItem);
            _diff.Add(new TodoListDelta("INSERT", newItem, _items.Count - 1));

            EnsureLastBlank();
        }

        public void Remove(TodoItem item)
        {
            var index = _items.FindIndex(x => x == item);
            if (index < 0)
                return;

            _items.RemoveAt(index);
            _diff.Add(new TodoListDelta("REMOVE", item, index));

            EnsureLastBlank();
        }

        public void SetIsDone(TodoItem item, bool isDone)
        {
            item.IsDone = isDone;
            _diff.Add(new TodoListDelta("CHANGE", item, -1));
        }

        public void SetItemText(TodoItem item, string text)
        {
            item.Text = text;
            _diff.Add(new TodoListDelta("CHANGE", item, -1));

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

        public TodoListDelta[] DrainDiff()
        {
            var diff = _diff.ToArray();
            _diff.Clear();
            return diff;
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

        public bool IsDone { get; set; }

        public string Text { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Layouts;

namespace VirtualGrid.Models
{
    public sealed class VGridBuilder
    {
        private long _lastId = 0;

        private readonly List<VCell> _cells =
            new List<VCell>();

        private readonly HashSet<object> _elementKeys =
            new HashSet<object>();

        public IGridLayoutStack NewColumn()
        {
            return new StackGridLayoutStack(horizontal: false);
        }

        public IGridLayoutStack NewRow()
        {
            return new StackGridLayoutStack(horizontal: true);
        }

        private string FreshKey()
        {
            _lastId++;
            return "?" + _lastId;
        }

        public VCell NewCell(object elementKey = null)
        {
            if (elementKey == null)
            {
                elementKey = FreshKey();
            }

            var cell = new VCell(elementKey);

            if (!_elementKeys.Add(elementKey))
                throw new ArgumentException("elementKey が重複しています。同じキーを持つセルを同じグリッドに配置することはできません。");

            _cells.Add(cell);
            return cell;
        }

        public VCell NewText(string text, object elementKey = null)
        {
            return NewCell(elementKey).WithText(text).WithReadOnly(true);
        }

        public VCell NewEdit(string text, object elementKey = null)
        {
            return NewCell(elementKey).WithText(text);
        }

        public VGrid Finish()
        {
            return VGrid.Empty.WithCells(_cells.ToArray());
        }

        public VCell NewButton(string text, object elementKey = null)
        {
            // FIXME: ボタンは未実装
            return NewText(text, elementKey);
        }
    }
}

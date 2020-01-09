using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;

namespace VirtualGrid.RowsComponents
{
    public interface IGridRowsElementListener
    {
        void OnAdd(object rowKey);

        void OnRemove(object rowKey);

        void OnChange(object rowKey);

        void Patch();
    }

    // 列全体といくつかの行を占有する要素
    public sealed class GridRowsElement<TData>
    {
        public readonly GridHeaderList RowHeader;

        public readonly IGridHeaderNode ColumnHeader;

        private readonly IGridRowsElementListener _listener;

        public GridRowsElement(GridHeaderList rowHeader, IGridHeaderNode columnHeader, IGridRowsElementListener listener)
        {
            RowHeader = rowHeader;
            ColumnHeader = columnHeader;
            _listener = listener;
        }

        public Builder GetBuilder()
        {
            return new Builder(this, RowHeader.GetBuilder());
        }

        private void Patch()
        {
            _listener.Patch();
        }

        public struct Builder
        {
            private GridRowsElement<TData> _parent;

            private GridHeaderListBuilder _rowList;

            public Builder(GridRowsElement<TData> parent, GridHeaderListBuilder rowList)
            {
                _parent = parent;
                _rowList = rowList;
            }

            public void Insert(int index, object rowKey)
            {
                _parent._listener.OnAdd(rowKey);
                _rowList.Insert(index, rowKey);
            }

            public void RemoveAt(int index, object rowKey)
            {
                _parent._listener.OnRemove(rowKey);
                _rowList.RemoveAt(index);
            }

            public void Update(object rowKey)
            {
                _parent._listener.OnChange(rowKey);
            }

            public void Patch()
            {
                _rowList.Patch();
                _parent.Patch();
            }
        }
    }
}

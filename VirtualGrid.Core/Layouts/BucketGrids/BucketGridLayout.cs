using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;
using VirtualGrid.Layouts;

namespace VirtualGrid.Layouts.BucketGrids
{
    public sealed class BucketGridLayout
        : IGridLayout
    {
        public BucketGridHeaderLayout RowHeader;

        public BucketGridHeaderLayout ColumnHeader;

        public BucketGridBodyLayout Body;

        internal GridHeaderNode RowHeaderNode =
            new GridHeaderNode();

        internal GridHeaderNode ColumnHeaderNode =
            new GridHeaderNode();

        internal Dictionary<Tuple<object, object>, object> BodyCells =
            new Dictionary<Tuple<object, object>, object>();

        public BucketGridLayout()
        {
            RowHeader = new BucketGridHeaderLayout(this, isRowHeader: true);

            ColumnHeader = new BucketGridHeaderLayout(this, isRowHeader: false);

            Body = new BucketGridBodyLayout(this);
        }

        IGridPartLayout IGridLayout.RowHeader
        {
            get
            {
                return RowHeader;
            }
        }

        IGridPartLayout IGridLayout.ColumnHeader
        {
            get
            {
                return ColumnHeader;
            }
        }

        IGridPartLayout IGridLayout.Body
        {
            get
            {
                return Body;
            }
        }
    }

    public sealed class BucketGridHeaderLayout
        : IGridPartLayout
    {
        private BucketGridLayout _layout;

        private bool _isRowHeader;

        public BucketGridHeaderLayout(BucketGridLayout layout, bool isRowHeader)
        {
            _layout = layout;
            _isRowHeader = isRowHeader;
        }

        public IEnumerable<GridElementKey> Hit(GridVector index)
        {
            return _isRowHeader
                ? _layout.RowHeaderNode.Hit(null, index.Row.Row)
                    .Select(elementKey => GridElementKey.NewRowHeader(elementKey))
                : _layout.ColumnHeaderNode.Hit(null, index.Column.Column)
                    .Select(elementKey => GridElementKey.NewColumnHeader(elementKey));
        }
    }

    public sealed class BucketGridBodyLayout
        : IGridPartLayout
    {
        private BucketGridLayout _layout;

        public BucketGridBodyLayout(BucketGridLayout layout)
        {
            _layout = layout;
        }

        public IEnumerable<GridElementKey> Hit(GridVector index)
        {
            // FIXME: 複数行ヘッダーに対応
            foreach (var rowElementKey in _layout.RowHeader.Hit(index.Row.AsVector))
            {
                foreach (var columnElementKey in _layout.ColumnHeader.Hit(index.Column.AsVector))
                {
                    yield return GridElementKey.NewBody(rowElementKey.RowElementKeyOpt, columnElementKey.ColumnElementKeyOpt);
                }
            }
        }
    }
}

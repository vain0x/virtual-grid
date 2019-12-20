using System.Collections.Generic;
using System.Linq;
using VirtualGrid.Layouts;

namespace VirtualGrid.Rendering
{
    /// <summary>
    /// グリッドのヘッダーを構築するもの。
    ///
    /// カラムヘッダーなら横に、ローヘッダーなら縦に要素を積んでいく。
    /// </summary>
    public sealed class GridHeaderBuilder<TProvider>
        : IGridLayoutBuilder
    {
        private List<IGridLayoutBuilder> _layouts;

        private bool _horizontal;

        private GridRenderContext<TProvider> _context;

        public GridHeaderBuilder(List<IGridLayoutBuilder> layouts, bool horizontal, GridRenderContext<TProvider> context)
        {
            _layouts = layouts;
            _horizontal = horizontal;
            _context = context;
        }

        private GridPart GridPart
        {
            get
            {
                return _horizontal
                    ? GridPart.ColumnHeader
                    : GridPart.RowHeader;
            }
        }

        public GridHeaderCellAdder WithKey(object elementKey)
        {
            return new GridHeaderCellAdder(this, elementKey);
        }

        IGridLayoutNode IGridLayoutBuilder.ToGridLayoutNode()
        {
            if (_layouts.Count == 0)
            {
                var elementKey = _horizontal ? "?_EMPTY_COLUMN_HEADER" : "?_EMPTY_ROW_HEADER";
                return new EmptyGridLayoutNode(elementKey);
            }

            return new StackGridLayoutNode(_layouts.Select(layout => layout.ToGridLayoutNode()).ToArray(), _horizontal);
        }

        public struct GridHeaderCellAdder
            : IGridCellAdder<TProvider>
        {
            private GridHeaderBuilder<TProvider> _parent;

            private readonly object _elementKey;

            public GridHeaderCellAdder(GridHeaderBuilder<TProvider> parent, object elementKey)
            {
                _parent = parent;
                _elementKey = elementKey;
            }

            public IGridCellBuilder<TProvider> AddCell()
            {
                var cell = new IGridCellBuilder<TProvider>(_elementKey, _parent._context);
                _parent._layouts.Add(cell);
                _parent._context.AddCell(_parent.GridPart, null, null, _elementKey);
                return cell;
            }
        }
    }
}

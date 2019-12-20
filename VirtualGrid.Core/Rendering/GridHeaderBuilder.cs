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

        public IGridCellAdder<TProvider> WithKey(object elementKey)
        {
            return new AnonymousGridCellAdder<TProvider>(() =>
            {
                var cell = new IGridCellBuilder<TProvider>(elementKey, _context);
                _layouts.Add(cell);
                _context.AddCell(GridPart, null, null, elementKey);
                return cell;
            });
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
    }
}

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
    public sealed class GridHeaderBuilder
        : IGridLayoutBuilder
    {
        private List<IGridLayoutBuilder> _layouts;

        private bool _horizontal;

        private GridRenderContext _context;

        public GridHeaderBuilder(List<IGridLayoutBuilder> layouts, bool horizontal, GridRenderContext context)
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

        public IGridCellAdder WithKey(object elementKey)
        {
            return new AnonymousGridCellAdder(() =>
            {
                var cell = new IGridCellBuilder(elementKey, _context);
                _layouts.Add(cell);
                _context.AddCell(GridPart, null, null, elementKey);
                return cell;
            });
        }

        private IGridLayoutNode Combine(IGridLayoutNode first, IGridLayoutBuilder second)
        {
            if (first == null)
                return second.ToGridLayoutNode();

            if (_horizontal)
                return new HorizontalLinkGridLayoutNode(first, second.ToGridLayoutNode());

            return new VerticalLinkGridLayoutNode(first, second.ToGridLayoutNode());
        }

        IGridLayoutNode IGridLayoutBuilder.ToGridLayoutNode()
        {
            return _layouts.Aggregate(default(IGridLayoutNode), Combine)
                ?? new EmptyGridLayoutNode(this); // FIXME: elementKey
        }
    }
}

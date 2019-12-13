using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Layouts;
using VirtualGrid.Models;

namespace VirtualGrid.Diff
{
    internal sealed class GridLayoutDiffer
    {
        private readonly VGrid _oldGrid;

        private readonly VGrid _newGrid;

        private readonly GridLayoutModel _layoutModel;

        private readonly List<GridLayoutDelta> _layoutDiff;

        private readonly List<AttributeDelta> _attributeDiff;

        public void Diff()
        {
            throw new NotImplementedException();
        }
    }
}

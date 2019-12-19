using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    /// <summary>
    /// グリッドのレイアウトの差分を計算する。
    /// </summary>
    public sealed class GridLayoutDiffer
    {
        private GridPivots _old;

        private GridPivots _new;

        private List<GridLayoutDelta> _diff;

        public GridLayoutDiffer(GridPivots old, GridPivots @new, List<GridLayoutDelta> diff)
        {
            _old = old;
            _new = @new;
            _diff = diff;
        }

        private void DiffCore(object[] oldPivots, object[] newPivots, GridLayoutDeltaKind insertKind, GridLayoutDeltaKind removeKind)
        {
            var si = 0;
            var ti = 0;

            var oldKeys = new HashSet<object>(oldPivots);
            var newKeys = new HashSet<object>(newPivots);

            while (si < oldPivots.Length || ti < newPivots.Length)
            {
                var doesInsert = si == oldPivots.Length
                    || (ti < newPivots.Length && !oldKeys.Contains(newPivots[ti]));
                if (doesInsert)
                {
                    _diff.Add(GridLayoutDelta.Create(insertKind, ti));
                    ti++;
                    continue;
                }

                Debug.Assert(si < oldPivots.Length);

                var doesRemove = ti == newPivots.Length
                    || !newKeys.Contains(oldPivots[si]);
                if (doesRemove)
                {
                    _diff.Add(GridLayoutDelta.Create(removeKind, ti));
                    si++;
                    continue;
                }

                Debug.Assert(ti < newPivots.Length);

                if (EqualityComparer<object>.Default.Equals(oldPivots[si], newPivots[ti]))
                {
                    si++;
                    ti++;
                    continue;
                }

                _diff.Add(GridLayoutDelta.Create(insertKind, ti));
                ti++;

                _diff.Add(GridLayoutDelta.Create(removeKind, ti));
                si++;
            }
        }

        private void DiffRows()
        {
            DiffCore(_old.RowPivots, _new.RowPivots, GridLayoutDeltaKind.InsertRow, GridLayoutDeltaKind.RemoveRow);
        }

        private void DiffColumns()
        {
            DiffCore(_old.ColumnPivots, _new.ColumnPivots, GridLayoutDeltaKind.InsertColumn, GridLayoutDeltaKind.RemoveColumn);
        }

        public void MakeDiff()
        {
            DiffRows();
            DiffColumns();
        }
    }
}

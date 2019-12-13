using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Abstraction;
using VirtualGrid.Layouts;
using VirtualGrid.Models;

namespace VirtualGrid.WinFormsDemo
{
    public sealed class DataGridViewGridProvider
        : IGridProvider
    {
        private readonly DataGridView _inner;

        private VGrid _lastGrid = VGrid.Empty;

        private readonly GridLayoutModel _layoutModel =
            new GridLayoutModel();

        private readonly Action<object, Action> _dispatch;

        public DataGridViewGridProvider(DataGridView inner, Action<object, Action> dispatch)
        {
            _inner = inner;

            _dispatch = (elementKey, action) =>
            {
                Debug.WriteLine("Dispatch({0}, {1})", elementKey, action);
                dispatch(elementKey, action);
            };

            Initialize();
        }

        void Initialize()
        {
            _inner.CellClick += (sender, ev) =>
            {
                var row = RowIndex.From(ev.RowIndex);
                var column = ColumnIndex.From(ev.ColumnIndex);
                var index = GridVector.Create(row, column);

                foreach (var elementKey in _layoutModel.Locate(index))
                {
                    var vCell = _lastGrid.FindCell(elementKey);
                    if (vCell == null)
                        continue;

                    var action = vCell.Attributes["A_ON_CLICK"] as Action;
                    if (action == null)
                        continue;

                    _dispatch(vCell.ElementKey, action);
                }
            };

            _inner.CellValueChanged += (sender, ev) =>
            {
                var row = RowIndex.From(ev.RowIndex);
                var column = ColumnIndex.From(ev.ColumnIndex);
                var index = GridVector.Create(row, column);

                foreach (var elementKey in _layoutModel.Locate(index))
                {
                    var vCell = _lastGrid.FindCell(elementKey);
                    if (vCell == null)
                        continue;

                    var action = vCell.Attributes["A_ON_CHANGED"] as Action<object>;
                    if (action == null)
                        continue;

                    var value = _inner.Rows[row.Row].Cells[column.Column].Value;

                    _dispatch(vCell.ElementKey, () => action(value));
                }
            };
        }

        public void Render(VGrid grid)
        {
            var measure = grid.Body.Measure(GridMeasure.Infinite, _layoutModel);
            var range = GridRange.Create(GridVector.Zero, measure);

            grid.Body.Arrange(range, _layoutModel);

            // 余った列を削除する。
            for (var i = _inner.Columns.Count; i > measure.Column;)
            {
                i--;

                Debug.WriteLine("Remove column");
                _inner.Columns.RemoveAt(i);
            }

            // 足りない列を追加する。
            for (var i = _inner.Columns.Count; i < measure.Column; i++)
            {
                Debug.WriteLine("Add column");
                _inner.Columns.Add(i.ToString(), i.ToString());
            }

            // 余った行を削除する。
            for (var i = _inner.Rows.Count; i > measure.Row;)
            {
                i--;

                if (!_inner.Rows[i].IsNewRow)
                {
                    Debug.WriteLine("Remove row");
                    _inner.Rows.RemoveAt(i);
                }
            }

            // 足りない行を追加する。
            for (var i = _inner.Rows.Count; i < measure.Row; i++)
            {
                Debug.WriteLine("Add row");
                _inner.Rows.Add();
            }

            // セルを更新する。
            foreach (var vCell in grid.Cells)
            {
                var point = _layoutModel.Touch(vCell.ElementKey).LastArrange.Start;

                var cellElement = _inner.Rows[point.Row.Row].Cells[point.Column.Column];

                if (!EqualityComparer<object>.Default.Equals(cellElement.Value, vCell.Value))
                {
                    Debug.WriteLine("Cell[{0}].Value = {1}", point.AsDebug, vCell.Value);
                    cellElement.Value = vCell.Value;
                }

                var readOnly = vCell.Attributes["A_READ_ONLY"] as bool? == true;
                if (cellElement.ReadOnly != readOnly)
                {
                    cellElement.ReadOnly = readOnly;
                }
            }

            _lastGrid = grid;
        }
    }
}

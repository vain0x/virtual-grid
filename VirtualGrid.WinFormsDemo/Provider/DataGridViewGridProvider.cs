using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Abstraction;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;

namespace VirtualGrid.WinFormsDemo
{
    public sealed class DataGridViewGridProvider
        : IGridProvider
    {
        internal readonly DataGridView _inner;

        public readonly GridDataAttributeProvider<bool, IsCheckedAttributePolicy> IsCheckedAttribute;

        public readonly GridEventAttributeProvider<Action<bool>> OnCheckChangedAttribute;

        public readonly GridEventAttributeProvider<Action> OnClickAttribute;

        public readonly GridEventAttributeProvider<Action<string>> OnTextChangedAttribute;

        public readonly GridDataAttributeProvider<bool, ReadOnlyAttributePolicy> ReadOnlyAttribute;

        public readonly GridDataAttributeProvider<string, TextAttributePolicy> TextAttribute;

        private IGridLayoutNode _columnHeaderLayout =
            GridLayoutNode.Empty("?_EMPTY_COLUMN_HEADER");

        private GridPivots _columnHeaderPivots =
            GridPivots.Empty;

        private IGridLayoutNode _rowHeaderLayout =
            GridLayoutNode.Empty("?_EMPTY_ROW_HEADER");

        private GridPivots _rowHeaderPivots =
            GridPivots.Empty;

        private IGridLayoutNode _bodyLayout =
            GridLayoutNode.Empty("?_EMPTY_BODY");

        internal IDictionary<object, GridLocation> _locationMap =
            new Dictionary<object, GridLocation>();

        private readonly GridLayoutContext _layoutContext =
            new GridLayoutContext();

        private readonly GridRenderContext<DataGridViewGridProvider> _renderContext;

        private readonly Action<object, Action> _dispatch;

        public DataGridViewGridProvider(DataGridView inner, Action<object, Action> dispatch)
        {
            _inner = inner;

            IsCheckedAttribute = GridDataAttributeProvider.Create(default(bool), new IsCheckedAttributePolicy(this), this);

            OnCheckChangedAttribute = new GridEventAttributeProvider<Action<bool>>();

            OnClickAttribute = new GridEventAttributeProvider<Action>();

            OnTextChangedAttribute = new GridEventAttributeProvider<Action<string>>();

            ReadOnlyAttribute = GridDataAttributeProvider.Create(default(bool), new ReadOnlyAttributePolicy(this), this);

            TextAttribute = GridDataAttributeProvider.Create(default(string), new TextAttributePolicy(this), this);

            _renderContext = new GridRenderContext<DataGridViewGridProvider>(this);

            _dispatch = (elementKey, action) =>
            {
                Debug.WriteLine("Dispatch({0}, {1})", elementKey, action);
                dispatch(elementKey, action);
            };

            SubscribeEvents();
        }

        private void UpdateLocationMap()
        {
            _locationMap.Clear();

            foreach (var t in _renderContext._cells)
            {
                var part = t.GridPart;
                var rowKey = t.RowElementKey;
                var columnKey = t.ColumnElementKey;
                var elementKey = t.CellElementKey;

                if (part == GridPart.Body)
                {
                    var row = _layoutContext.LastArrange(rowKey).Start.Row;
                    var column = _layoutContext.LastArrange(columnKey).Start.Column;
                    _locationMap.Add(elementKey, GridLocation.NewBody(GridVector.Create(row, column)));
                }
                else
                {
                    var index = _layoutContext.LastArrange(elementKey).Start;
                    _locationMap.Add(elementKey, GridLocation.Create(part, index));
                }
            }
        }

        public GridBuilder<DataGridViewGridProvider> GetBuilder()
        {
            _renderContext.Clear();
            return new GridBuilder<DataGridViewGridProvider>(_renderContext);
        }

        private void AddColumnHeaderRow(int rowIndex)
        {
            Debug.Assert(rowIndex == 0);
        }

        private void AddColumnHeaderColumn(int columnIndex)
        {
            _inner.Columns.Insert(columnIndex, new DataGridViewColumn()
            {
                HeaderText = "",
                CellTemplate = new DataGridViewTextBoxCell(),
                Width = 100,
            });
        }

        private void RemoveColumnHeaderRow(int _rowIndex)
        {
            Debug.Assert(false, "カラムヘッダーの行は削除できません。");
        }

        private void RemoveColumnHeaderColumn(int columnIndex)
        {
            _inner.Columns.RemoveAt(columnIndex);
        }

        private void AddRowHeaderRow(int rowIndex)
        {
            _inner.Rows.Insert(rowIndex, 1);
        }

        private void AddRowHeaderColumn(int columnIndex)
        {
            // Debug.Assert(columnIndex == 0);
        }

        private void RemoveRowHeaderRow(int rowIndex)
        {
            _inner.Rows.RemoveAt(rowIndex);
        }

        private void RemoveRowHeaderColumn(int _columnIndex)
        {
            // FIXME: ローヘッダーの1列目の主成分が1行目のキーになっているので、
            //        1行目に挿入・削除が起こるたびにカラムが追加・削除されてしまう。
            // Debug.Assert(false, "ローヘッダーの列は削除できません。");
        }

        private void AddRow(GridPart part, RowIndex rowIndex)
        {
            switch (part)
            {
                case GridPart.ColumnHeader:
                    AddColumnHeaderRow(rowIndex.Row);
                    return;

                case GridPart.RowHeader:
                    AddRowHeaderRow(rowIndex.Row);
                    return;

                case GridPart.Body:
                    throw new InvalidOperationException("Body には列を追加できません。ローヘッダーに行を追加してください。");

                default:
                    throw new Exception("Unknown GridPart");
            }
        }

        private void AddColumn(GridPart part, ColumnIndex columnIndex)
        {
            switch (part)
            {
                case GridPart.ColumnHeader:
                    AddColumnHeaderColumn(columnIndex.Column);
                    return;

                case GridPart.RowHeader:
                    AddRowHeaderColumn(columnIndex.Column);
                    return;

                case GridPart.Body:
                    throw new InvalidOperationException("Body には行を追加できません。カラムヘッダーに列を追加してください。");

                default:
                    throw new Exception("Unknown GridPart");
            }
        }

        private void RemoveRow(GridPart part, RowIndex rowIndex)
        {
            switch (part)
            {
                case GridPart.ColumnHeader:
                    RemoveColumnHeaderRow(rowIndex.Row);
                    return;

                case GridPart.RowHeader:
                    RemoveRowHeaderRow(rowIndex.Row);
                    return;

                case GridPart.Body:
                    throw new InvalidOperationException("Body の行を削除できません。ローヘッダーから行を削除してください。");

                default:
                    throw new Exception("Unknown GridPart");
            }
        }

        private void RemoveColumn(GridPart part, ColumnIndex columnIndex)
        {
            switch (part)
            {
                case GridPart.ColumnHeader:
                    RemoveColumnHeaderColumn(columnIndex.Column);
                    return;

                case GridPart.RowHeader:
                    RemoveRowHeaderColumn(columnIndex.Column);
                    return;

                case GridPart.Body:
                    throw new InvalidOperationException("Body の列を削除できません。カラムヘッダーの列を追加してください。");

                default:
                    throw new Exception("Unknown GridPart");
            }
        }

        private void ApplyGridLyaoutDelta(GridPart part, GridLayoutDelta delta)
        {
            switch (delta.Kind)
            {
                case GridLayoutDeltaKind.InsertRow:
                    AddRow(part, RowIndex.From(delta.Index));
                    return;

                case GridLayoutDeltaKind.InsertColumn:
                    AddColumn(part, ColumnIndex.From(delta.Index));
                    return;

                case GridLayoutDeltaKind.RemoveRow:
                    RemoveRow(part, RowIndex.From(delta.Index));
                    return;

                case GridLayoutDeltaKind.RemoveColumn:
                    RemoveColumn(part, ColumnIndex.From(delta.Index));
                    return;

                default:
                    throw new Exception("Unknown GridLayoutDeltaKind");
            }
        }

        private void ApplyGridLayoutDiff(GridPart part, List<GridLayoutDelta> diff)
        {
            foreach (var delta in diff)
            {
                ApplyGridLyaoutDelta(part, delta);
            }
        }

        private void SubscribeEvents()
        {
            _inner.CellClick += (sender, ev) =>
            {
                // ヘッダーのイベントは未実装
                if (ev.RowIndex < 0 || ev.ColumnIndex < 0)
                    return;

                var row = RowIndex.From(ev.RowIndex);
                var column = ColumnIndex.From(ev.ColumnIndex);
                var index = GridVector.Create(row, column);

                // FIXME: 効率化
                foreach (var pair in _locationMap)
                {
                    if (pair.Value.Part == GridPart.Body && pair.Value.Index == index)
                    {
                        var elementKey = pair.Key;

                        // チェックボックスのチェックを実装する。
                        // FIXME: セルタイプを見る。
                        if (IsCheckedAttribute.IsAttached(elementKey))
                        {
                            var isChecked = IsCheckedAttribute.GetValue(elementKey);
                            var action = OnCheckChangedAttribute.GetValue(elementKey);
                            if (action != null)
                            {
                                _dispatch(elementKey, () => action(!isChecked));
                            }
                        }

                        {
                            var action = OnClickAttribute.GetValue(elementKey);
                            if (action == null)
                                continue;

                            _dispatch(elementKey, action);
                        }
                    }
                }
            };

            _inner.CellValueChanged += (sender, ev) =>
            {
                if (ev.RowIndex < 0 || ev.ColumnIndex < 0)
                    return;

                var value = _inner.Rows[ev.RowIndex].Cells[ev.ColumnIndex].Value;

                var row = RowIndex.From(ev.RowIndex);
                var column = ColumnIndex.From(ev.ColumnIndex);
                var index = GridVector.Create(row, column);

                foreach (var pair in _locationMap)
                {
                    if (pair.Value.Part == GridPart.Body && pair.Value.Index == index)
                    {
                        var elementKey = pair.Key;

                        {
                            var text = value as string;
                            if (text != null || value == null)
                            {
                                var action = OnTextChangedAttribute.GetValue(elementKey);
                                if (action != null)
                                {
                                    _dispatch(elementKey, () => action(text ?? ""));
                                }
                            }
                        }
                    }
                }
            };
        }

        private void DiffHeaderLayoutCore(GridPivots oldPivots, IGridLayoutNode newLayout, out GridPivots newPivots, List<GridLayoutDelta> diff)
        {
            var measure = _layoutContext.Measure(newLayout, GridMeasure.Infinite);

            var range = GridRange.Create(GridVector.Zero, measure);
            _layoutContext.Arrange(newLayout, range);

            newPivots = _layoutContext.GetPivots(newLayout);
            new GridLayoutDiffer(oldPivots, newPivots, diff).MakeDiff();
        }

        private void UpdateColumnHeaderLayout(IGridLayoutBuilder columnHeader, List<GridLayoutDelta> diff)
        {
            var newLayout = columnHeader.ToGridLayoutNode();
            GridPivots newPivots;

            DiffHeaderLayoutCore(_columnHeaderPivots, newLayout, out newPivots, diff);

            _columnHeaderLayout = newLayout;
            _columnHeaderPivots = newPivots;
        }

        private void UpdateRowHeaderLayout(IGridLayoutBuilder rowHeader, List<GridLayoutDelta> diff)
        {
            var newLayout = rowHeader.ToGridLayoutNode();
            GridPivots newPivots;

            DiffHeaderLayoutCore(_rowHeaderPivots, newLayout, out newPivots, diff);

            _rowHeaderLayout = newLayout;
            _rowHeaderPivots = newPivots;
        }

        private void UpdateBodyLayout(GridBuilder<DataGridViewGridProvider> grid)
        {
            // FIXME: ボディーのレイアウトを実装
        }

        public void Render(GridBuilder<DataGridViewGridProvider> grid)
        {
            var columnHeaderLayoutDiff = new List<GridLayoutDelta>();
            var rowHeaderLayoutDiff = new List<GridLayoutDelta>();

            _layoutContext.Clear();

            UpdateColumnHeaderLayout(grid.ColumnHeader, columnHeaderLayoutDiff);
            UpdateRowHeaderLayout(grid.RowHeader, rowHeaderLayoutDiff);
            UpdateBodyLayout(grid);
            UpdateLocationMap();

            ApplyGridLayoutDiff(GridPart.ColumnHeader, columnHeaderLayoutDiff);
            ApplyGridLayoutDiff(GridPart.RowHeader, rowHeaderLayoutDiff);

            IsCheckedAttribute.ApplyDiff();
            OnCheckChangedAttribute.ApplyDiff();
            OnClickAttribute.ApplyDiff();
            OnTextChangedAttribute.ApplyDiff();
            ReadOnlyAttribute.ApplyDiff();
            TextAttribute.ApplyDiff();
        }
    }
}

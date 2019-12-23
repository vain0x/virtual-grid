using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Headers;
using VirtualGrid.Rendering;

namespace VirtualGrid.Layouts.BucketGrids
{
    public sealed class BucketGridHeaderBuilder<TProvider>
        : IGridHeaderDecomposer
    {
        private GridHeaderNode _node;

        private GridRenderContext<TProvider> _context;

        private IGridHeaderDeltaListener _listener;

        private bool _isRowHeader;

        private List<object> _elementKeys =
            new List<object>();

        private Dictionary<object, int> _keyMap =
            new Dictionary<object, int>();

        private List<bool> _dirtyFlags =
            new List<bool>();

        private List<Action<BucketGridHeaderBuilder<TProvider>>> _renderFuncOpts =
            new List<Action<BucketGridHeaderBuilder<TProvider>>>();
        
        public BucketGridHeaderBuilder(GridHeaderNode node, GridRenderContext<TProvider> context, IGridHeaderDeltaListener listener, bool isRowHeader)
        {
            _node = node;
            _context = context;
            _listener = listener;
            _isRowHeader = isRowHeader;
        }

        private void AddCore(object elementKey, bool isDirty, Action<BucketGridHeaderBuilder<TProvider>> renderFuncOpt)
        {
            if (elementKey == null)
                throw new ArgumentNullException("elementKey");

            if (_keyMap.ContainsKey(elementKey))
                throw new ArgumentException("elementKey duplicated");

            var index = _elementKeys.Count;
            _keyMap.Add(elementKey, index);
            _elementKeys.Add(elementKey);
            _dirtyFlags.Add(isDirty);
            _renderFuncOpts.Add(renderFuncOpt);
        }

        private GridElementKey CreateElementKey(object elementKey)
        {
            return _isRowHeader
                ? GridElementKey.NewRowHeader(elementKey)
                : GridElementKey.NewColumnHeader(elementKey);
        }

        public IGridCellAdder<TProvider> WithKey(object elementKey)
        {
            return new AnonymousGridCellAdder<TProvider>(() =>
            {
                AddCore(elementKey, true, null);

                var cell = new IGridCellBuilder<TProvider>(CreateElementKey(elementKey), _context);
                return cell;
            });
        }

        public void AddBucket(object elementKey, bool isDirty, Action<BucketGridHeaderBuilder<TProvider>> renderFunc)
        {
            if (renderFunc == null)
                throw new ArgumentNullException("renderFunc");

            AddCore(elementKey, isDirty, renderFunc);
        }

        public void Patch()
        {
            new GridHeaderNodePatcher(_node, _elementKeys, this, 0, _listener).Patch();
        }

        GridHeaderDecomposition? IGridHeaderDecomposer.Decompose(object elementKey)
        {
            int index;
            if (!_keyMap.TryGetValue(elementKey, out index))
            {
                return null;
            }

            var renderFuncOpt = _renderFuncOpts[index];
            if (renderFuncOpt == null)
                return null;

            _renderFuncOpts[index] = _ =>
            {
                throw new InvalidOperationException("分解は1度だけ行える");
            };

            var newBuilder = new BucketGridHeaderBuilder<TProvider>(new GridHeaderNode(), _context, _listener, _isRowHeader);
            renderFuncOpt(newBuilder);

            // FIXME: newBuilder の内部にある _renderFuncOpts を使う。
            return new GridHeaderDecomposition(newBuilder._elementKeys, isDirty: _dirtyFlags[index]);
        }
    }
}

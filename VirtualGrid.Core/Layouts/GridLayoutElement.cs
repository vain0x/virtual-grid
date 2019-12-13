using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    public sealed class GridLayoutElement
    {
        public readonly object ElementKey;

        public readonly IDictionary<string, object> Attributes =
            new DefaultDictionary<string, object>(_ => null);

        public readonly IDictionary<string, bool> DirtyAttributes =
            new DefaultDictionary<string, bool>(_ => false);

        private GridVector _lastMeasure = GridVector.Zero;

        private bool _lastMeasureIsDirty = false;

        public bool LastMeasureIsDirty
        {
            get
            {
                return _lastMeasureIsDirty;
            }
        }

        private GridRange _lastArrange = GridRange.Zero;

        private bool _lastArrangeIsDirty = false;

        public GridLayoutElement(object elementKey)
        {
            ElementKey = elementKey;
        }

        public GridVector LastMeasure
        {
            get
            {
                return _lastMeasure;
            }
            set
            {
                if (_lastMeasure != value)
                {
                    _lastMeasureIsDirty = true;
                    _lastMeasure = value;
                }
            }
        }

        public GridRange LastArrange
        {
            get
            {
                return _lastArrange;
            }
            set
            {
                if (_lastArrange != value)
                {
                    _lastArrangeIsDirty = true;
                    _lastArrange = value;
                }
            }
        }
    }
}

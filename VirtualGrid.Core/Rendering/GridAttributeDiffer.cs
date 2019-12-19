using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VirtualGrid.Rendering
{
    public sealed class GridAttributeDiffer
    {
        private GridAttributeBinding[] _oldBindings;

        private GridAttributeBinding[] _newBindings;

        private List<GridAttributeDelta> _diff;

        public GridAttributeDiffer(GridAttributeBinding[] oldBindings, GridAttributeBinding[] newBindings, List<GridAttributeDelta> diff)
        {
            _oldBindings = oldBindings;
            _newBindings = newBindings;
            _diff = diff;
        }

        public void MakeDiff()
        {
            var si = 0;
            var ti = 0;

            var oldKeys = new HashSet<object>(_oldBindings.Select(binding => binding.GetKey()));
            var newKeys = new HashSet<object>(_newBindings.Select(binding => binding.GetKey()));

            while (si < _oldBindings.Length || ti < _newBindings.Length)
            {
                var doesInsert = si == _oldBindings.Length
                    || (ti < _newBindings.Length && !oldKeys.Contains(_newBindings[ti].GetKey()));
                if (doesInsert)
                {
                    _diff.Add(GridAttributeDelta.NewAdd(_newBindings[ti]));
                    ti++;
                    continue;
                }

                Debug.Assert(si < _oldBindings.Length);

                var doesRemove = ti == _newBindings.Length
                    || !newKeys.Contains(_oldBindings[si].GetKey());
                if (doesRemove)
                {
                    _diff.Add(GridAttributeDelta.NewRemove(_oldBindings[si]));
                    si++;
                    continue;
                }

                Debug.Assert(ti < _newBindings.Length);

                if (EqualityComparer<object>.Default.Equals(_oldBindings[si].GetKey(), _newBindings[ti].GetKey()))
                {
                    if (!EqualityComparer<object>.Default.Equals(_oldBindings[si].Value, _newBindings[ti].Value))
                    {
                        _diff.Add(GridAttributeDelta.NewChange(_newBindings[ti]));
                    }

                    si++;
                    ti++;
                    continue;
                }

                _diff.Add(GridAttributeDelta.NewAdd(_newBindings[ti]));
                ti++;

                _diff.Add(GridAttributeDelta.NewRemove(_oldBindings[si]));
                si++;
            }
        }
    }
}

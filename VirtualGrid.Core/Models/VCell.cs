using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.Layouts;

namespace VirtualGrid.Models
{
    public sealed class VCell
        : IGridLayoutProvider
    {
        public readonly object ElementKey;

        public IDictionary<string, object> Attributes =
            DefaultDictionary.CreateWithDefaultValue<string, object>(null);

        public VCell(object elementKey)
        {
            ElementKey = elementKey;
        }

        public VCell WithText(string text)
        {
            Text = text;
            return this;
        }

        public object Value
        {
            get
            {
                return Attributes["A_VALUE"];
            }
            set
            {
                Attributes["A_VALUE"] = value;
            }
        }

        public string Text
        {
            get
            {
                return Value as string;
            }
            set
            {
                Value = value;
            }
        }

        public VCell OnClick(Action action)
        {
            Attributes["A_ON_CLICK"] = action;
            return this;
        }

        public VCell WithReadOnly(bool isReadOnly)
        {
            Attributes["A_READ_ONLY"] = isReadOnly;
            return this;
        }

        public IGridLayout ToGridLayout()
        {
            return new CellGridLayout(ElementKey);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualGrid.Layouts;
using VirtualGrid.Rendering;
using VirtualGrid.Spreads;

namespace VirtualGrid.WinFormsDemo
{
    public sealed class AttributeBuilder
    {
        private readonly IDataGridViewPart _part;

        public readonly GridDataAttributeProvider<bool, IsCheckedAttributePolicy> IsCheckedAttribute;

        public readonly GridEventAttributeProvider<Action<bool>> OnCheckChangedAttribute;

        public readonly GridEventAttributeProvider<Action> OnClickAttribute;

        public readonly GridEventAttributeProvider<Action<string>> OnTextChangedAttribute;

        public readonly GridDataAttributeProvider<bool, ReadOnlyAttributePolicy> ReadOnlyAttribute;

        public readonly GridDataAttributeProvider<string, TextAttributePolicy> TextAttribute;

        public AttributeBuilder(IDataGridViewPart part)
        {
            _part = part;

            IsCheckedAttribute = GridDataAttributeProvider.Create(_part, default(bool), new IsCheckedAttributePolicy());

            OnCheckChangedAttribute = new GridEventAttributeProvider<Action<bool>>();

            OnClickAttribute = new GridEventAttributeProvider<Action>();

            OnTextChangedAttribute = new GridEventAttributeProvider<Action<string>>();

            ReadOnlyAttribute = GridDataAttributeProvider.Create(_part, default(bool), new ReadOnlyAttributePolicy());

            TextAttribute = GridDataAttributeProvider.Create(_part, default(string), new TextAttributePolicy());
        }

        public void Patch()
        {
            IsCheckedAttribute.Patch();
            OnCheckChangedAttribute.Patch();
            OnClickAttribute.Patch();
            OnTextChangedAttribute.Patch();
            ReadOnlyAttribute.Patch();
            TextAttribute.Patch();
        }

        public IGridCellAdder<AttributeBuilder> At(GridRow row, GridColumn column)
        {
            return new AnonymousGridCellAdder<AttributeBuilder>(() =>
            {
                return new GridCellBuilder<AttributeBuilder>(GridElementKey.Create(row.ElementKey, column.ElementKey), this);
            });
        }
    }
}

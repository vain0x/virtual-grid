using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.RowsComponents;

namespace VirtualGrid.WinFormsDemo.Provider.RowsComponents
{
    public sealed class GridRowElementDataProvider
        : IGridRowElementDataProvider<AttributeBuilder>
    {
        private readonly IDataGridViewPart _part;

        public GridRowElementDataProvider(IDataGridViewPart part)
        {
            _part = part;
        }

        public AttributeBuilder Create()
        {
            return new AttributeBuilder(_part);
        }

        public void Destroy(AttributeBuilder data)
        {
        }

        public void Update(AttributeBuilder data)
        {
            data.Patch();
        }
    }
}

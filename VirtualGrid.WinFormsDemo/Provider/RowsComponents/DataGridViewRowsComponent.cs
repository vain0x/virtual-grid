using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGrid.RowsComponents;

namespace VirtualGrid.WinFormsDemo.Provider.RowsComponents
{
    public sealed class DataGridViewRowsComponent
    {
        private GridRowsComponent<DataGridViewRowHeaderPart.RowHeaderDeltaListener, AttributeBuilder> _inner;

        public DataGridViewRowsComponent(DataGridViewSpreadLayout layout, DataGridViewGridProvider provider)
        {
            _inner = new GridRowsComponent<DataGridViewRowHeaderPart.RowHeaderDeltaListener, AttributeBuilder>(
                layout.RowHeader._rowHeader,
                layout.ColumnHeader._columnHeader,
                new GridRowElementDataProvider(provider.Body),
                provider.Body.TryGetKey
            );

            provider.SetBody(_inner);
        }

        public Builder GetBuilder()
        {
            return new Builder(_inner.GetBuilder());
        }

        public struct Builder
        {
            private GridRowsComponent<DataGridViewRowHeaderPart.RowHeaderDeltaListener, AttributeBuilder>.Builder _inner;

            public Builder(GridRowsComponent<DataGridViewRowHeaderPart.RowHeaderDeltaListener, AttributeBuilder>.Builder inner)
            {
                _inner = inner;
            }

            public GridRowsElement<AttributeBuilder> AddRowList(object rowKey, Action<object, GridRowElement<AttributeBuilder>> renderFunc)
            {
                return _inner.AddRowList(rowKey, renderFunc);
            }

            public void AddRow(object rowKey, Action<object, GridRowElement<AttributeBuilder>> renderFunc)
            {
                _inner.AddRow(rowKey, renderFunc);
            }

            public void Patch()
            {
                _inner.Patch();
            }
        }
    }
}

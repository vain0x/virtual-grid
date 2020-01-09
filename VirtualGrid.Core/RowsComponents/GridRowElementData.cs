using System;

namespace VirtualGrid.RowsComponents
{
    public struct GridRowElementData<TData>
    {
        public TData Data;

        public Action<object, GridRowElement<TData>> RenderFunc;

        public GridRowElementData(TData data, Action<object, GridRowElement<TData>> renderFunc)
        {
            Data = data;
            RenderFunc = renderFunc;
        }
    }
}

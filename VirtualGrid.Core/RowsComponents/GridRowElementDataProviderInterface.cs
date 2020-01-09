namespace VirtualGrid.RowsComponents
{
    public interface IGridRowElementDataProvider<TData>
    {
        TData Create();

        void Destroy(TData data);

        void Update(TData data);
    }
}

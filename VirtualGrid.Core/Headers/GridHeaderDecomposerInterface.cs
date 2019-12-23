namespace VirtualGrid.Headers
{
    public interface IGridHeaderDecomposer
    {
        GridHeaderDecomposition? Decompose(object elementKey);
    }
}

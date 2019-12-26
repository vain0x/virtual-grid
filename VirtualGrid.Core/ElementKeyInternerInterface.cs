namespace VirtualGrid
{
    public interface IElementKeyInterner
    {
        object TryGetKey(int index);

        int? TryGetIndex(object elementKey);
    }
}

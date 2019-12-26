namespace VirtualGrid.Headers
{
    public struct GridHeaderDelta
    {
        public readonly GridHeaderDeltaKind Kind;

        public readonly int Index;

        public readonly object ElementKey;

        public GridHeaderDelta(GridHeaderDeltaKind kind, int index, object elementKey)
        {
            Kind = kind;
            Index = index;
            ElementKey = elementKey;
        }

        public static GridHeaderDelta NewInsert(int index, object elementKey)
        {
            return new GridHeaderDelta(GridHeaderDeltaKind.Insert, index, elementKey);
        }

        public static GridHeaderDelta NewRemove(int index)
        {
            return new GridHeaderDelta(GridHeaderDeltaKind.Remove, index, null);
        }
    }
}

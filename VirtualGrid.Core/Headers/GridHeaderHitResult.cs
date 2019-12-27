namespace VirtualGrid.Headers
{
    public struct GridHeaderHitResult
    {
        private GridHeaderNode Node;

        public int Index;

        public object ElementKey
        {
            get
            {
                return Node.ElementKey;
            }
        }

        public int Offset
        {
            get
            {
                return Node.Offset;
            }
        }

        private GridHeaderHitResult(GridHeaderNode node, int index)
        {
            Node = node;
            Index = index;
        }

        public static GridHeaderHitResult Create(GridHeaderNode node, int index)
        {
            return new GridHeaderHitResult(node, index);
        }
    }
}

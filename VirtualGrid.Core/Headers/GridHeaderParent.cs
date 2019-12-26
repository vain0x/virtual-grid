namespace VirtualGrid.Headers
{
    public interface IGridHeaderParent
    {
        void OnChildChanged(int countChange);

        void UpdateChildren();
    }

    public struct GridHeaderParent
        : IGridHeaderParent
    {
        private readonly IGridHeaderParent _innerOpt;

        public GridHeaderParent(IGridHeaderParent innerOpt)
        {
            _innerOpt = innerOpt;
        }

        public void OnChildChanged(int countChange)
        {
            if (_innerOpt != null)
            {
                _innerOpt.OnChildChanged(countChange);
            }
        }

        public void UpdateChildren()
        {
            if (_innerOpt != null)
            {
                _innerOpt.UpdateChildren();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    public interface IGridElementResolver<TData>
    {
        GridElementHitResult<TData>? Hit(GridVector index);
    }

    public struct GridElementHitResult<TData>
    {
        public GridElementKey Key;

        public TData Data;

        public GridElementHitResult(GridElementKey key, TData data)
        {
            Key = key;
            Data = data;
        }
    }

    public static class GridElementHitResult
    {
        public static GridElementHitResult<T> Create<T>(GridElementKey key, T data)
        {
            return new GridElementHitResult<T>(key, data);
        }
    }
}

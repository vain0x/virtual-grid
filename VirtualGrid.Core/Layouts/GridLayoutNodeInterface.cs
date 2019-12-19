using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualGrid.Layouts
{
    /// <summary>
    /// グリッドのレイアウトを表すツリーのノード
    /// </summary>
    public interface IGridLayoutNode
    {
        object ElementKey { get; }

        /// <summary>
        /// レイアウトのサイズを測る。
        /// </summary>
        /// <param name="available">
        /// レイアウトのサイズの上限
        /// </param>
        /// <param name="context">
        /// レイアウト計算のデータを管理するもの
        /// </param>
        GridVector Measure(GridMeasure available, GridLayoutContext context);

        /// <summary>
        /// レイアウトの位置を決定する。
        /// </summary>
        /// <param name="range">
        /// このレイアウトに割り当てられた範囲。
        /// </param>
        /// <param name="context">
        /// レイアウト計算のデータを管理するもの
        /// </param>
        void Arrange(GridRange range, GridLayoutContext context);

        /// <summary>
        /// 子要素を列挙する。
        /// </summary>
        /// <param name="action">
        /// 各子要素に対して呼び出される関数
        /// </param>
        void Iterate(Action<IGridLayoutNode> action);
    }

    public static class GridLayoutNode
    {
        public static IGridLayoutNode Empty(object elementKey)
        {
            return new EmptyGridLayoutNode(elementKey);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    /// <summary>
    /// グリッドの部位
    /// </summary>
    public enum GridPart
    {
        /// <summary>
        /// ローヘッダー (左にあるヘッダー)
        /// </summary>
        RowHeader,

        /// <summary>
        /// カラムヘッダー (上にあるヘッダー)
        /// </summary>
        ColumnHeader,

        /// <summary>
        /// ボディー
        /// </summary>
        Body,
    }
}

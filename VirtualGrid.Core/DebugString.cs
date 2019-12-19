using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    /// <summary>
    /// デバッグ用の文字列を作るための機能を提供する。
    /// </summary>
    internal static class DebugString
    {
        /// <summary>
        /// オブジェクトをデバッグ用の文字列に変換する。
        ///
        /// <para>
        /// - string と null はそのまま表示する。
        /// - <c>AsDebug</c> という名前のプロパティを持っていたら、それを表示する。
        /// - コレクションなら件数を表示する。
        /// - いずれでもなければ、<c>ToString</c> の結果を表示する。
        /// </para>
        /// </summary>
        public static string From(object obj)
        {
            if (obj == null)
                return "null";

            var str = obj as string;
            if (str != null)
                return str;

            var asDebugProperty =
                obj.GetType()
                .GetProperty("AsDebug", BindingFlags.Instance | BindingFlags.Public);
            if (asDebugProperty != null)
                return From(asDebugProperty.GetValue(obj));

            var collection = obj as System.Collections.ICollection;
            if (collection != null)
                return "[Count = " + collection.Count + "]";

            return obj.ToString();
        }
    }
}

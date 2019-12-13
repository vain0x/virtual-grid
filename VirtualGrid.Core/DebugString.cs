using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGrid
{
    internal static class DebugString
    {
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

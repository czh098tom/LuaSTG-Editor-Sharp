using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Util
{
    public static class DictionaryExtension
    {
        public static U GetOrDefault<T, U>(this Dictionary<T, U> dict, T inValue, U defaultValue)
        {
            U value = defaultValue;
            if (dict.TryGetValue(inValue, out U val)) value = val;
            return value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Lua
{
    public static class StringParser
    {
        public static string ParseLua(string s)
        {
            if (!s.Contains('\\') && !s.Contains('\'') && !s.Contains('\"')) return s;
            StringBuilder sb = new StringBuilder();
            foreach(char c in s)
            {
                if (c == '\\' || c == '\'' || c == '\"') sb.Append('\\');
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}

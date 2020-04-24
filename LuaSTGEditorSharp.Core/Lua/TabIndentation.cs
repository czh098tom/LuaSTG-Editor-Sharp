using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Lua
{
    public class TabIndentation : IndentationGenerator
    {
        public override string CreateIndentation(int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++) sb.Append("\t");
            return sb.ToString();
        }
    }
}

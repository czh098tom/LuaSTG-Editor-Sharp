using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Lua
{
    public class SpaceIndentation : IndentationGenerator
    {
        public int NumOfSpaces { get; set; } = 4;

        public override string CreateIndentation(int length)
        {
            return "".PadLeft(NumOfSpaces * length);
        }
    }
}

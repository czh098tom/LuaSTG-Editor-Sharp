using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Lua
{
    public static class StaticAnalysis
    {
        public static string ParseBool(string source, string truecase, string falsecase)
        {
            if (source == "true") return truecase;
            if (source == "false") return falsecase;
            return "if (" + source + ") then " + truecase + " else " + falsecase + "end\n";
        }

        public static string BoolHint(string source, string truecase, string falsecase, string unknowncase)
        {
            if (source == "true") return truecase;
            if (source == "false") return falsecase;
            return unknowncase;
        }
    }
}

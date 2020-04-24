using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Exception
{
    public class InvalidRelativeResPathException : ApplicationException
    {
        public string Path { get; }

        public InvalidRelativeResPathException(string s)
        {
            Path = s;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LuaSTGEditorSharp.Lua
{
    public abstract class IndentationGenerator
    {
        private static IndentationGenerator _current;

        public static IndentationGenerator Current
        {
            get
            {
                if (_current == null)
                {
                    if ((Application.Current as IAppSettings).SpaceIndentation)
                    {
                        _current = new SpaceIndentation() { NumOfSpaces = (Application.Current as IAppSettings).IndentationSpaceLength };
                    }
                    else
                    {
                        _current = new TabIndentation();
                    }
                }
                return _current;
            }
            set
            {
                _current = value;
            }
        }

        public abstract string CreateIndentation(int length);
    }
}

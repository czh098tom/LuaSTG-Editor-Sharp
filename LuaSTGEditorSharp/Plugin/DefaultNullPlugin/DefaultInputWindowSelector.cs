using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.Windows.Input;

namespace LuaSTGEditorSharp.Plugin.DefaultNullPlugin
{
    class DefaultInputWindowSelector : IInputWindowSelector
    {
        public string[] SelectComboBox(string name)
        {
            switch (name)
            {
                case "bool":
                    return new string[] { "true", "false" };
                default:
                    return new string[] { };
            }
        }

        public InputWindow SelectInputWindow(AttrItem source, string name, string toEdit, MainWindow owner)
        {
            InputWindow window;
            switch (name)
            {
                case "bool":
                    window = new Selector(toEdit, owner, SelectComboBox(name), "Input Bool");
                    break;
                case "userDefinedNode":
                    window = new NodeDefInput(toEdit, owner, source);
                    break;
                default:
                    window = new SingleLineInput(toEdit, owner);
                    break;
            }
            window.AppendTitle(source.AttrCap);
            return window;
        }
    }
}

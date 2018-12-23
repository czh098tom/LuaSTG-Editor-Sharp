using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.Windows.Input;

namespace LuaSTGEditorSharp.Plugin
{
    public interface IInputWindowSelector
    {
        string[] SelectComboBox(string name);
        InputWindow SelectInputWindow(AttrItem source, string name, string toEdit, MainWindow owner);
    }
}

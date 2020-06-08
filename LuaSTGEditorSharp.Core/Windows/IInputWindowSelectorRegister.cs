using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.Windows.Input;

namespace LuaSTGEditorSharp.Windows
{
    public interface IInputWindowSelectorRegister
    {
        //string[] SelectComboBox(string name);
        void RegisterComboBoxText(Dictionary<string, string[]> target);
        //InputWindow SelectInputWindow(AttrItem source, string name, string toEdit, MainWindow owner);
        void RegisterInputWindow(Dictionary<string, Func<AttrItem, string, IInputWindow>> target);
    }
}

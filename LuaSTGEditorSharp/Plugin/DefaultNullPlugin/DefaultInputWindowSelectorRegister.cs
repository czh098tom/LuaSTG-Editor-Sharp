using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.Windows.Input;

namespace LuaSTGEditorSharp.Plugin.DefaultNullPlugin
{
    class DefaultInputWindowSelectorRegister : IInputWindowSelectorRegister
    {
        public void RegisterComboBoxText(Dictionary<string, string[]> target)
        {
            target.Add("bool", new string[] { "true", "false" });
        }

        public void RegisterInputWindow(Dictionary<string, Func<AttrItem, string, InputWindow>> target)
        {
            target.Add("bool", (source, toEdit) => new Selector(toEdit, InputWindowSelector.SelectComboBox("bool"), "Input Bool"));
            target.Add("code", (source, toEdit) => new CodeInput(toEdit));
            target.Add("userDefinedNode", (source, toEdit) => new NodeDefInput(toEdit, source));
        }
    }
}

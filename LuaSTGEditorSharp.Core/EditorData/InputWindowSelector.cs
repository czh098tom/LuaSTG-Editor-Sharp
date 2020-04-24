using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.Util;
using LuaSTGEditorSharp.Windows.Input;
using LuaSTGEditorSharp.Plugin;

namespace LuaSTGEditorSharp.EditorData
{
    public static class InputWindowSelector
    {
        public static readonly string[] nullSelection = new string[0] { };
        public static readonly Func<AttrItem, string, InputWindow> nullWindow = (e, s) => new SingleLineInput(s);

        private static readonly Dictionary<string, string[]> comboBox = new Dictionary<string, string[]>();
        private static readonly Dictionary<string, Func<AttrItem, string, InputWindow>> windowGenerator
            = new Dictionary<string, Func<AttrItem, string, InputWindow>>();

        public static void Register(IInputWindowSelectorRegister register)
        {
            register.RegisterComboBoxText(comboBox);
            register.RegisterInputWindow(windowGenerator);
        }

        public static void AfterRegister()
        {
            List<string> vs = new List<string>(windowGenerator.Keys);
            vs.Add("");
            vs.Sort();
            comboBox.Add("editWindow", vs.ToArray());
            windowGenerator.Add("editWindow", (src, tar) => new Selector(tar
                 , SelectComboBox("editWindow"), "Input Edit Window"));
        }

        public static string[] SelectComboBox(string name)
        {
            return comboBox.GetOrDefault(name, nullSelection);
        }

        public static InputWindow SelectInputWindow(AttrItem source, string name, string toEdit)
        {
            InputWindow iw = windowGenerator.GetOrDefault(name, nullWindow)(source, toEdit); 
            iw.AppendTitle(source.AttrCap);
            return iw;
        }
    }
}

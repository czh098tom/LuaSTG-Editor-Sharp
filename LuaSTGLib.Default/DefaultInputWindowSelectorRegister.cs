using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.Windows;
using LuaSTGEditorSharp.Windows.Input;

namespace LuaSTGEditorSharp.Plugin.Default
{
    class DefaultInputWindowSelectorRegister : IInputWindowSelectorRegister
    {
        public void RegisterComboBoxText(Dictionary<string, string[]> target)
        {
            target.Add("bool", new string[] { "true", "false" });
            target.Add("sineinterpolation"
                , new string[] { "SINE_ACCEL", "SINE_DECEL", "SINE_ACC_DEC" });
        }

        public void RegisterInputWindow(Dictionary<string, Func<AttrItem, string, IInputWindow>> target)
        {
            target.Add("bool", (source, toEdit) => new Selector(toEdit
                , InputWindowSelector.SelectComboBox("bool"), "Input Bool"));
            target.Add("sineinterpolation", (source, toEdit) => new Selector(toEdit
                , InputWindowSelector.SelectComboBox("sineinterpolation"), "Input Sine Interpolation Mode"));
            target.Add("code", (source, toEdit) => new CodeInput(toEdit));
            target.Add("userDefinedNodeDefinition", (source, toEdit) => new NodeDefInput(toEdit, source));
            target.Add("plainFile", (src, tar) => new PathInput(tar, "File (*.*)|*.*", src));
            target.Add("plainMultipleFiles", (src, tar) => new MultiplePathInput(tar, "File (*.*)|*.*", src));
        }
    }
}

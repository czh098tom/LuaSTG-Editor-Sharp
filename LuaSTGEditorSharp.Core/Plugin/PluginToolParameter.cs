using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Plugin
{
    public sealed class PluginToolParameter
    {
        public readonly EditorData.TreeNodeBase selected;

        public PluginToolParameter(EditorData.TreeNodeBase selected = null)
        {
            this.selected = selected;
        }
    }
}

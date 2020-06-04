using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Plugin
{
    public sealed class PluginToolParameter
    {
        public readonly EditorData.TreeNode selected;

        public PluginToolParameter(EditorData.TreeNode selected = null)
        {
            this.selected = selected;
        }
    }
}

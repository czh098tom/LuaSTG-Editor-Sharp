using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.EditorData;

namespace LuaSTGEditorSharp.Plugin
{
    public abstract class PluginTool
    {
        public abstract string Name { get; }
        public virtual PluginToolResult ExecutePlugin(PluginToolParameter param)
            => new PluginToolResult()
            {
                commands = new List<Command>(),
                clipBoard = null,
                newDocument = null
            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.EditorData;

namespace LuaSTGEditorSharp.Plugin
{
    public struct PluginToolResult
    {
        public List<Command> commands;
        public TreeNodeBase clipBoard;
        public DocumentData newDocument;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.Plugin;

namespace DanmakuRandomizer
{
    public class DanmakuRandomizerPluginTool : PluginTool
    {
        public override string Name => "Danmaku Randomizer";

        public override PluginToolResult ExecutePlugin(PluginToolParameter param)
        {
            var w = new View.DanmakuRandomizer();
            w.ShowDialog();
            return new PluginToolResult()
            {
                clipBoard = w.Nodes,
                commands = new List<LuaSTGEditorSharp.EditorData.Command>(),
                newDocument = null
            };
        }
    }
}

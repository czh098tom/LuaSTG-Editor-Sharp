using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.Lua;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public abstract class ResourceLoadMetaInfo : MetaInfo
    {
        public override sealed string Name
        {
            get => StringParser.ParseLua(target.PreferredNonMacrolize(1, "Resource name"));
        }

        public string Path
        {
            get => target.PreferredNonMacrolize(0, "Path");
        }

        public ResourceLoadMetaInfo(TreeNodeBase target) : base(target) { }
    }
}

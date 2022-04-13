using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.EditorData.Node;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public abstract class MetaInfoWithDifficulty : MetaInfo
    {
        public MetaInfoWithDifficulty(TreeNodeBase target) : base(target) { }

        public override string Name
        {
            get => Lua.StringParser.ParseLua(target.PreferredNonMacrolize(0, nameof(DefinitionWithDifficulty.Name)));
        }

        public override string Difficulty
        {
            get => Lua.StringParser.ParseLua(target.PreferredNonMacrolize(1, nameof(DefinitionWithDifficulty.Difficulty)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Audio
{
    [Serializable, NodeIcon("pausebgm.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class PauseBGM : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public PauseBGM() : base() { }

        public PauseBGM(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "_pause_music()\n";
        }

        public override string ToString()
        {
            return "Pause background music";
        }

        public override object Clone()
        {
            var n = new PauseBGM(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }
    }
}

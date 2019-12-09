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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/pausebgm.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class PauseBGM : TreeNode
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

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

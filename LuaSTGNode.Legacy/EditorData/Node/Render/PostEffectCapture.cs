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

namespace LuaSTGEditorSharp.EditorData.Node.Render
{
    [Serializable, NodeIcon("PostEffectCapture.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    public class PostEffectCapture : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public PostEffectCapture() : base() { }

        public PostEffectCapture(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            yield return Indent(spacing) + "PostEffectCapture()\n";
        }

        public override string ToString()
        {
            return "Begin render texture capturing";
        }

        public override object Clone()
        {
            var n = new PostEffectCapture(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }
    }
}

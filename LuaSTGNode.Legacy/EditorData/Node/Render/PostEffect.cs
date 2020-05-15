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
    [Serializable, NodeIcon("PostEffect.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    [CreateInvoke(1)]
    public class PostEffect : TreeNode
    {
        [JsonConstructor]
        public PostEffect() : base() { }

        public PostEffect(DocumentData workSpaceData) : this(workSpaceData, "\"\"", "", "\"\"", "") { }

        public PostEffect(DocumentData workSpaceData, string tex, string shader, string blend, string param)
            : base(workSpaceData) 
        {
            Texture = tex;
            Shader = shader;
            Blend = blend;
            Parameter = param;
        }

        [JsonIgnore, NodeAttribute]
        public string Texture
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Shader
        {
            get => DoubleCheckAttr(1, "fx").attrInput;
            set => DoubleCheckAttr(1, "fx").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Blend
        {
            get => DoubleCheckAttr(2, "blend", "Blend Mode").attrInput;
            set => DoubleCheckAttr(2, "blend", "Blend Mode").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Parameter
        {
            get => DoubleCheckAttr(3).attrInput;
            set => DoubleCheckAttr(3).attrInput = value;
        }

        public override string ToString()
        {
            string s0 = $"Render {NonMacrolize(0)}";
            if (string.IsNullOrEmpty(NonMacrolize(0)))
            {
                s0 = "Render captured texture";
            }
            return $"{s0} by shader {NonMacrolize(1)} with blend mode {NonMacrolize(2)} "
                + $"and parameter {{{NonMacrolize(3)}}}";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s = Macrolize(0);
            if (string.IsNullOrEmpty(s))
            {
                yield return sp + $"PostEffectApply({Macrolize(1)},{Macrolize(2)},{{{Macrolize(3)}}})\n";
            }
            else
            {
                yield return sp + $"PostEffect({s},{Macrolize(1)},{Macrolize(2)},{{{Macrolize(3)}}})\n";
            }
        }

        public override object Clone()
        {
            var n = new PostEffect(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

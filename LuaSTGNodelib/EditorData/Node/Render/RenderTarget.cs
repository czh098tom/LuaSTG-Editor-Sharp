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
    [Serializable, NodeIcon("RenderTarget.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    [RCInvoke(1)]
    public class RenderTarget : TreeNode
    {
        [JsonConstructor]
        public RenderTarget() : base() { }

        public RenderTarget(DocumentData workSpaceData) : this(workSpaceData, "Push", "\"\"") { }

        public RenderTarget(DocumentData workSpaceData, string operation, string name) : base(workSpaceData) 
        {
            Operation = operation;
            Name = name;
        }

        [JsonIgnore, NodeAttribute("Push")]
        public string Operation
        {
            get => DoubleCheckAttr(0, "renderOp").attrInput;
            set => DoubleCheckAttr(0, "renderOp").attrInput = value;
        }

        [JsonIgnore, NodeAttribute("\"\"")]
        public string Name
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        public override string ToString()
        {
            return $"{NonMacrolize(0)} render target {NonMacrolize(1)}";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"{NonMacrolize(0)}RenderTarget({Macrolize(1)})\n";
        }

        public override object Clone()
        {
            var n = new RenderTarget(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

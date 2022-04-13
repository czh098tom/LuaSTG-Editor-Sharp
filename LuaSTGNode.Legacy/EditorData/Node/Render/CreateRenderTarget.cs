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
    [Serializable, NodeIcon("CreateRenderTarget.png")]
    [CreateInvoke(0), RCInvoke(0)]
    public class CreateRenderTarget : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public CreateRenderTarget() : base() { }

        public CreateRenderTarget(DocumentData workSpaceData) : this(workSpaceData, "\"\"") { }

        public CreateRenderTarget(DocumentData workSpaceData, string name) : base(workSpaceData) 
        {
            Name = name;
        }

        [JsonIgnore, NodeAttribute("\"\"")]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override string ToString()
        {
            return $"Create render target {NonMacrolize(0)}";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"CreateRenderTarget({Macrolize(0)})\n";
        }

        public override object Clone()
        {
            var n = new CreateRenderTarget(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }
    }
}

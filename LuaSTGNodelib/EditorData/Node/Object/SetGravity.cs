using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("setgravity.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(1)]
    public class SetGravity : TreeNode
    {
        [JsonConstructor]
        private SetGravity() : base() { }

        public SetGravity(DocumentData workSpaceData)
            : this(workSpaceData, "self", "0.05")
        { }

        public SetGravity(DocumentData workSpaceData, string target, string g)
            : base(workSpaceData)
        {
            Target = target;
            Gravity = g;
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Gravity
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "_set_g(" + Macrolize(0) + "," + Macrolize(1) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set gravity of \"" + NonMacrolize(0) + "\" to " + NonMacrolize(1);
        }

        public override object Clone()
        {
            var n = new SetGravity(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

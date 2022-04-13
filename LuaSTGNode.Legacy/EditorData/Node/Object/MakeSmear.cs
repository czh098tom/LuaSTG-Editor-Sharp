using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("smear.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(1)]
    class MakeSmear : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private MakeSmear() : base() { }

        public MakeSmear(DocumentData workSpaceData)
            : this(workSpaceData, "self", "1") { }

        public MakeSmear(DocumentData workSpaceData, string target, string interval)
            : base(workSpaceData)
        {
            Target = target;
            Interval = interval;
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Interval
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        public override string ToString()
        {
            return $"Make smear to {NonMacrolize(0)} with an interval of {NonMacrolize(1)}";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return $"{sp}last=New(smear,{Macrolize(0)},{Macrolize(1)})";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override object Clone()
        {
            var n = new MakeSmear(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
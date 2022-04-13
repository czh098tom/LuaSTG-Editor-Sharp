using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("objectsetimg.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(1), RCInvoke(1)]
    class SetObjectImage : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private SetObjectImage() : base() { }

        public SetObjectImage(DocumentData workSpaceData)
            : this(workSpaceData, "self", "white") { }

        public SetObjectImage(DocumentData workSpaceData, string target, string image)
            : base(workSpaceData)
        {
            Target = target;
            Image = image;
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(1, "image").attrInput;
            set => DoubleCheckAttr(1, "image").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return $"{sp}{Macrolize(0)}.img={Macrolize(1)}";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return $"Set {NonMacrolize(0)}'s image to {NonMacrolize(1)}";
        }

        public override object Clone()
        {
            var n = new SetObjectImage(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

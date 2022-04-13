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
    [Serializable, NodeIcon("setaccel.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(1)]
    public class SetAccel : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private SetAccel() : base() { }

        public SetAccel(DocumentData workSpaceData)
            : this(workSpaceData, "self", "0.05", "0", "false") { }

        public SetAccel(DocumentData workSpaceData, string tar, string v, string r, string aim)
            : base(workSpaceData)
        {
            Target = tar;
            Acceleration = v;
            Angle = r;
            AimToPlayer = aim;
            /*
            attributes.Add(new AttrItem("Target", tar, this, "target"));
            attributes.Add(new AttrItem("Acceleration", v, this));
            attributes.Add(new AttrItem("Rotation", r, this));
            attributes.Add(new AttrItem("Aim to Player", aim, this, "bool"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Acceleration
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Angle
        {
            get => DoubleCheckAttr(2).attrInput;
            set => DoubleCheckAttr(2).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string AimToPlayer
        {
            get => DoubleCheckAttr(3, "bool", "Aim to Player").attrInput;
            set => DoubleCheckAttr(3, "bool", "Aim to Player").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "_set_a(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + "," 
                + Macrolize(3) + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "Set acceleration of " + NonMacrolize(0) + " : v=" + NonMacrolize(1) + " angle=" + NonMacrolize(2)
                + (NonMacrolize(3) == "true" ? " , aim to player" : "");
        }

        public override object Clone()
        {
            var n = new SetAccel(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

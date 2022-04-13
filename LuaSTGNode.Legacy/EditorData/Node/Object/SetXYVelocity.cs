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
    [Serializable, NodeIcon("setvxy.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(1)]
    public class SetXYVelocity : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private SetXYVelocity() : base() { }

        public SetXYVelocity(DocumentData workSpaceData)
            : this(workSpaceData, "self", "3,0", "true") { }

        public SetXYVelocity(DocumentData workSpaceData, string tar, string v, string setR)
            : base(workSpaceData)
        {
            Target = tar;
            Velocity = v;
            SetRotation = setR;
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Velocity
        {
            get => DoubleCheckAttr(1, "velocity").attrInput;
            set => DoubleCheckAttr(1, "velocity").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string SetRotation
        {
            get => DoubleCheckAttr(2, "bool", "Set Rotation").attrInput;
            set => DoubleCheckAttr(2, "bool", "Set Rotation").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + Macrolize(0) + ".vx," + Macrolize(0) + ".vy=" + Macrolize(1) + "\n";
            if (SetRotation == "true")
            {
                yield return sp + Macrolize(0) + ".rot=atan2(" + Macrolize(0) + ".vy," + Macrolize(0) + ".vx)\n";
            }
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            if (SetRotation == "true")
            {
                yield return new Tuple<int, TreeNodeBase>(2, this);
            }
            else
            {
                yield return new Tuple<int, TreeNodeBase>(1, this);
            }
        }

        public override string ToString()
        {
            return "Set velocity of " + NonMacrolize(0) + " : vx, vy=" + NonMacrolize(1)
                + (NonMacrolize(2) == "true" ? " , also set rotation" : "");
        }

        public override object Clone()
        {
            var n = new SetXYVelocity(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

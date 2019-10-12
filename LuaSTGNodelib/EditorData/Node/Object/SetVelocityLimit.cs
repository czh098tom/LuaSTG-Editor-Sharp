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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/setfv.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(1)]
    public class SetVelocityLimit : TreeNode
    {
        [JsonConstructor]
        private SetVelocityLimit() : base() { }

        public SetVelocityLimit(DocumentData workSpaceData)
            : this(workSpaceData, "self", "", "", "")
        { }

        public SetVelocityLimit(DocumentData workSpaceData, string target, string maxv, string maxvx, string maxvy)
            : base(workSpaceData)
        {
            Target = target;
            MaxV = maxv;
            MaxVX = maxvx;
            MaxVY = maxvy;
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string MaxV
        {
            get => DoubleCheckAttr(1, name:"Max velocity").attrInput;
            set => DoubleCheckAttr(1, name:"Max velocity").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string MaxVX
        {
            get => DoubleCheckAttr(2, name: "Max X-velocity").attrInput;
            set => DoubleCheckAttr(2, name: "Max X-velocity").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string MaxVY
        {
            get => DoubleCheckAttr(3, name: "Max Y-velocity").attrInput;
            set => DoubleCheckAttr(3, name: "Max Y-velocity").attrInput = value;
        }

        public static string NullToDefault(string s, string def) => string.IsNullOrEmpty(s) ? def : s;

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            string def = "\'original\'";
            yield return sp + "_forbid_v(" + Macrolize(0) + "," + NullToDefault(Macrolize(1),def) 
                + "," + NullToDefault(Macrolize(2), def) + "," + NullToDefault(Macrolize(3), def) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            List<string> strs = new List<string>();
            if (!string.IsNullOrEmpty(NonMacrolize(1))) strs.Add("max velocity to " + NonMacrolize(1));
            if (!string.IsNullOrEmpty(NonMacrolize(2))) strs.Add("max x-velocity to " + NonMacrolize(2));
            if (!string.IsNullOrEmpty(NonMacrolize(3))) strs.Add("max y-velocity to " + NonMacrolize(3));
            return "Set \"" + NonMacrolize(0) + "\"'s " + string.Join(", ", strs);
        }

        public override object Clone()
        {
            var n = new SetVelocityLimit(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

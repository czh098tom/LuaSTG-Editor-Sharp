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

namespace LuaSTGEditorSharp.EditorData.Node.Data
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/assignment.png")]
    [LeafNode]
    [CreateInvoke(1), RCInvoke(1)]
    public class PositionAssignment : TreeNode
    {
        [JsonConstructor]
        public PositionAssignment() : base() { }

        public PositionAssignment(DocumentData workSpaceData) : this(workSpaceData, "self", "0,0") { }

        public PositionAssignment(DocumentData workSpaceData, string target, string pos) : base(workSpaceData)
        {
            Target = target;
            Position = pos;
        }

        [JsonIgnore]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore]
        public string Position
        {
            get => DoubleCheckAttr(1, "position").attrInput;
            set => DoubleCheckAttr(1, "position").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "(" + Macrolize(0) + ").x,(" + Macrolize(0) + ").y=" + Macrolize(1) + "\n";
        }

        public override string ToString()
        {
            return "Set position of \"" + NonMacrolize(0) + "\" to (" + NonMacrolize(1) + ")";
        }

        public override object Clone()
        {
            var n = new PositionAssignment(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("bossmoveto.png")]
    [RequireParent(typeof(BossDefine))]
    [LeafNode]
    public class BossMoveTo : TreeNode
    {
        [JsonConstructor]
        public BossMoveTo() : base() { }

        public BossMoveTo(DocumentData workSpaceData)
            : this(workSpaceData, "self.x,self.y", "60", "MOVE_NORMAL") { }

        public BossMoveTo(DocumentData workSpaceData, string destination, string time, string mode) : base(workSpaceData)
        {
            Destination = destination;
            Time = time;
            Mode = mode;
        }

        [JsonIgnore, NodeAttribute]
        public string Destination
        {
            get => DoubleCheckAttr(0, "position", "Position").attrInput;
            set => DoubleCheckAttr(0, "position", "Position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Mode
        {
            get => DoubleCheckAttr(2, "interpolation", "Mode").attrInput;
            set => DoubleCheckAttr(2, "interpolation", "Mode").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            if (Parent?.attributes != null && Parent.AttributeCount >= 2)
            {
                parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0) +
                    (Parent.NonMacrolize(1) == "All" ? "" : ":" + Parent.NonMacrolize(1)));
            }

            string fr = Macrolize(1);
            fr = string.IsNullOrEmpty(fr) ? "1" : fr;
            string mode = Macrolize(2);
            mode = string.IsNullOrEmpty(mode) ? "MOVE_NORMAL" : mode;
            yield return sp + "table.insert(_editor_class[\"" + parentName + "\"].cards,boss.move.New(" 
                + Macrolize(0) + "," + fr + "," + mode + "))\n";
        }

        public override string ToString()
        {
            string fr = NonMacrolize(1);
            fr = string.IsNullOrEmpty(fr) ? "1" : fr;
            string mode = NonMacrolize(2);
            mode = string.IsNullOrEmpty(mode) || mode == "MOVE_NORMAL" ? "" : ", interpolation mode: " + mode;
            return "Move to (" + NonMacrolize(0) + ") in " + fr + " frame(s)" + mode;
        }

        public override object Clone()
        {
            var n = new BossMoveTo(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            TreeNode p = GetLogicalParent();
            if (p?.attributes == null || p.AttributeCount < 2)
            {
                a.Add(new CannotFindAttributeInParent(2, this));
            }
            return a;
        }
    }
}
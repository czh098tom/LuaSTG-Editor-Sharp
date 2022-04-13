using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Laser
{
    [Serializable, NodeIcon("laserturnoff.png")]
    [RequireAncestor(typeof(LaserAlikeTypes), typeof(Laser.BentLaserInit))]
    [RequireAncestor(typeof(TaskAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(2)]
    public class LaserTurnOff : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private LaserTurnOff() : base() { }

        public LaserTurnOff(DocumentData workSpaceData)
            : this(workSpaceData, "30", "true")
        { }

        public LaserTurnOff(DocumentData workSpaceData, string time, string wait)
            : base(workSpaceData)
        {
            Target = "self";
            Time = time;
            WaitInThisTask = wait;
            /*
            attributes.Add(new AttrItem("Target", "self", this, "target"));
            attributes.Add(new AttrItem("Time", time, this, "time"));
            attributes.Add(new AttrItem("Wait in this Task", wait, this, "bool"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(1, "time").attrInput;
            set => DoubleCheckAttr(1, "time").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string WaitInThisTask
        {
            get => DoubleCheckAttr(2, "bool", "Wait in this Task").attrInput;
            set => DoubleCheckAttr(2, "bool", "Wait in this Task").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "laser._TurnOff(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "\"" + NonMacrolize(0) + "\" turn off in " + NonMacrolize(1) + " frame(s)"
                + (NonMacrolize(2) == "true" ? ", wait" : "");
        }

        public override object Clone()
        {
            var n = new LaserTurnOff(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

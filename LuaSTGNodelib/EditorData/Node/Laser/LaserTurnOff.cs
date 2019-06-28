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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/laserturnoff.png")]
    [RequireAncestor(typeof(Object.CallBackFunc), typeof(Laser.LaserInit), typeof(Laser.BentLaserInit), typeof(Data.Function))]
    [RequireAncestor(typeof(Task.TaskNode), typeof(Data.Function), typeof(Task.Tasker))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(2)]
    public class LaserTurnOff : TreeNode
    {
        [JsonConstructor]
        private LaserTurnOff() : base() { }

        public LaserTurnOff(DocumentData workSpaceData)
            : this(workSpaceData, "30", "true")
        { }

        public LaserTurnOff(DocumentData workSpaceData, string time, string wait)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Target", "self", this, "target"));
            attributes.Add(new AttrItem("Time", time, this, "time"));
            attributes.Add(new AttrItem("Wait in this Task", wait, this, "bool"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "laser._TurnOff(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "\"" + NonMacrolize(0) + "\"turn off in " + NonMacrolize(1) + " frame(s)"
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

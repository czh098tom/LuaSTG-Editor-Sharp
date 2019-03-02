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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/laserturnhalfon.png")]
    [RequireAncestor(typeof(Object.CallBackFunc), typeof(Laser.LaserInit), typeof(Laser.BentLaserInit))]
    [RequireAncestor(typeof(Task.TaskNode))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(2)]
    public class LaserTurnHalfOn : TreeNode
    {
        [JsonConstructor]
        private LaserTurnHalfOn() : base() { }

        public LaserTurnHalfOn(DocumentData workSpaceData)
            : this(workSpaceData, "30", "true")
        { }

        public LaserTurnHalfOn(DocumentData workSpaceData, string time, string wait)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Target", "self", this, "target"));
            attributes.Add(new AttrItem("Time", time, this, "time"));
            attributes.Add(new AttrItem("Wait in this Task", wait, this, "bool"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "laser._TurnHalfOn(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "\"" + NonMacrolize(0) + "\"turn half on in " + NonMacrolize(1) + " frame(s)"
                + (NonMacrolize(2) == "true" ? ", wait" : "");
        }

        public override object Clone()
        {
            var n = new LaserTurnHalfOn(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

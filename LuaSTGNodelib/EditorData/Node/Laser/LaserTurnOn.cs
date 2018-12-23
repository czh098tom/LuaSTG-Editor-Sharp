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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/laserturnon.png")]
    [RequireAncestor(typeof(Object.CallBackFunc), typeof(Laser.LaserInit), typeof(Laser.BentLaserInit))]
    [RequireAncestor(typeof(Task.TaskNode))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(2)]
    public class LaserTurnOn : TreeNode
    {
        [JsonConstructor]
        private LaserTurnOn() : base() { }

        public LaserTurnOn(DocumentData workSpaceData)
            : this(workSpaceData, "30", "true", "true")
        { }

        public LaserTurnOn(DocumentData workSpaceData, string time, string se, string wait)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Target", "self", this, "target"));
            attributes.Add(new AttrItem("Time", time, this, "time"));
            attributes.Add(new AttrItem("Play Sound Effect", se, this, "bool"));
            attributes.Add(new AttrItem("Wait in this Task", wait, this, "bool"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "laser._TurnOn(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2)
                + "," + Macrolize(3) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "\"" + NonMacrolize(0) + "\"turn on in " + NonMacrolize(1) + " frame(s)"
                + (NonMacrolize(3) == "true" ? ", wait" : "")
                + (NonMacrolize(2) == "true" ? ", play sound effect" : "");
        }

        public override object Clone()
        {
            var n = new LaserTurnOn(parentWorkSpace)
            {
                attributes = new ObservableCollection<AttrItem>(from AttrItem a in attributes select (AttrItem)a.Clone()),
                Children = new ObservableCollection<TreeNode>(from TreeNode t in Children select (TreeNode)t.Clone()),
                _parent = _parent,
                isExpanded = isExpanded
            };
            n.FixAttrParent();
            n.FixChildrenParent();
            return n;
        }
    }
}

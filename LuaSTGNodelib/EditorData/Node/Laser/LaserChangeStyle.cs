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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/laserchangestyle.png")]
    [RequireAncestor(typeof(Object.CallBackFunc), typeof(Laser.LaserInit), typeof(Data.Function))]
    [RequireAncestor(typeof(Task.TaskNode), typeof(Data.Function), typeof(Task.Tasker))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(2)]
    public class LaserChangeStyle : TreeNode
    {
        [JsonConstructor]
        private LaserChangeStyle() : base() { }

        public LaserChangeStyle(DocumentData workSpaceData)
            : this(workSpaceData, "original", "1")
        { }

        public LaserChangeStyle(DocumentData workSpaceData, string color, string style)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Target", "self", this, "target"));
            attributes.Add(new AttrItem("Color", color, this, "nullableColor"));
            attributes.Add(new AttrItem("Style", style, this, "laserStyle"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            string target;
            if (NonMacrolize(1) == "original")
            {
                target = "";
            }
            else
            {
                target = "," + Macrolize(1);
            }
            yield return sp + "laser.ChangeImage(" + Macrolize(0) + "," + Macrolize(2) + target + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Change color and style of \"" + NonMacrolize(0) + "\" to " + NonMacrolize(1) + ", " + NonMacrolize(2);
        }

        public override object Clone()
        {
            var n = new LaserChangeStyle(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

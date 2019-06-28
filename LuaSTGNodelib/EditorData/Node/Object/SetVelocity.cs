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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/setv.png")]
    [RequireAncestor(typeof(Stage.Stage), typeof(Object.CallBackFunc), typeof(Bullet.BulletInit), typeof(Boss.BossBGLayerInit)
        , typeof(Boss.BossBGLayerFrame), typeof(Boss.BossBGLayerRender), typeof(Boss.BossSCStart), typeof(Boss.BossSCFinish)
        , typeof(Laser.LaserInit), typeof(Laser.BentLaserInit), typeof(Data.Function), typeof(Object.ObjectDefine))]
    [LeafNode]
    [RCInvoke(1)]
    public class SetVelocity : TreeNode
    {
        [JsonConstructor]
        private SetVelocity() : base() { }

        public SetVelocity(DocumentData workSpaceData)
            : this(workSpaceData, "self", "3", "0", "false", "true") { }

        public SetVelocity(DocumentData workSpaceData, string tar, string v, string r, string aim, string setR)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Target", tar, this, "target"));
            attributes.Add(new AttrItem("Velocity", v, this, "velocity"));
            attributes.Add(new AttrItem("Rotation", r, this, "rotation"));
            attributes.Add(new AttrItem("Aim to Player", aim, this, "bool"));
            attributes.Add(new AttrItem("Set Rotation", setR, this, "bool"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "SetV2(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + "," 
                + Macrolize(4) + "," + Macrolize(3) + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set velocity of " + NonMacrolize(0) + " : v=" + NonMacrolize(1) + " angle=" + NonMacrolize(2)
                + (NonMacrolize(3) == "true" ? " , aim to player" : "");
        }

        public override object Clone()
        {
            var n = new SetVelocity(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

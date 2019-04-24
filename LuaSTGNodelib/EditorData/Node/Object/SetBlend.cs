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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/setcolor.png")]
    [RequireAncestor(typeof(Stage.Stage), typeof(Object.CallBackFunc), typeof(Bullet.BulletInit), typeof(Boss.BossBGLayerInit)
        , typeof(Boss.BossBGLayerFrame), typeof(Boss.BossBGLayerRender), typeof(Boss.BossSCStart), typeof(Boss.BossSCFinish)
        , typeof(Laser.LaserInit), typeof(Laser.BentLaserInit), typeof(Data.Function))]
    [LeafNode]
    [RCInvoke(2)]
    public class SetBlend : TreeNode
    {
        [JsonConstructor]
        private SetBlend() : base() { }

        public SetBlend(DocumentData workSpaceData)
            : this(workSpaceData, "self", "\"\"", "255,255,255,255") { }

        public SetBlend(DocumentData workSpaceData, string tar, string blend, string color)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Target", tar, this, "target"));
            attributes.Add(new AttrItem("Blend Mode", blend, this, "blend"));
            attributes.Add(new AttrItem("ARGB", color, this, "ARGB"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "_object.set_color(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Set color of " + NonMacrolize(0) + " to(" + NonMacrolize(2) + "), blend mode to " 
                + NonMacrolize(1);
        }

        public override object Clone()
        {
            var n = new SetBlend(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

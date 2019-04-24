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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/unitkill.png")]
    [RequireAncestor(typeof(Stage.Stage), typeof(Object.CallBackFunc), typeof(Bullet.BulletInit), typeof(Boss.BossBGLayerInit)
        , typeof(Boss.BossBGLayerFrame), typeof(Boss.BossBGLayerRender), typeof(Boss.BossSCStart), typeof(Boss.BossSCFinish)
        , typeof(Laser.LaserInit), typeof(Laser.BentLaserInit), typeof(Data.Function))]
    [LeafNode]
    [RCInvoke(0)]
    public class Kill : TreeNode
    {
        [JsonConstructor]
        private Kill() : base() { }

        public Kill(DocumentData workSpaceData)
            : this(workSpaceData, "self", "true") { }

        public Kill(DocumentData workSpaceData, string tar, string trigger)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Target", tar, this, "target"));
            attributes.Add(new AttrItem("Trigger event", trigger, this, "bool"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "_kill(" + Macrolize(0) + "," + Macrolize(1) + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Kill " + NonMacrolize(0);
        }

        public override object Clone()
        {
            var n = new Kill(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

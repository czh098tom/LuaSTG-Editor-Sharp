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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/unitdel.png")]
    [RequireAncestor(typeof(Stage.Stage), typeof(Object.CallBackFunc), typeof(Bullet.BulletInit), typeof(Boss.BossBGLayerInit)
        , typeof(Boss.BossBGLayerFrame), typeof(Boss.BossBGLayerRender), typeof(Boss.BossSCStart), typeof(Boss.BossSCFinish)
        , typeof(Laser.LaserInit), typeof(Laser.BentLaserInit))]
    [LeafNode]
    [RCInvoke(0)]
    public class Del : TreeNode
    {
        [JsonConstructor]
        private Del() : base() { }

        public Del(DocumentData workSpaceData)
            : this(workSpaceData, "self", "true") { }

        public Del(DocumentData workSpaceData, string tar, string trigger)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Target", tar, this, "target"));
            attributes.Add(new AttrItem("Trigger event", trigger, this, "bool"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "_del(" + Macrolize(0) + "," + Macrolize(1) + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Delete " + NonMacrolize(0);
        }

        public override object Clone()
        {
            var n = new Del(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

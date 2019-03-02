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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/laserbentcreate.png")]
    [RequireAncestor(typeof(Stage.Stage), typeof(Object.CallBackFunc), typeof(Bullet.BulletInit), typeof(Boss.BossBGLayerInit)
        , typeof(Boss.BossBGLayerFrame), typeof(Boss.BossBGLayerRender), typeof(Boss.BossSCStart), typeof(Boss.BossSCFinish)
        , typeof(Laser.LaserInit), typeof(Laser.BentLaserInit))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(2)]
    public class CreateBentLaser : TreeNode
    {
        [JsonConstructor]
        private CreateBentLaser() : base() { }

        public CreateBentLaser(DocumentData workSpaceData)
            : this(workSpaceData, "", "self.x,self.y", "")
        { }

        public CreateBentLaser(DocumentData workSpaceData, string name, string pos, string param)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Name", name, this, "bentLaserDef"));
            attributes.Add(new AttrItem("Position", pos, this, "position"));
            attributes.Add(new AttrItem("Parameters", param, this, "bentLaserParam"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            string p = Macrolize(2);
            if (string.IsNullOrEmpty(p)) p = "_";
            yield return sp + "last=New(_editor_class[" + Macrolize(0) + "]," + Macrolize(1) + "," + p + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Create bent laser of type " + NonMacrolize(0) + " at (" + NonMacrolize(1) + ") with parameter " + NonMacrolize(2);
        }

        public override object Clone()
        {
            var n = new CreateBentLaser(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

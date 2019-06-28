using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Bullet
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bulletcreatestraight.png")]
    [RequireAncestor(typeof(Stage.Stage), typeof(Object.CallBackFunc), typeof(Bullet.BulletInit), typeof(Boss.BossBGLayerInit)
        , typeof(Boss.BossBGLayerFrame), typeof(Boss.BossBGLayerRender), typeof(Boss.BossSCStart), typeof(Boss.BossSCFinish)
        , typeof(Laser.LaserInit), typeof(Laser.BentLaserInit), typeof(Data.Function), typeof(Object.ObjectDefine))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(3)]
    public class CreateSimpleBullet : TreeNode
    {
        [JsonConstructor]
        private CreateSimpleBullet() : base() { }

        public CreateSimpleBullet(DocumentData workSpaceData)
            : this(workSpaceData, "arrow_big", "COLOR_RED", "self.x,self.y", "3", "0", "false"
                  , "0", "true", "true", "0", "false", "0", "0", "0", "false")
        { }

        public CreateSimpleBullet(DocumentData workSpaceData, string style, string color, string pos, string v, string r
            , string aim, string rotv, string stay, string destroyable, string time, string rebound, string a, string arot
            , string maxv, string shuttle)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Style", style, this, "bulletStyle"));
            attributes.Add(new AttrItem("Color", color, this, "color"));
            attributes.Add(new AttrItem("Position", pos, this, "position"));
            attributes.Add(new AttrItem("Velocity", v, this, "velocity"));
            attributes.Add(new AttrItem("Angle", r, this, "rotation"));
            attributes.Add(new AttrItem("Aim to player", aim, this, "bool"));
            attributes.Add(new AttrItem("Rotation velocity", rotv, this));
            attributes.Add(new AttrItem("Stay on create", stay, this, "bool"));
            attributes.Add(new AttrItem("Destroyable", destroyable, this, "bool"));
            attributes.Add(new AttrItem("Time", time, this));
            attributes.Add(new AttrItem("Rebound", rebound, this, "bool"));
            attributes.Add(new AttrItem("Acceleration", a, this));
            attributes.Add(new AttrItem("Accel Angle", arot, this, "rotation"));
            attributes.Add(new AttrItem("Max Velocity", maxv, this, "velocity"));
            attributes.Add(new AttrItem("Shuttle", shuttle, this, "bool"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            string s = "";
            foreach(AttrItem at in attributes)
            {
                s += "," + Macrolize(at);
            }
            yield return sp + "last=New(_straight" + s + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Create simple bullet \"" + attributes[0].AttrInput + "\" in \"" + attributes[1].AttrInput
                + "\" at (" + attributes[2].AttrInput + "), v= " + attributes[3].AttrInput + " ,angle= "
                + attributes[4].AttrInput + (attributes[5].AttrInput == "true" ? ", aim to player" : "")
                + (attributes[8].AttrInput == "true" ? ", destroyable" : "")
                + (string.IsNullOrEmpty(attributes[9].AttrInput) ? ", wait " + attributes[9].AttrInput + "frame(s)" : "")
                + (attributes[10].AttrInput == "true" ? ", rebound" : "") + ", a= " + attributes[11].AttrInput
                + " , accelrot= " + attributes[12].AttrInput + (attributes[14].AttrInput == "true" ? ", shuttle" : "");
        }

        public override object Clone()
        {
            var n = new CreateSimpleBullet(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

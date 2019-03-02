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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bulletcreatestraightex.png")]
    [RequireAncestor(typeof(Stage.Stage), typeof(Object.CallBackFunc), typeof(Bullet.BulletInit), typeof(Boss.BossBGLayerInit)
        , typeof(Boss.BossBGLayerFrame), typeof(Boss.BossBGLayerRender), typeof(Boss.BossSCStart), typeof(Boss.BossSCFinish)
        , typeof(Laser.LaserInit), typeof(Laser.BentLaserInit))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(3)]
    public class CreateBulletGroup : TreeNode
    {
        [JsonConstructor]
        private CreateBulletGroup() : base() { }

        public CreateBulletGroup(DocumentData workSpaceData)
            : this(workSpaceData, "arrow_big", "COLOR_RED", "self.x,self.y", "5", "0", "3", "4"
                  , "0", "360", "false", "0", "true", "true", "0", "false")
        { }

        public CreateBulletGroup(DocumentData workSpaceData, string style, string color, string pos, string num, string intv
            , string vb, string ve, string r, string rs, string aim, string rotv, string stay, string destroyable, string time, string rebound)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Style", style, this, "bulletStyle"));
            attributes.Add(new AttrItem("Color", color, this, "color"));
            attributes.Add(new AttrItem("Position", pos, this, "position"));
            attributes.Add(new AttrItem("Number", num, this));
            attributes.Add(new AttrItem("Interval", intv, this));
            attributes.Add(new AttrItem("Velocity begin", vb, this, "velocity"));
            attributes.Add(new AttrItem("Velocity end", ve, this, "velocity"));
            attributes.Add(new AttrItem("Angle", r, this, "rotation"));
            attributes.Add(new AttrItem("Angle spread", rs, this));
            attributes.Add(new AttrItem("Aim to player", aim, this, "bool"));
            attributes.Add(new AttrItem("Rotation velocity", rotv, this, "omega"));
            attributes.Add(new AttrItem("Stay on create", stay, this, "bool"));
            attributes.Add(new AttrItem("Destroyable", destroyable, this, "bool"));
            attributes.Add(new AttrItem("Time", time, this));
            attributes.Add(new AttrItem("Rebound", rebound, this, "bool"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            string s = "";
            foreach(AttrItem at in attributes)
            {
                s += Macrolize(at) + ",";
            }
            yield return sp + "_create_bullet_group(" + s + "self)\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Create "+ attributes[3].AttrInput + " simple bullets \"" + attributes[0].AttrInput + "\" in \"" 
                + attributes[1].AttrInput + "\" at (" + attributes[2].AttrInput + ") in " + attributes[4].AttrInput 
                + " frame(s), v= " + attributes[5].AttrInput + "~" + attributes[6].AttrInput + " ,angle= "
                + attributes[7].AttrInput + " ,spread= " + attributes[8].AttrInput 
                + (attributes[9].AttrInput == "true" ? ", aim to player" : "")
                + (attributes[12].AttrInput == "true" ? ", destroyable" : "")
                + (string.IsNullOrEmpty(attributes[13].AttrInput) ? ", wait " + attributes[9].AttrInput + "frame(s)" : "")
                + (attributes[14].AttrInput == "true" ? ", rebound" : "");
        }

        public override object Clone()
        {
            var n = new CreateBulletGroup(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

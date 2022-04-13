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
    [Serializable, NodeIcon("bulletcreatestraight.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(3)]
    public class CreateSimpleBullet : FixedAttributeTreeNode
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
            Style = style;
            Color = color;
            Position = pos;
            Velocity = v;
            Angle = r;
            AimToPlayer = aim;
            RotationVelocity = rotv;
            StayOnCreate = stay;
            Destroyable = destroyable;
            Time = time;
            Rebound = rebound;
            Acceleration = a;
            AccelAngle = arot;
            MaxVelocity = maxv;
            Shuttle = shuttle;
            /*
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
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Style
        {
            get => DoubleCheckAttr(0, "bulletStyle").attrInput;
            set => DoubleCheckAttr(0, "bulletStyle").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Color
        {
            get => DoubleCheckAttr(1, "color").attrInput;
            set => DoubleCheckAttr(1, "color").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(2, "position").attrInput;
            set => DoubleCheckAttr(2, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Velocity
        {
            get => DoubleCheckAttr(3, "velocity").attrInput;
            set => DoubleCheckAttr(3, "velocity").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Angle
        {
            get => DoubleCheckAttr(4, "rotation").attrInput;
            set => DoubleCheckAttr(4, "rotation").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string AimToPlayer
        {
            get => DoubleCheckAttr(5, "bool", "Aim to player").attrInput;
            set => DoubleCheckAttr(5, "bool", "Aim to player").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string RotationVelocity
        {
            get => DoubleCheckAttr(6, name: "Rotation velocity").attrInput;
            set => DoubleCheckAttr(6, name: "Rotation velocity").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string StayOnCreate
        {
            get => DoubleCheckAttr(7, "bool", "Stay on create").attrInput;
            set => DoubleCheckAttr(7, "bool", "Stay on create").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Destroyable
        {
            get => DoubleCheckAttr(8, "bool").attrInput;
            set => DoubleCheckAttr(8, "bool").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(9).attrInput;
            set => DoubleCheckAttr(9).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Rebound
        {
            get => DoubleCheckAttr(10, "bool").attrInput;
            set => DoubleCheckAttr(10, "bool").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Acceleration
        {
            get => DoubleCheckAttr(11).attrInput;
            set => DoubleCheckAttr(11).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string AccelAngle
        {
            get => DoubleCheckAttr(12, "rotation", "Accel Angle").attrInput;
            set => DoubleCheckAttr(12, "rotation", "Accel Angle").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string MaxVelocity
        {
            get => DoubleCheckAttr(13, "velocity", "Max Velocity").attrInput;
            set => DoubleCheckAttr(13, "velocity", "Max Velocity").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Shuttle
        {
            get => DoubleCheckAttr(14, "bool").attrInput;
            set => DoubleCheckAttr(14, "bool").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s = "";
            foreach(AttrItem at in attributes)
            {
                s += "," + Macrolize(at);
            }
            yield return sp + "last=New(_straight" + s + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
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

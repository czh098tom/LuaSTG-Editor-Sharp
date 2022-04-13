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
    [Serializable, NodeIcon("bulletcreatestraightex.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(3)]
    public class CreateBulletGroup : FixedAttributeTreeNode
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
            Style = style;
            Color = color;
            Position = pos;
            Number = num;
            Interval = intv;
            VelocityBegin = vb;
            VelocityEnd = ve;
            Angle = r;
            AngleSpread = rs;
            AimToPlayer = aim;
            RotationVelocity = rotv;
            StayOnCreate = stay;
            Destroyable = destroyable;
            Time = time;
            Rebound = rebound;
            /*
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
        public string Number
        {
            get => DoubleCheckAttr(3).attrInput;
            set => DoubleCheckAttr(3).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Interval
        {
            get => DoubleCheckAttr(4).attrInput;
            set => DoubleCheckAttr(4).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string VelocityBegin
        {
            get => DoubleCheckAttr(5, "velocity", "Velocity begin").attrInput;
            set => DoubleCheckAttr(5, "velocity", "Velocity begin").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string VelocityEnd
        {
            get => DoubleCheckAttr(6, "velocity", "Velocity end").attrInput;
            set => DoubleCheckAttr(6, "velocity", "Velocity end").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Angle
        {
            get => DoubleCheckAttr(7, "rotation").attrInput;
            set => DoubleCheckAttr(7, "rotation").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string AngleSpread
        {
            get => DoubleCheckAttr(8, name: "Angle spread").attrInput;
            set => DoubleCheckAttr(8, name: "Angle spread").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string AimToPlayer
        {
            get => DoubleCheckAttr(9, "bool", "Aim to player").attrInput;
            set => DoubleCheckAttr(9, "bool", "Aim to player").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string RotationVelocity
        {
            get => DoubleCheckAttr(10, "omega", "Rotation velocity").attrInput;
            set => DoubleCheckAttr(10, "omega", "Rotation velocity").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string StayOnCreate
        {
            get => DoubleCheckAttr(11, "bool", "Stay on create").attrInput;
            set => DoubleCheckAttr(11, "bool", "Stay on create").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Destroyable
        {
            get => DoubleCheckAttr(12, "bool").attrInput;
            set => DoubleCheckAttr(12, "bool").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(13).attrInput;
            set => DoubleCheckAttr(13).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Rebound
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
                s += Macrolize(at) + ",";
            }
            yield return sp + "_create_bullet_group(" + s + "self)\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
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

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
    [Serializable, NodeIcon("bulletcleanrange.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class BulletClearRange : TreeNode
    {
        [JsonConstructor]
        private BulletClearRange() : base() { }

        public BulletClearRange(DocumentData workSpaceData)
            : this(workSpaceData, "player.x,player.y", "48", "15", "45", "true", "true", "0")
        { }

        public BulletClearRange(DocumentData workSpaceData, string pos, string radius, string expTime, string dur
            , string convToFaith, string clrIndes, string vy)
            : base(workSpaceData)
        {
            Position = pos;
            RangeRadius = radius;
            ExpandingTime = expTime;
            Duration = dur;
            ConvToFaith = convToFaith;
            ClearIndest = clrIndes;
            Y_Velocity = vy;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(0, "position").attrInput;
            set => DoubleCheckAttr(0, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string RangeRadius
        {
            get => DoubleCheckAttr(1, name: "Radius of range").attrInput;
            set => DoubleCheckAttr(1, name: "Radius of range").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ExpandingTime
        {
            get => DoubleCheckAttr(2, name: "Time of expanding").attrInput;
            set => DoubleCheckAttr(2, name: "Time of expanding").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Duration
        {
            get => DoubleCheckAttr(3).attrInput;
            set => DoubleCheckAttr(3).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ConvToFaith
        {
            get => DoubleCheckAttr(4, "bool", "Convert to faith").attrInput;
            set => DoubleCheckAttr(4, "bool", "Convert to faith").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ClearIndest
        {
            get => DoubleCheckAttr(5, "bool", "Clear indestructible").attrInput;
            set => DoubleCheckAttr(5, "bool", "Clear indestructible").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Y_Velocity
        {
            get => DoubleCheckAttr(6, name: "Y-velocity").attrInput;
            set => DoubleCheckAttr(6, name: "Y-velocity").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s = "";
            foreach (AttrItem ai in attributes)
            {
                s += "," + Macrolize(ai);
            }
            yield return sp + "New(bullet_cleaner" + s + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Clear bullets in " + NonMacrolize(1) + " from (" + NonMacrolize(0) 
                + (NonMacrolize(4) == "true" ? ") (including indestructible)" : ")") 
                + " expand in " + NonMacrolize(2) + " frame(s), last for " + NonMacrolize(3) 
                + " frame(s), move with vy=" + NonMacrolize(6) + (NonMacrolize(5) == "true" ? ", and convert bullet to faith" : "");
        }

        public override object Clone()
        {
            var n = new BulletClearRange(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

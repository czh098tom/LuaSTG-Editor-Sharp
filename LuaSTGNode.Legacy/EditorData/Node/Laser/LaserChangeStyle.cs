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
    [Serializable, NodeIcon("laserchangestyle.png")]
    [RequireAncestor(typeof(LaserAlikeTypes))]
    [RequireAncestor(typeof(TaskAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(2)]
    public class LaserChangeStyle : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private LaserChangeStyle() : base() { }

        public LaserChangeStyle(DocumentData workSpaceData)
            : this(workSpaceData, "", "1")
        { }

        public LaserChangeStyle(DocumentData workSpaceData, string color, string style)
            : base(workSpaceData)
        {
            Target = "self";
            Color = color;
            Style = style;
            /*
            attributes.Add(new AttrItem("Target", "self", this, "target"));
            attributes.Add(new AttrItem("Color", color, this, "nullableColor"));
            attributes.Add(new AttrItem("Style", style, this, "laserStyle"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Color
        {
            get => DoubleCheckAttr(1, "nullableColor").attrInput;
            set => DoubleCheckAttr(1, "nullableColor").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Style
        {
            get => DoubleCheckAttr(2, "laserStyle").attrInput;
            set => DoubleCheckAttr(2, "laserStyle").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string target;
            if (string.IsNullOrEmpty(NonMacrolize(1)))
            {
                target = "";
            }
            else
            {
                target = "," + Macrolize(1);
            }
            yield return sp + "laser.ChangeImage(" + Macrolize(0) + "," + Macrolize(2) + target + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "Change color and style of \"" + NonMacrolize(0) + "\" to " + NonMacrolize(1) + ", " + NonMacrolize(2);
        }

        public override object Clone()
        {
            var n = new LaserChangeStyle(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

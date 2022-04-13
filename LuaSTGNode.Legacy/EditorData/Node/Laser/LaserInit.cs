using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Laser
{
    [Serializable, NodeIcon("laserinit.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(LaserDefine)), Uniqueness]
    [RCInvoke(0)]
    public class LaserInit : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private LaserInit() : base() { }

        public LaserInit(DocumentData workSpaceData)
            : this(workSpaceData, "", "COLOR_RED", "1", "64", "32", "64", "8", "0", "0") { }

        public LaserInit(DocumentData workSpaceData, string para, string color, string style, string hlength
            , string blength, string tlength, string width, string nsize, string hsize)
            : base(workSpaceData)
        {
            ParameterList = para;
            Color = color;
            Style = style;
            HeadLength = hlength;
            BodyLength = blength;
            TailLength = tlength;
            Width = width;
            NodeSize = nsize;
            HeadSize = hsize;
            /*
            attributes.Add(new AttrItem("Parameter List", para, this));
            attributes.Add(new AttrItem("Color", color, this, "color"));
            attributes.Add(new AttrItem("Style", style, this, "laserStyle"));
            attributes.Add(new AttrItem("Head Length", hlength, this, "length"));
            attributes.Add(new AttrItem("Body Length", blength, this, "length"));
            attributes.Add(new AttrItem("Tail Length", tlength, this, "length"));
            attributes.Add(new AttrItem("Width", width, this, "length"));
            attributes.Add(new AttrItem("Node size", nsize, this));
            attributes.Add(new AttrItem("Head size", hsize, this));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string ParameterList
        {
            get => DoubleCheckAttr(0, name: "Parameter List").attrInput;
            set => DoubleCheckAttr(0, name: "Parameter List").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Color
        {
            get => DoubleCheckAttr(1, "color").attrInput;
            set => DoubleCheckAttr(1, "color").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Style
        {
            get => DoubleCheckAttr(2, "laserStyle").attrInput;
            set => DoubleCheckAttr(2, "laserStyle").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string HeadLength
        {
            get => DoubleCheckAttr(3, "length", "Head Length").attrInput;
            set => DoubleCheckAttr(3, "length", "Head Length").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string BodyLength
        {
            get => DoubleCheckAttr(4, "length", "Body Length").attrInput;
            set => DoubleCheckAttr(4, "length", "Body Length").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string TailLength
        {
            get => DoubleCheckAttr(5, "length", "Tail Length").attrInput;
            set => DoubleCheckAttr(5, "length", "Tail Length").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Width
        {
            get => DoubleCheckAttr(6, "length").attrInput;
            set => DoubleCheckAttr(6, "length").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string NodeSize
        {
            get => DoubleCheckAttr(7, name: "Node size").attrInput;
            set => DoubleCheckAttr(7, name: "Node size").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string HeadSize
        {
            get => DoubleCheckAttr(8, name: "Head size").attrInput;
            set => DoubleCheckAttr(8, name: "Head size").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            TreeNodeBase parent = GetLogicalParent();
            string parentName = DefinitionWithDifficulty.GetNameWithDifficulty(parent);
            string p = (!string.IsNullOrEmpty(NonMacrolize(0)) ? NonMacrolize(0) : "_");
            yield return sp + "_editor_class[\"" + parentName + "\"].init=function(self,_x,_y," + p + ")\n"
                         + s1 + "laser.init(self," + Macrolize(1) + ",_x,_y,0," + Macrolize(3) + ","
                         + Macrolize(4) + "," + Macrolize(5) + "," + Macrolize(6) + "," + Macrolize(7) 
                         + "," + Macrolize(8) + ")\n";
            string style = Macrolize(2);
            if (string.IsNullOrEmpty(style)) style = "1";
            if (style != "1")
            {
                yield return sp + s1 + "laser.ChangeImage(self," + style + ")\n";
            }
            else
            {
                yield return sp + s1 + "\n";
            }
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(3, this);
            foreach (Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "on init(" + NonMacrolize(0) + ")";
        }

        public override object Clone()
        {
            var n = new LaserInit(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            a.AddRange(DefinitionWithDifficulty.PopulateMessageOfFinding(GetLogicalParent(), this));
            return a;
        }
    }
}

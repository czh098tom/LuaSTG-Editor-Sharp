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
    [Serializable, NodeIcon("laserbentinit.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(BentLaserDefine)), Uniqueness]
    [RCInvoke(0)]
    public class BentLaserInit : TreeNode
    {
        [JsonConstructor]
        private BentLaserInit() : base() { }

        public BentLaserInit(DocumentData workSpaceData)
            : this(workSpaceData, "", "COLOR_RED", "32", "8", "4", "0") { }

        public BentLaserInit(DocumentData workSpaceData, string para, string color, string length, string width
            , string sampling, string node)
            : base(workSpaceData)
        {
            ParameterList = para;
            Color = color;
            LengthInFrames = length;
            Width = width;
            Sampling = sampling;
            Node = node;
            /*
            attributes.Add(new AttrItem("Parameter List", para, this));
            attributes.Add(new AttrItem("Color", color, this, "color"));
            attributes.Add(new AttrItem("Length (in frames)", length, this, "time"));
            attributes.Add(new AttrItem("Width", width, this, "length"));
            attributes.Add(new AttrItem("Sampling", sampling, this));
            attributes.Add(new AttrItem("Node", node, this, "length"));
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
        public string LengthInFrames
        {
            get => DoubleCheckAttr(2, "time", "Length (in frames)").attrInput;
            set => DoubleCheckAttr(2, "time", "Length (in frames)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Width
        {
            get => DoubleCheckAttr(3, "length").attrInput;
            set => DoubleCheckAttr(3, "length").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Sampling
        {
            get => DoubleCheckAttr(4).attrInput;
            set => DoubleCheckAttr(4).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Node
        {
            get => DoubleCheckAttr(5, "length").attrInput;
            set => DoubleCheckAttr(5, "length").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            if (Parent?.attributes != null && Parent.AttributeCount >= 2)
            {
                parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0) +
                    (Parent.NonMacrolize(1) == "All" ? "" : ":" + Parent.NonMacrolize(1)));
            }
            string p = (!string.IsNullOrEmpty(NonMacrolize(0)) ? NonMacrolize(0) : "_");
            yield return sp + "_editor_class[\"" + parentName + "\"].init=function(self,_x,_y," + p + ")\n"
                         + sp + s1 + "laser_bent.init(self," + Macrolize(1) + ",_x,_y," + Macrolize(2) + ","
                         + Macrolize(3) + "," + Macrolize(4) + "," + Macrolize(5) + ")\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(2, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "on init(" + NonMacrolize(0) + ")";
        }

        public override object Clone()
        {
            var n = new BentLaserInit(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            TreeNode p = GetLogicalParent();
            if (p?.attributes == null || p.AttributeCount < 2)
            {
                a.Add(new CannotFindAttributeInParent(2, this));
            }
            return a;
        }
    }
}

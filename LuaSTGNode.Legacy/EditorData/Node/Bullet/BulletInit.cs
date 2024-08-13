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

namespace LuaSTGEditorSharp.EditorData.Node.Bullet
{
    [Serializable, NodeIcon("bulletinit.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(BulletDefine)), Uniqueness]
    [RCInvoke(0)]
    public class BulletInit : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private BulletInit() : base() { }

        public BulletInit(DocumentData workSpaceData)
            : this(workSpaceData, "", "arrow_big", "COLOR_RED") { }

        public BulletInit(DocumentData workSpaceData, string para, string style, string color)
            : base(workSpaceData)
        {
            Parameters = para;
            Style = style;
            Color = color;
            Stay = "true";
            Destroyable = "true";
            /*
            attributes.Add(new AttrItem("Parameter List", para, this));
            attributes.Add(new AttrItem("Style", style, this, "bulletStyle"));
            attributes.Add(new AttrItem("Color", color, this, "color"));
            attributes.Add(new AttrItem("Stay on create", "true", this, "bool"));
            attributes.Add(new AttrItem("Destroyable", "true", this, "bool"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Parameters
        {
            get => DoubleCheckAttr(0, name: "Parameter List").attrInput;
            set => DoubleCheckAttr(0, name: "Parameter List").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Style
        {
            get => DoubleCheckAttr(1, "bulletStyle").attrInput;
            set => DoubleCheckAttr(1, "bulletStyle").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Color
        {
            get => DoubleCheckAttr(2, "color").attrInput;
            set => DoubleCheckAttr(2, "color").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Stay
        {
            get => DoubleCheckAttr(3, "bool", "Stay on create").attrInput;
            set => DoubleCheckAttr(3, "bool", "Stay on create").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Destroyable
        { 
            get => DoubleCheckAttr(4, "bool").attrInput;
            set => DoubleCheckAttr(4, "bool").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(spacing);
            TreeNodeBase parent = GetLogicalParent();
            string parentName = DefinitionWithDifficulty.GetNameWithDifficulty(parent);
            string p = (!string.IsNullOrEmpty(NonMacrolize(0)) ? "," + NonMacrolize(0) : string.Empty);
            yield return sp + "_editor_class[\"" + parentName + "\"].init=function(self,_x,_y" + p + ")\n"
                         + sp + s1 + "bullet.init(self," + Macrolize(1) + "," + Macrolize(2) + "," + Macrolize(3) + "," + Macrolize(4) + ")\n"
                         + sp + s1 + "self.x,self.y=_x,_y\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(3, this);
            foreach(Tuple<int,TreeNodeBase> t in GetChildLines())
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
            var n = new BulletInit(parentWorkSpace);
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

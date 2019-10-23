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

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/objectinit.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(ObjectDefine)), Uniqueness]
    [RCInvoke(0)]
    public class ObjectInit : TreeNode
    {
        [JsonConstructor]
        private ObjectInit() : base() { }

        public ObjectInit(DocumentData workSpaceData)
            : this(workSpaceData, "", "\"leaf\"", "LAYER_ENEMY_BULLET", "GROUP_ENEMY_BULLET", "false", "true"
                  , "false", "10", "true") { }

        public ObjectInit(DocumentData workSpaceData, string para, string image, string layer, string group
            , string hide, string bound, string autorot, string hp, string colli)
            : base(workSpaceData)
        {
            ParameterList = para;
            Image = image;
            Layer = layer;
            Group = group;
            Hide = hide;
            Bound = bound;
            AutoRotation = autorot;
            HP = hp;
            Collision = colli;
            /*
            attributes.Add(new AttrItem("Parameter List", para, this));
            attributes.Add(new AttrItem("Image", image, this, "image"));
            attributes.Add(new AttrItem("Layer", layer, this, "layer"));
            attributes.Add(new AttrItem("Group", group, this, "group"));
            attributes.Add(new AttrItem("Hide", hide, this, "bool"));
            attributes.Add(new AttrItem("Bound", bound, this, "bool"));
            attributes.Add(new AttrItem("Auto Rotation", autorot, this, "bool"));
            attributes.Add(new AttrItem("HP", hp, this));
            attributes.Add(new AttrItem("Collision", colli, this, "bool"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string ParameterList
        {
            get => DoubleCheckAttr(0, name: "Parameter List").attrInput;
            set => DoubleCheckAttr(0, name: "Parameter List").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(1, "image").attrInput;
            set => DoubleCheckAttr(1, "image").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Layer
        {
            get => DoubleCheckAttr(2, "layer").attrInput;
            set => DoubleCheckAttr(2, "layer").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Group
        {
            get => DoubleCheckAttr(3, "group").attrInput;
            set => DoubleCheckAttr(3, "group").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Hide
        {
            get => DoubleCheckAttr(4, "bool").attrInput;
            set => DoubleCheckAttr(4, "bool").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Bound
        {
            get => DoubleCheckAttr(5, "bool").attrInput;
            set => DoubleCheckAttr(5, "bool").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string AutoRotation
        {
            get => DoubleCheckAttr(6, "bool", "Auto Rotation").attrInput;
            set => DoubleCheckAttr(6, "bool", "Auto Rotation").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string HP
        {
            get => DoubleCheckAttr(7).attrInput;
            set => DoubleCheckAttr(7).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Collision
        {
            get => DoubleCheckAttr(8, "bool").attrInput;
            set => DoubleCheckAttr(8, "bool").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(4);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            if (Parent?.attributes != null && Parent.AttributeCount >= 2)
            {
                parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0) +
                    (Parent.NonMacrolize(1) == "All" ? "" : ":" + Parent.NonMacrolize(1)));
            }
            string p = (!string.IsNullOrEmpty(NonMacrolize(0)) ? NonMacrolize(0) : "_");
            yield return "_editor_class[\"" + parentName + "\"].init=function(self,_x,_y," + p + ")\n"
                         + sp + "self.x,self.y=_x,_y\n"
                         + sp + "self.img=" + Macrolize(1) + "\n"
                         + sp + "self.layer=" + Macrolize(2) + "\n"
                         + sp + "self.group=" + Macrolize(3) + "\n"
                         + sp + "self.hide=" + Macrolize(4) + "\n"
                         + sp + "self.bound=" + Macrolize(5) + "\n"
                         + sp + "self.navi=" + Macrolize(6) + "\n"
                         + sp + "self.hp=" + Macrolize(7) + "\n"
                         + sp + "self.maxhp=" + Macrolize(7) + "\n"
                         + sp + "self.colli=" + Macrolize(8) + "\n"
                         + sp + "self._servants={}\n"
                         + sp + "self._blend,self._a,self._r,self._g,self._b='',255,255,255,255\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(13, this);
            foreach(Tuple<int,TreeNode> t in GetChildLines())
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
            var n = new ObjectInit(parentWorkSpace);
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

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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bulletinit.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(BulletDefine)), Uniqueness]
    [RCInvoke(0)]
    public class BulletInit : TreeNode
    {
        [JsonConstructor]
        private BulletInit() : base() { }

        public BulletInit(DocumentData workSpaceData)
            : this(workSpaceData, "", "arrow_big", "COLOR_RED") { }

        public BulletInit(DocumentData workSpaceData, string para, string style, string color)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Parameter List", para, this));
            attributes.Add(new AttrItem("Style", style, this, "bulletStyle"));
            attributes.Add(new AttrItem("Color", color, this, "color"));
            attributes.Add(new AttrItem("Stay on create", "true", this, "bool"));
            attributes.Add(new AttrItem("Destroyable", "true", this, "bool"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(4);
            string parentName = Lua.StringParser.ParseLua(NonMacrolize(Parent.attributes[0]) + 
                (NonMacrolize(Parent.attributes[1]) == "All" ? "" : ":" + NonMacrolize(Parent.attributes[1])));
            string p = (!string.IsNullOrEmpty(NonMacrolize(0)) ? NonMacrolize(0) : "_");
            yield return "_editor_class[\"" + parentName + "\"].init=function(self,_x,_y," + p + ")\n"
                         + sp + "bullet.init(self," + Macrolize(1) + "," + Macrolize(2) + "," + Macrolize(3) + "," + Macrolize(4) + ")\n"
                         + sp + "self.x,self.y=_x,_y\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(3, this);
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
            var n = new BulletInit(parentWorkSpace)
            {
                attributes = new ObservableCollection<AttrItem>(from AttrItem a in attributes select (AttrItem)a.Clone()),
                Children = new ObservableCollection<TreeNode>(from TreeNode t in Children select (TreeNode)t.Clone()),
                _parent = _parent,
                isExpanded = isExpanded
            };
            n.FixAttrParent();
            n.FixChildrenParent();
            return n;
        }
    }
}

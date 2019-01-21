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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/laserinit.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(LaserDefine)), Uniqueness]
    [RCInvoke(0)]
    public class LaserInit : TreeNode
    {
        [JsonConstructor]
        private LaserInit() : base() { }

        public LaserInit(DocumentData workSpaceData)
            : this(workSpaceData, "", "COLOR_RED", "1", "64", "32", "64", "8", "0", "0") { }

        public LaserInit(DocumentData workSpaceData, string para, string color, string style, string hlength
            , string blength, string tlength, string width, string nsize, string hsize)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Parameter List", para, this));
            attributes.Add(new AttrItem("Color", color, this, "color"));
            attributes.Add(new AttrItem("Style", style, this, "laserStyle"));
            attributes.Add(new AttrItem("Head Length", hlength, this, "length"));
            attributes.Add(new AttrItem("Body Length", blength, this, "length"));
            attributes.Add(new AttrItem("Tail Length", tlength, this, "length"));
            attributes.Add(new AttrItem("Width", width, this, "length"));
            attributes.Add(new AttrItem("Node size", nsize, this));
            attributes.Add(new AttrItem("Head size", hsize, this));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(4);
            string parentName = Lua.StringParser.ParseLua(NonMacrolize(Parent.attributes[0]) +
                (NonMacrolize(Parent.attributes[1]) == "All" ? "" : ":" + NonMacrolize(Parent.attributes[1])));
            string p = (!string.IsNullOrEmpty(NonMacrolize(0)) ? NonMacrolize(0) : "_");
            yield return "_editor_class[\"" + parentName + "\"].init=function(self,_x,_y," + p + ")\n"
                         + sp + "laser.init(self," + Macrolize(1) + ",_x,_y,0," + Macrolize(3) + ","
                         + Macrolize(4) + "," + Macrolize(5) + "," + Macrolize(6) + "," + Macrolize(7) 
                         + "," + Macrolize(8) + ")\n";
            string style = Macrolize(2);
            if (string.IsNullOrEmpty(style)) style = "1";
            if (style != "1")
            {
                yield return sp + "laser.ChangeImage(self," + style + ")\n";
            }
            else
            {
                yield return sp + "\n";
            }
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(3, this);
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
            var n = new LaserInit(parentWorkSpace)
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

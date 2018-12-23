﻿using System;
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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/laserbentinit.png")]
    [CannotDelete]
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
            attributes.Add(new AttrItem("Parameter List", para, this));
            attributes.Add(new AttrItem("Color", color, this, "color"));
            attributes.Add(new AttrItem("Length (in frames)", length, this, "time"));
            attributes.Add(new AttrItem("Width", width, this, "length"));
            attributes.Add(new AttrItem("Sampling", sampling, this));
            attributes.Add(new AttrItem("Node", node, this, "length"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(4);
            string parentName = Lua.StringParser.ParseLua(NonMacrolize(Parent.attributes[0]) +
                (NonMacrolize(Parent.attributes[1]) == "All" ? "" : ":" + NonMacrolize(Parent.attributes[1])));
            string p = (!string.IsNullOrEmpty(NonMacrolize(0)) ? NonMacrolize(0) : "_");
            yield return "_editor_class[\"" + parentName + "\"].init=function(self,_x,_y," + p + ")\n"
                         + sp + "laser_bent.init(self," + Macrolize(1) + ",_x,_y," + Macrolize(2) + ","
                         + Macrolize(3) + "," + Macrolize(4) + "," + Macrolize(5) + ")\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return "end\n";
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
            var n = new BentLaserInit(parentWorkSpace)
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

﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bosscreate.png")]
    [RequireAncestor(typeof(Stage.Stage))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class CreateBoss : TreeNode
    {
        [JsonConstructor]
        private CreateBoss() : base() { }

        public CreateBoss(DocumentData workSpaceData)
            : this(workSpaceData, "", "true")
        { }

        public CreateBoss(DocumentData workSpaceData, string name, string wait)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Name", name, this, "bossDef"));
            attributes.Add(new AttrItem("Wait", wait, this, "bool"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "local _boss_wait=" + Macrolize(1) + "\n"
                       + sp + "local _ref=New(_editor_class[" + Macrolize(0) + "],_editor_class[" + Macrolize(0) + "].cards)\n"
                       + sp + "last=_ref\n"
                       + sp + "if _boss_wait then while IsValid(_ref) do task.Wait() end end\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(4, this);
        }

        public override string ToString()
        {
            return "Create boss " + NonMacrolize(0) + (NonMacrolize(1)=="true"?", wait":"");
        }

        public override object Clone()
        {
            var n = new CreateBoss(parentWorkSpace)
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

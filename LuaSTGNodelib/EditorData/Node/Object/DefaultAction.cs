using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/defaultaction.png")]
    [RequireAncestor(typeof(CallBackFunc))]
    [LeafNode]
    public class DefaultAction : TreeNode
    {
        [JsonConstructor]
        private DefaultAction() : base() { }

        public DefaultAction(DocumentData workSpaceData)
            : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            TreeNode callBackFunc = this;
            while(!(callBackFunc is CallBackFunc))
            {
                callBackFunc = callBackFunc.Parent;
            }
            string other = NonMacrolize(callBackFunc.attributes[0]) == "colli" ? ",other" : "";
            yield return sp + "self.class.base." + NonMacrolize(callBackFunc.attributes[0]) + "(self" + other + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Do default action";
        }

        public override object Clone()
        {
            var n = new DefaultAction(parentWorkSpace)
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

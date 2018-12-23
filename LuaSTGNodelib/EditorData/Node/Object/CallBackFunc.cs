using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/callbackfunc.png")]
    [RequireParent(typeof(Bullet.BulletDefine), typeof(Laser.BentLaserDefine))]
    [RCInvoke(0)]
    public class CallBackFunc : TreeNode
    {
        [JsonConstructor]
        private CallBackFunc() : base() { }

        public CallBackFunc(DocumentData workSpaceData)
            : this(workSpaceData, "frame") { }

        public CallBackFunc(DocumentData workSpaceData, string ev)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Event type", ev, this, "event"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string parentName = Lua.StringParser.ParseLua(NonMacrolize(Parent.attributes[0]) + 
                (NonMacrolize(Parent.attributes[1]) == "All" ? "" : ":" + NonMacrolize(Parent.attributes[1])));
            string other = NonMacrolize(0) == "colli" ? ",other" : "";
            yield return "_editor_class[\"" + parentName + "\"]." + NonMacrolize(0) + "=function(self" + other + ")\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "on " + NonMacrolize(0) + "()";
        }

        public override object Clone()
        {
            var n = new CallBackFunc(parentWorkSpace)
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

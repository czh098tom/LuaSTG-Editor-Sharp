using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/callbackfunc.png")]
    [RequireParent(typeof(Bullet.BulletDefine), typeof(Laser.BentLaserDefine), typeof(Object.ObjectDefine))]
    [RCInvoke(0)]
    public class CallBackFunc : TreeNode, ICallBackFunc
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
            TreeNode Parent = this.Parent;
            string parentName = "";
            if (Parent?.attributes != null && Parent.AttributeCount >= 2) 
            {
                parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0) +
                   (Parent.NonMacrolize(1) == "All" ? "" : ":" + Parent.NonMacrolize(1)));
            }
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
            var n = new CallBackFunc(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            if (Parent?.attributes == null || Parent.AttributeCount < 2)
            {
                a.Add(new CannotFindAttributeInParent(2, this));
            }
            return a;
        }

        public string FuncName
        {
            get => Macrolize(0);
        }
    }
}

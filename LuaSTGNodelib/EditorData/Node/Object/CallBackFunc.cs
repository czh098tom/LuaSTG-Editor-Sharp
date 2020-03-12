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
    [Serializable, NodeIcon("callbackfunc.png")]
    [RequireParent(typeof(ObjectPoolTypeAlikeTypes))]
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
            EventType = ev;
            //attributes.Add(new AttrItem("Event type", ev, this, "event"));
        }

        [JsonIgnore, NodeAttribute]
        public string EventType
        {
            get => DoubleCheckAttr(0, "event", "Event type").attrInput;
            set => DoubleCheckAttr(0, "event", "Event type").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            if (Parent?.attributes != null && Parent.AttributeCount >= 2) 
            {
                parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0) +
                   (Parent.NonMacrolize(1) == "All" ? "" : ":" + Parent.NonMacrolize(1)));
            }
            string other = NonMacrolize(0) == "colli" ? ",other" : "";
            yield return sp + "_editor_class[\"" + parentName + "\"]." + NonMacrolize(0) + "=function(self" + other + ")\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
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
            TreeNode p = GetLogicalParent();
            if (p?.attributes == null || p.AttributeCount < 2)
            {
                a.Add(new CannotFindAttributeInParent(2, this));
            }
            return a;
        }

        [JsonIgnore]
        public string FuncName
        {
            get => Macrolize(0);
        }
    }
}

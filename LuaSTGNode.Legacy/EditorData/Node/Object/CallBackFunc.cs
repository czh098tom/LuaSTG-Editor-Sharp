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
    public class CallBackFunc : FixedAttributeTreeNode, ICallBackFunc
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
            TreeNodeBase parent = GetLogicalParent();
            string parentName = DefinitionWithDifficulty.GetNameWithDifficulty(parent);
            string other = NonMacrolize(0) == "colli" ? ",other" : "";
            yield return sp + "_editor_class[\"" + parentName + "\"]." + NonMacrolize(0) + "=function(self" + other + ")\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
            foreach(Tuple<int,TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNodeBase>(1, this);
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
            a.AddRange(DefinitionWithDifficulty.PopulateMessageOfFinding(GetLogicalParent(), this));
            return a;
        }

        [JsonIgnore]
        public string FuncName
        {
            get => Macrolize(0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Document;

namespace LuaSTGEditorSharp.EditorData.Node.Task
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/taskdefine.png")]
    [ClassNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class TaskDefine : TreeNode
    {
        [JsonConstructor]
        private TaskDefine() : base() { }

        public TaskDefine(DocumentData workSpaceData) : this(workSpaceData, "", "") { }

        public TaskDefine(DocumentData workSpaceData, string name, string parameter) : base(workSpaceData)
        {
            Name = name;
            Parameter = parameter;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Parameter
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(4);
            yield return "_editor_tasks[\"" + Lua.StringParser.ParseLua(NonMacrolize(0)) + "\"]=function(" + Macrolize(1) + ")\n" +
                sp + "return function()\n" +
                sp + sp + "local self=task.GetSelf()\n";
            foreach(string s in base.ToLua(spacing + 1))
            {
                yield return s;
            }
            yield return sp + "end\n" + "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(3, this);
            foreach (Tuple<int, TreeNode> t in base.GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(2, this);
        }

        public override string ToString()
        {
            return "Define task \"" + NonMacrolize(0) + "\" with parameter (" + NonMacrolize(1) + ")";
        }

        public override MetaInfo GetMeta()
        {
            return new TaskDefineMetaInfo(this);
        }

        public override object Clone()
        {
            TreeNode n = new TaskDefine(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (string.IsNullOrEmpty(NonMacrolize(0)))
                messages.Add(new ArgNotNullMessage(attributes[0].AttrCap, 0, this));
            return messages;
        }
    }
}

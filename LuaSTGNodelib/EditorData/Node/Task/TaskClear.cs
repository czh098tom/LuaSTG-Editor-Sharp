using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Task
{
    [Serializable, NodeIcon("taskclear.png")]
    [RequireAncestor(typeof(TaskAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class TaskClear : TreeNode
    {
        [JsonConstructor]
        public TaskClear() : base() { }

        public TaskClear(DocumentData workSpaceData) : this(workSpaceData, "true") { }

        public TaskClear(DocumentData workSpaceData, string keepThis) : base(workSpaceData)
        {
            KeepThis = keepThis;
        }

        [JsonIgnore, NodeAttribute]
        public string KeepThis
        {
            get => DoubleCheckAttr(0, "bool", "Keep this").attrInput;
            set => DoubleCheckAttr(0, "bool", "Keep this").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            if (string.IsNullOrEmpty(Macrolize(0)) || Macrolize(0) == "false")
            {
                yield return sp + "task.Clear(self)\n";
            }
            else
            {
                yield return sp + "task.Clear(self," + Macrolize(0) + ")\n";
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(NonMacrolize(0)) || NonMacrolize(0) == "false")
            {
                return "Clear all task(s)";
            }
            else
            {
                return "Clear all other task(s)";
            }
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override object Clone()
        {
            TreeNode n = new TaskClear(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

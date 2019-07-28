using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Task
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/taskattach.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class TaskCreate : TreeNode
    {
        [JsonConstructor]
        private TaskCreate() : base() { }

        public TaskCreate(DocumentData workSpaceData) : this(workSpaceData, "", "", "self") { }

        public TaskCreate(DocumentData workSpaceData, string name, string parameter, string target) : base(workSpaceData)
        {
            Name = name;
            Parameters = parameter;
            Target = target;
        }

        [JsonIgnore]
        public string Name
        {
            get => DoubleCheckAttr(0, "taskDef").attrInput;
            set => DoubleCheckAttr(0, "taskDef").attrInput = value;
        }

        [JsonIgnore]
        public string Parameters
        {
            get => DoubleCheckAttr(1, "taskParam").attrInput;
            set => DoubleCheckAttr(1, "taskParam").attrInput = value;
        }

        [JsonIgnore]
        public string Target
        {
            get => DoubleCheckAttr(2, "target").attrInput;
            set => DoubleCheckAttr(2, "target").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "lasttask=task.New(" + Macrolize(2) + ",_editor_tasks[" + Macrolize(0) + "](" + Macrolize(1) + "))\n";
        }

        public override string ToString()
        {
            return "Attach task " + NonMacrolize(0) + " to " + NonMacrolize(2) + " with parameter(" + NonMacrolize(1) + ")";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override object Clone()
        {
            TreeNode n = new TaskCreate(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

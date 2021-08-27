using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Task
{
    [Serializable, NodeIcon("taskforobject.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    public class TaskForObject : TreeNode
    {
        [JsonConstructor]
        private TaskForObject() : base() { }

        public TaskForObject(DocumentData workSpaceData) : this(workSpaceData, "last", "true") { }

        public TaskForObject(DocumentData workSpaceData, string target, string resettarget) : base(workSpaceData)
        {
            Target = target;
            ResetTargetInTask = resettarget;
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ResetTargetInTask
        {
            get => DoubleCheckAttr(1, "bool", "Redirect self").attrInput;
            set => DoubleCheckAttr(1, "bool", "Redirect self").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"lasttask=task.New({Macrolize(0)},function()\n"
                + (attributes[1].AttrInput == "true" ? Indent(spacing + 1) + "local self=task.GetSelf()\n" : "");
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end)\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return $"Create task for {NonMacrolize(0)}{(attributes[1].AttrInput == "true" ? " and redirect \"self\" to the target of task" : "")}";
        }

        public override object Clone()
        {
            var n = new TaskForObject(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

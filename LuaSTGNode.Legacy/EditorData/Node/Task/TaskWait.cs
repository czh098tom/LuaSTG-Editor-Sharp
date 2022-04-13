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

namespace LuaSTGEditorSharp.EditorData.Node.Task
{
    [Serializable, NodeIcon("taskwait.png")]
    [RequireAncestor(typeof(TaskAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class TaskWait : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private TaskWait() : base() { }

        public TaskWait(DocumentData workSpaceData)
            : this(workSpaceData, "60") { }

        public TaskWait(DocumentData workSpaceData, string code)
            : base(workSpaceData)
        {
            Time = code;
            //attributes.Add(new AttrItem("Time", code, this, "yield"));
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(0, "yield").attrInput;
            set => DoubleCheckAttr(0, "yield").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "task._Wait(" + Macrolize(0) + ")\n";
        }
        
        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "Wait " + attributes[0].AttrInput + " frame(s)";
        }

        public override object Clone()
        {
            var n = new TaskWait(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

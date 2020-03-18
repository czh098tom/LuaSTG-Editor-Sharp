using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Task
{
    [Serializable, NodeIcon("taskclear.png")]
    [RequireAncestor(typeof(EnemyDefine),typeof(BossDefine),typeof(ObjectDefine), typeof(LaserDefine), typeof(LaserBentDefine))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class ClearTasks : TreeNode
    {
        [JsonConstructor]
        private ClearTasks() : base() { }

        public ClearTasks(DocumentData workSpaceData) : this(workSpaceData,"true") { }

        public ClearTasks(DocumentData workSpaceData, string keepthis) : base(workSpaceData)
        {
            KeepThis = keepthis;
        }

        [JsonIgnore, NodeAttribute]
        public string KeepThis
        {
            get => DoubleCheckAttr(0, "bool","Keep this").attrInput;
            set => DoubleCheckAttr(0, "bool", "Keep this").attrInput = value;
        }


        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            if (string.IsNullOrEmpty(Macrolize(0)) || Macrolize(0) == "false")
                yield return "task.Clear(self)\n";
            else
                yield return "task.Clear(self," + Macrolize(0) + ")\n";
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Macrolize(0)) || Macrolize(0) == "false")
                yield return "Clear all task(s)";
            else
                yield return "Clear all other task(s)";
        }
        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override object Clone()
        {
            TreeNode n = new ClearTasks(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

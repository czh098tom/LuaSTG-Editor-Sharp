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
    [Serializable, NodeIcon("taskBeziermoveto.png")]
    [RequireAncestor(typeof(TaskAlikeTypes))]
    [LeafNode]
    [RCInvoke(3)]
    public class TaskMoveToCurve : TreeNode
    {
        [JsonConstructor]
        public TaskMoveToCurve() : base() { }

        public TaskMoveToCurve(DocumentData workSpaceData) : this(workSpaceData, "60", "Bezier", "MODE_NORMAL", "") { }

        public TaskMoveToCurve(DocumentData workSpaceData, string time, string type, string mode, string pset)
            : base(workSpaceData)
        {
            Time = time;
            Type = type;
            Mode = mode;
            PointSet = pset;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Type
        {
            get => DoubleCheckAttr(1, "curve").attrInput;
            set => DoubleCheckAttr(1, "curve").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Mode
        {
            get => DoubleCheckAttr(2, "interpolation").attrInput;
            set => DoubleCheckAttr(2, "interpolation").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string PointSet
        {
            get => DoubleCheckAttr(3, "pointSet", "Point set").attrInput;
            set => DoubleCheckAttr(3, "pointSet", "Point set").attrInput = value;
        }

        public override string ToString()
        {
            string[] strs = NonMacrolize(3).Split(',');
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < strs.Length; i += 2)
            {
                sb.Append(" (");
                sb.Append(strs[i].Trim());
                sb.Append(", ");
                if (i + 1 < strs.Length) sb.Append(strs[i + 1].Trim());
                sb.Append(")");
            }

            string mode = NonMacrolize(2);
            mode = string.IsNullOrEmpty(mode) || mode == "MOVE_NORMAL" ? "" : ", interpolation mode: " + mode;

            return $"Move following curve \"{NonMacrolize(1)}\" in {NonMacrolize(0)} frame(s), with fixed anchors"
                + sb.ToString() + mode;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string fr = Macrolize(0);
            fr = string.IsNullOrEmpty(fr) ? "1" : fr;
            string mode = Macrolize(2);
            mode = string.IsNullOrEmpty(mode) ? "MOVE_NORMAL" : mode;
            yield return sp + $"task.{NonMacrolize(1)}MoveTo({fr},{mode},{Macrolize(3)})\n";
        }

        public override object Clone()
        {
            var n = new TaskMoveToCurve(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

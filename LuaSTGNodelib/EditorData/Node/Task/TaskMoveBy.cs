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
    [Serializable, NodeIcon("taskmovetoex.png")]
    [RequireAncestor(typeof(TaskAlikeTypes))]
    [LeafNode]
    [RCInvoke(1)]
    public class TaskMoveBy : TreeNode
    {
        [JsonConstructor]
        private TaskMoveBy() : base() { }

        public TaskMoveBy(DocumentData workSpaceData)
            : this(workSpaceData, "0,0", "60", "MOVE_NORMAL") { }

        public TaskMoveBy(DocumentData workSpaceData, string dest, string frame, string mode)
            : base(workSpaceData)
        {
            Destination = dest;
            Frame = frame;
            Mode = mode;
            /*
            attributes.Add(new AttrItem("Destination", dest, this, "position"));
            attributes.Add(new AttrItem("Frame", frame, this));
            attributes.Add(new AttrItem("Mode", mode, this, "interpolation"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Destination
        {
            get => DoubleCheckAttr(0, "position").attrInput;
            set => DoubleCheckAttr(0, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Frame
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Mode
        {
            get => DoubleCheckAttr(2, "interpolation").attrInput;
            set => DoubleCheckAttr(2, "interpolation").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string fr = Macrolize(1);
            fr = string.IsNullOrEmpty(fr) ? "1" : fr;
            string mode = Macrolize(2);
            mode = string.IsNullOrEmpty(mode) ? "MOVE_NORMAL" : mode;
            yield return sp + "task.MoveToEx(" + Macrolize(0) + "," + fr + "," + mode + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            string fr = NonMacrolize(1);
            fr = string.IsNullOrEmpty(fr) ? "1" : fr;
            string mode = NonMacrolize(2);
            mode = string.IsNullOrEmpty(mode) || mode == "MOVE_NORMAL" ? "" : ", interpolation mode: " + mode;
            return "Move by (" + NonMacrolize(0) + ") in " + fr + " frame(s)" + mode;
        }

        public override object Clone()
        {
            var n = new TaskMoveBy(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

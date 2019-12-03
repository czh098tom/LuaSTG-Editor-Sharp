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

namespace LuaSTGEditorSharp.EditorData.Node.Enemy
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/taskbosswander.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class EnemyWander : TreeNode
    {
        [JsonConstructor]
        public EnemyWander() : base() { }

        public EnemyWander(DocumentData workspaceData) 
            : this(workspaceData,"30", "-96,96,112,144", "16,32,8,16", "MOVE_NORMAL", "MOVE_X_TOWARDS_PLAYER") { }

        public EnemyWander(DocumentData workSpaceData, string time, string range, string amp,
            string movem, string dirm) : base(workSpaceData)
        {
            Time = time;
            Range = range;
            Amplitude = amp;
            MoveMode = movem;
            DirMode = dirm;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Range
        {
            get => DoubleCheckAttr(1, "rect").attrInput;
            set => DoubleCheckAttr(1, "rect").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Amplitude
        {
            get => DoubleCheckAttr(2, "rectNonNegative").attrInput;
            set => DoubleCheckAttr(2, "rectNonNegative").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string MoveMode
        {
            get => DoubleCheckAttr(3, "interpolation", "Move mode").attrInput;
            set => DoubleCheckAttr(3, "interpolation", "Move mode").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string DirMode
        {
            get => DoubleCheckAttr(4, "directionMode", "Direction mode").attrInput;
            set => DoubleCheckAttr(4, "directionMode", "Direction mode").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "task.MoveToPlayer(" + NullOrDefault(Macrolize(0), "1") + ","
                + NullOrDefault(Macrolize(1), "-96,96,112,144") + ","
                + NullOrDefault(Macrolize(2), "16,32,8,16") + ","
                + NullOrDefault(Macrolize(3), "MOVE_NORMAL") + ","
                + NullOrDefault(Macrolize(4), "MOVE_X_TOWARDS_PLAYER") + ")\n";
        }

        public override string ToString()
        {
            return "Wander in " + NullOrDefault(NonMacrolize(0), "1")
                + " frame(s), in range (" + NullOrDefault(NonMacrolize(1), "-96,96,112,144")
                + ") and amplitude (" + NullOrDefault(NonMacrolize(2), "16,32,8,16")
                + ", " + NullOrDefault(NonMacrolize(4), "MOVE_X_TOWARDS_PLAYER");
        }

        public override object Clone()
        {
            var n = new EnemyWander(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

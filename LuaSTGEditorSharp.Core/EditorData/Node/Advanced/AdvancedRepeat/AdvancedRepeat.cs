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

namespace LuaSTGEditorSharp.EditorData.Node.Advanced.AdvancedRepeat
{
    [Serializable, NodeIcon("advancedrepeat.png")]
    [CreateInvoke(0), RCInvoke(0)]
    public class AdvancedRepeat : TreeNode
    {
        [JsonConstructor]
        public AdvancedRepeat() : base() { }

        public AdvancedRepeat(DocumentData workSpaceData) : this(workSpaceData, "_infinite") { }

        public AdvancedRepeat(DocumentData workSpaceData, string times) : base(workSpaceData) 
        {
            Times = times;
        }

        [JsonIgnore, NodeAttribute]
        public string Times
        {
            get => DoubleCheckAttr(0, "yield").attrInput;
            set => DoubleCheckAttr(0, "yield").attrInput = value;
        }

        public override string ToString()
        {
            return "repeat " + NonMacrolize(0) + " times";
        }

        public override object Clone()
        {
            var n = new AdvancedRepeat(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string sp1 = Indent(spacing + 1);
            string sp2 = Indent(spacing + 2);
            string times = Macrolize(0);
            VariableCollection vc = GetVariableCollection();
            List<Tuple<string, string>> info = new List<Tuple<string, string>>();
            foreach(VariableTransformation vt in vc.GetVariableTransformations())
            {
                info.Add(vt.GetInformation(times));
            }
            yield return sp + "do\n";
            foreach(Tuple<string,string> t in info)
            {
                yield return sp1 + t.Item1;
            }
            yield return sp1 + "for _=1," + times + " do\n";
            foreach (var a in base.ToLua(spacing + 2))
            {
                yield return a;
            }
            foreach (Tuple<string, string> t in info)
            {
                yield return sp2 + t.Item2;
            }
            yield return sp1 + "end\n";
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            VariableCollection vc = GetVariableCollection();
            List<IEnumerator<Tuple<int, TreeNode>>> lines = new List<IEnumerator<Tuple<int, TreeNode>>>();
            foreach (VariableTransformation transformation in vc.GetVariableTransformations())
            {
                lines.Add(transformation.GetLines().GetEnumerator());
            }
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (IEnumerator<Tuple<int, TreeNode>> e in lines)
            {
                e.MoveNext();
                yield return e.Current;
            }
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            foreach (IEnumerator<Tuple<int, TreeNode>> e in lines)
            {
                e.MoveNext();
                yield return e.Current;
            }
            yield return new Tuple<int, TreeNode>(2, this);
        }

        private VariableCollection GetVariableCollection()
        {
            foreach(TreeNode t in GetLogicalChildren())
            {
                if (t is VariableCollection) return t as VariableCollection;
            }
            return null;
        }
    }
}

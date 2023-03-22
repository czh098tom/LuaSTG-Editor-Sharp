using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Curve
{
    [Serializable, NodeIcon("TerminateTrack.png")]
    [RequireAncestor(typeof(NumericalCurve))]
    [LeafNode]
    public class TerminateTrack : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public TerminateTrack() : base() { }

        public TerminateTrack(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"__terminated = true\n";
        }

        public override string ToString()
        {
            return "Terminate track";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override object Clone()
        {
            var n = new TerminateTrack(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Curve
{
    [Serializable, NodeIcon("NumericalTrack.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [CreateInvoke(0), RCInvoke(0)]
    public class NumericalTrack : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public NumericalTrack() : base() { }

        public NumericalTrack(DocumentData workSpaceData) : this(workSpaceData, "self", "") { }

        public NumericalTrack(DocumentData workSpaceData, string target, string mapping) : base(workSpaceData)
        {
            Mapping = mapping;
            Target = target;
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Mapping
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string sp1 = Indent(spacing + 1);
            string sp2 = Indent(spacing + 2);
            var nodes = GetLogicalChildren()
                .OfType<NumericalCurve>()
                .Where(c => !c.IsBanned)
                .ToArray();
            var varNames = Enumerable.Range(0, GetLogicalChildren()
                    .OfType<NumericalCurve>()
                    .Where(c => !c.IsBanned)
                    .Count())
                .Select(i => $"_{i}")
                .ToArray();
            for (int i = 0; i < nodes.Length; i++)
            {
                var v0 = nodes[i].FirstPoint();
                if (v0 != null)
                {
                    yield return sp + $"local {varNames[i]} = {v0.Value}\n";
                }
                else
                {
                    yield return sp + $"local {varNames[i]}\n";
                }
            }
            int id = 0;
            foreach (var n in GetLogicalChildren())
            {
                if (!n.IsBanned)
                {
                    if (n is NumericalCurve nc)
                    {
                        foreach (var code in nc.GetCurveTranslatedForVar(spacing, Macrolize(0), varNames[id]))
                        {
                            yield return code;
                        }
                        id++;
                    }
                    else
                    {
                        foreach (var code in n.ToLua(spacing))
                        {
                            yield return code;
                        }
                    }
                }
            }
            yield return sp + $"task.New({Macrolize(0)}, function()\n";
            yield return sp + $"local self = task.GetSelf()\n";
            yield return sp1 + "for _ = 1, _infinite do\n";
            yield return sp2 + string.Format(Macrolize(1), varNames) + "\n";
            yield return sp2 + "task.Wait()\n";
            yield return sp1 + "end\n";
            yield return sp + "end)\n";
            yield break;
        }

        public override string ToString()
        {
            return $"Track on {NonMacrolize(0)} with mapping: {NonMacrolize(1)}";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(GetLogicalChildren()
                    .OfType<NumericalCurve>()
                    .Where(c => !c.IsBanned)
                    .Count(), this);
            foreach (Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNodeBase>(7, this);
        }

        public override object Clone()
        {
            var n = new NumericalTrack(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

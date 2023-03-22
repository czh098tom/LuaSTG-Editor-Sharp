using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Curve
{
    [Serializable, NodeIcon("NumericalTrack.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [CreateInvoke(1), RCInvoke(1)]
    public class NumericalTrack : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public NumericalTrack() : base() { }

        public NumericalTrack(DocumentData workSpaceData) : this(workSpaceData, "self", "", "true") { }

        public NumericalTrack(DocumentData workSpaceData, string target, string mapping, string autoTerminate) 
            : base(workSpaceData)
        {
            Mapping = mapping;
            Target = target;
            AutoTerminate = autoTerminate;
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

        [JsonIgnore, NodeAttribute]
        public string AutoTerminate
        {
            get => DoubleCheckAttr(2, "bool", "Auto terminate").attrInput;
            set => DoubleCheckAttr(2, "bool", "Auto terminate").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp0 = Indent(spacing);
            string sp1 = Indent(spacing + 1);
            string sp2 = Indent(spacing + 2);
            string sp3 = Indent(spacing + 3);
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
            yield return sp0 + $"do\n";
            yield return sp1 + $"local __terminated = false\n";
            yield return sp1 + $"local __terminateCount = 0\n";
            for (int i = 0; i < nodes.Length; i++)
            {
                var v0 = nodes[i].FirstPoint();
                if (v0 != null)
                {
                    yield return sp1 + $"local {varNames[i]} = {v0.Value}\n";
                }
                else
                {
                    yield return sp1 + $"local {varNames[i]}\n";
                }
            }
            int id = 0;
            foreach (var n in GetLogicalChildren())
            {
                if (!n.IsBanned)
                {
                    if (n is NumericalCurve nc)
                    {
                        foreach (var code in nc.GetCurveTranslatedForVar(spacing + 1, Macrolize(0), varNames[id]))
                        {
                            yield return code;
                        }
                        id++;
                    }
                    else
                    {
                        foreach (var code in n.ToLua(spacing + 1))
                        {
                            yield return code;
                        }
                    }
                }
            }
            yield return sp1 + $"task.New({Macrolize(0)}, function()\n";
            yield return sp2 + $"local self = task.GetSelf()\n";
            yield return sp2 + "for _ = 1, _infinite do\n";
            yield return sp3 + string.Format(Macrolize(1), varNames) + "\n";
            if (NonMacrolize(2) == "true")
            {
                yield return sp3 + $"if __terminated or __terminateCount >= {nodes.Length} then break end\n";
            }
            else
            {
                yield return sp3 + $"if __terminated then break end\n";
            }
            yield return sp3 + "task.Wait()\n";
            yield return sp2 + "end\n";
            yield return sp1 + "end)\n";
            yield return sp0 + $"end\n";
            yield break;
        }

        public override string ToString()
        {
            if (NonMacrolize(2) == "true")
            {
                return $"Track on {NonMacrolize(0)} with mapping: \"{NonMacrolize(1)}\", Auto terminate";
            }
            else
            {
                return $"Track on {NonMacrolize(0)} with mapping: \"{NonMacrolize(1)}\", Manually terminate";
            }
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(3, this);
            yield return new Tuple<int, TreeNodeBase>(GetLogicalChildren()
                    .OfType<NumericalCurve>()
                    .Where(c => !c.IsBanned)
                    .Count(), this);
            foreach (Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNodeBase>(9, this);
        }

        public override object Clone()
        {
            var n = new NumericalTrack(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

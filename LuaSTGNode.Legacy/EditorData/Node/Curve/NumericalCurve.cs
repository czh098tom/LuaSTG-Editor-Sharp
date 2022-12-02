using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Curve
{
    [Serializable, NodeIcon("NumericalCurve.png")]
    [RequireParent(typeof(NumericalTrack))]
    [RCInvoke(1)]
    public class NumericalCurve : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public NumericalCurve() : base() { }

        public NumericalCurve(DocumentData workSpaceData) : this(workSpaceData, "0", "None") { }

        public NumericalCurve(DocumentData workSpaceData, string offset, string repeatType) : base(workSpaceData)
        {
            Offset = offset;
            RepeatType = repeatType;
        }

        [JsonIgnore, NodeAttribute]
        public string Offset
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string RepeatType
        {
            get => DoubleCheckAttr(1, "curveRepeatType", "Repeat type").attrInput;
            set => DoubleCheckAttr(1, "curveRepeatType", "Repeat type").attrInput = value;
        }

        public NumericalPoint FirstPoint()
        {
            foreach (var node in GetLogicalChildren())
            {
                if (node is NumericalPoint point)
                {
                    if (Convert.ToInt32(point.Time) == 0 && !point.IsBanned)
                    {
                        return point;
                    }
                    return null;
                }
            }
            return null;
        }

        public IEnumerable<string> GetCurveTranslatedForVar(int spacing, string target, string name)
        {
            string sp = Indent(spacing);
            string sp1 = Indent(spacing + 1);
            string sp2 = Indent(spacing + 2);
            yield return sp + $"task.New({target}, function()\n";
            yield return sp1 + $"task.Wait({Macrolize(0)})\n";
            string spa = sp1;
            int spacinga = spacing + 1;
            if (NonMacrolize(1) == "Round Robin")
            {
                yield return sp1 + "for _ = 1, _infinite do\n";
                spa = sp2;
                spacinga++;
            }
            yield return spa + "local i = 0\n";
            yield return spa + "local curr, ibeg, ky, kx\n";
            yield return spa + $"local prev = {FirstPoint().PreferredMacrolize(1, "Y")}\n";
            yield return spa + "local target\n";
            foreach (var node in GetLogicalChildren())
            {
                if (!node.IsBanned)
                {
                    if (node is NumericalPoint point)
                    {
                        foreach (var code in point.GetIterTranslatedForVar(spacinga, name))
                        {
                            yield return code;
                        }
                    }
                    else
                    {
                        foreach (var code in node.ToLua(spacinga))
                        {
                            yield return code;
                        }
                    }
                }
            }
            if (NonMacrolize(1) == "Round Robin")
            {
                yield return sp1 + "end\n";
            }
            yield return sp + "end)\n";
        }

        public override string ToString()
        {
            string s = $"Curve, offset: {NonMacrolize(0)}";
            if (NonMacrolize(1) != "None")
            {
                s += $", {NonMacrolize(1)}";
            }
            return s;
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            if (NonMacrolize(1) == "Round Robin")
            {
                yield return new Tuple<int, TreeNodeBase>(7, this);
            }
            else
            {
                yield return new Tuple<int, TreeNodeBase>(6, this);
            }
            foreach (Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            if (NonMacrolize(1) == "Round Robin")
            {
                yield return new Tuple<int, TreeNodeBase>(2, this);
            }
            else
            {
                yield return new Tuple<int, TreeNodeBase>(1, this);
            }
        }

        public override object Clone()
        {
            var n = new NumericalCurve(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

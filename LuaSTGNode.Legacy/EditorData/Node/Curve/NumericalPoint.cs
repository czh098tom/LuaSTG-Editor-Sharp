using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.Advanced.AdvancedRepeat;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Curve
{
    [Serializable, NodeIcon("NumericalPoint.png")]
    [RequireParent(typeof(NumericalCurve)), LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class NumericalPoint : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public NumericalPoint() : base() { }

        public NumericalPoint(DocumentData workSpaceData) : this(workSpaceData, "0", "0", "", "false") { }

        public NumericalPoint(DocumentData workSpaceData, string x, string y, string interp, string isRel) : base(workSpaceData)
        {
            Time = x;
            Value = y;
            Interpolation = interp;
            IsRelative = isRel;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Value
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Interpolation
        {
            get => DoubleCheckAttr(2).attrInput;
            set => DoubleCheckAttr(2).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string IsRelative
        {
            get => DoubleCheckAttr(3, "bool", "Is relative").attrInput;
            set => DoubleCheckAttr(3, "bool", "Is relative").attrInput = value;
        }

        public IEnumerable<string> GetIterTranslatedForVar(int spacing, string name)
        {
            string sp = Indent(spacing);
            string sp1 = Indent(spacing + 1);
            yield return sp + $"curr = {Macrolize(1)}\n";
            yield return sp + "ibeg = i\n";
            if (NonMacrolize(3) == "false")
            {
                yield return sp + $"target = {Macrolize(0)} + offset\n";
            }
            else
            {
                yield return sp + $"target = target + {Macrolize(0)}\n";
            }
            yield return sp + "kx = target - ibeg\n";
            yield return sp + "ky = curr - prev\n";
            string interpolate = $"prev + ky * {Macrolize(2)}((i - ibeg) / kx)";
            if (string.IsNullOrWhiteSpace(Macrolize(2)))
            {
                interpolate = $"prev + ky * (i - ibeg) / kx";
            }
            yield return sp + $"while i < target do\n";
            yield return sp1 + $"{name} = {interpolate}\n";
            yield return sp1 + "i = i + 1\n";
            yield return sp1 + "task.Wait()\n";
            yield return sp + "end\n";
            yield return sp + $"{name} = curr\n";
            yield return sp + "prev = curr\n";
            yield break;
        }

        public override string ToString()
        {
            string s = $"=> ({(NonMacrolize(3) == "false" ? "" : "(+)")}{NonMacrolize(0)}, {NonMacrolize(1)})";
            if (!string.IsNullOrWhiteSpace(NonMacrolize(2)))
            {
                s += $", interpolate by {NonMacrolize(2)}";
            }
            return s;
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(12, this);
        }

        public override object Clone()
        {
            var n = new NumericalPoint(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

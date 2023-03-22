using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Curve
{
    [Serializable, NodeIcon("SkipTime.png")]
    [RequireAncestor(typeof(NumericalCurve))]
    [CreateInvoke(0), RCInvoke(0)]
    [LeafNode]
    public class SkipTime : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public SkipTime() : base() { }

        public SkipTime(DocumentData workSpaceData) : this(workSpaceData, "0", "false") { }

        public SkipTime(DocumentData workSpaceData, string time, string isRelative) : base(workSpaceData)
        {
            Time = time;
            IsRelative = isRelative;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string IsRelative
        {
            get => DoubleCheckAttr(1, "bool", "Is relative").attrInput;
            set => DoubleCheckAttr(1, "bool", "Is relative").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            if (NonMacrolize(1) == "true")
            {
                yield return sp + $"i = i + {Macrolize(0)}\n";
            }
            else
            {
                yield return sp + $"i = {Macrolize(0)}\n";
            }
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            if (NonMacrolize(1) == "true")
            {
                return $"Skip by {NonMacrolize(0)} frame(s) in curve";
            }
            else
            {
                return $"Skip to frame {NonMacrolize(0)} in curve";
            }
        }

        public override object Clone()
        {
            var n = new SkipTime(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

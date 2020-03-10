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
    [Serializable, NodeIcon("images/16x16/SinusoidalOscillationVariable.png")]
    [RequireParent(typeof(VariableCollection))]
    [LeafNode]
    [CreateInvoke(0)]
    public class SinusoidalMovementVariable : VariableTransformation
    {
        [JsonConstructor]
        public SinusoidalMovementVariable() : base() { }

        public SinusoidalMovementVariable(DocumentData workSpaceData) : this(workSpaceData, "", "-90", "0", "1", "1", "false") { }

        public SinusoidalMovementVariable(DocumentData workSpaceData, string name, string init, string min
            , string max, string period, string precise)
            : base(workSpaceData)
        {
            Name = name;
            InitPhase = init;
            Min = min;
            Max = max;
            Period = period;
            Precisely = precise;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string InitPhase
        {
            get => DoubleCheckAttr(1, name: "Initial phase").attrInput;
            set => DoubleCheckAttr(1, name: "Initial phase").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Min
        {
            get => DoubleCheckAttr(2).attrInput;
            set => DoubleCheckAttr(2).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Max
        {
            get => DoubleCheckAttr(3).attrInput;
            set => DoubleCheckAttr(3).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Period
        {
            get => DoubleCheckAttr(4, name: "Num of period").attrInput;
            set => DoubleCheckAttr(4, name: "Num of period").attrInput = value;
        }

        [JsonIgnore, NodeAttribute("false")]
        public string Precisely
        {
            get => DoubleCheckAttr(5, "bool").attrInput;
            set => DoubleCheckAttr(5, "bool").attrInput = value;
        }

        public override object Clone()
        {
            var n = new SinusoidalMovementVariable(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            string offchar = Precisely == "true" ? "(Precisely)" : "(Expect next value IS)";
            return $"{NonMacrolize(0)} : sinusoidal movement between {NonMacrolize(2)} <=> {NonMacrolize(3)}"
                + $" with initial phase {NonMacrolize(1)}, for {NonMacrolize(4)} period(s) {offchar}";
        }

        public override Tuple<string, string> GetInformation(string times)
        {
            string offchar = Precisely == "true" ? "-1" : "";
            string sineTokenHead = $"_h_{NonMacrolize(0)}*sin";
            string sineTokenTail = $"+_t_{NonMacrolize(0)}";
            string begin = $"local _h_{NonMacrolize(0)}=({Macrolize(3)}-({Macrolize(2)}))/2"
                + $" local _t_{NonMacrolize(0)}=({Macrolize(3)}+({Macrolize(2)}))/2"
                + $" local {NonMacrolize(0)}={sineTokenHead}({Macrolize(1)}){sineTokenTail}"
                + $" local _w_{NonMacrolize(0)}={Macrolize(1)}"
                + $" local _d_w_{NonMacrolize(0)}={Macrolize(4)}*360/({times}{offchar})\n";
            string repeat = $"_w_{NonMacrolize(0)}=_w_{NonMacrolize(0)}+_d_w_{NonMacrolize(0)}"
                + $" {NonMacrolize(0)}={sineTokenHead}(_w_{NonMacrolize(0)}){sineTokenTail}\n";
            return new Tuple<string, string>(begin, repeat);
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

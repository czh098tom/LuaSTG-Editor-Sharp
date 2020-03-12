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
    [Serializable, NodeIcon("SinusoidalInterpolationVariable.png")]
    [RequireParent(typeof(VariableCollection))]
    [LeafNode]
    [CreateInvoke(0)]
    public class SinusoidalInterpolationVariable : VariableTransformation
    {
        [JsonConstructor]
        public SinusoidalInterpolationVariable() : base() { }

        public SinusoidalInterpolationVariable(DocumentData workSpaceData) : this(workSpaceData, "", "0", "0", "false", "SINE_ACC_DEC") { }

        public SinusoidalInterpolationVariable(DocumentData workSpaceData, string name, string from, string to, string precisely
            , string interp)
            : base(workSpaceData)
        {
            Name = name;
            From = from;
            To = to;
            Precisely = precisely;
            Interpolation = interp;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string From
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string To
        {
            get => DoubleCheckAttr(2).attrInput;
            set => DoubleCheckAttr(2).attrInput = value;
        }

        [JsonIgnore, NodeAttribute("false")]
        public string Precisely
        {
            get => DoubleCheckAttr(3, "bool").attrInput;
            set => DoubleCheckAttr(3, "bool").attrInput = value;
        }

        [JsonIgnore, NodeAttribute("SINE_ACC_DEC")]
        public string Interpolation
        {
            get => DoubleCheckAttr(4, "sineinterpolation").attrInput;
            set => DoubleCheckAttr(4, "sineinterpolation").attrInput = value;
        }

        public override object Clone()
        {
            var n = new SinusoidalInterpolationVariable(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            string offchar = Precisely == "true" ? "(Precisely)" : "(Expect next value IS)";
            string mode;
            switch (NonMacrolize(4))
            {
                case "SINE_ACCEL":
                    mode = "accelerate";
                    break;
                case "SINE_DECEL":
                    mode = "decelerate";
                    break;
                default:
                    mode = "half period";
                    break;
            }
            return $"{NonMacrolize(0)} : {NonMacrolize(1)} => {NonMacrolize(2)} {offchar}, following sine interpolation, {mode}";
        }

        public override Tuple<string, string> GetInformation(string times)
        {
            string offchar = Precisely == "true" ? "-1" : "";
            string beg = $"_beg_{NonMacrolize(0)}";
            string end = $"_end_{NonMacrolize(0)}";
            string begPhase, phaseDiff, ampChar, center;
            switch (NonMacrolize(4))
            {
                case "SINE_ACCEL":
                    begPhase = "-90";
                    phaseDiff = "90";
                    ampChar = "";
                    center = end;
                    break;
                case "SINE_DECEL":
                    begPhase = "0";
                    phaseDiff = "90";
                    ampChar = "";
                    center = beg;
                    break;
                default:
                    begPhase = "-90";
                    phaseDiff = "180";
                    ampChar = "/2";
                    center = $"({end}+{beg})/2";
                    break;
            }
            string begin = $"local _beg_{NonMacrolize(0)}={Macrolize(1)} local {NonMacrolize(0)}={beg} "
                + $" local _w_{NonMacrolize(0)}={begPhase} local _end_{NonMacrolize(0)}={Macrolize(2)}"
                + $" local _d_w_{NonMacrolize(0)}={phaseDiff}/({times}{offchar})\n";
            string repeat = $"_w_{NonMacrolize(0)}=_w_{NonMacrolize(0)}+_d_w_{NonMacrolize(0)}"
                + $" {NonMacrolize(0)}=({end}-{beg}){ampChar}*sin(_w_{NonMacrolize(0)})+({center})\n";
            return new Tuple<string, string>(begin, repeat);
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Advanced.AdvancedRepeat
{
    [Serializable, NodeIcon("CustomInterpolationVariable.png")]
    [RequireParent(typeof(VariableCollection))]
    [LeafNode]
    [CreateInvoke(0)]
    public class CustomInterpolationVariable : VariableTransformation
    {
        [JsonConstructor]
        public CustomInterpolationVariable() : base() { }

        public CustomInterpolationVariable(DocumentData workSpaceData) : this(workSpaceData, "", "0", "0", "false"
            , "function(x) return x end")
        { }

        public CustomInterpolationVariable(DocumentData workSpaceData, string name, string from, string to, string precisely
            , string interp)
            : base(workSpaceData)
        {
            Name = name;
            From = from;
            To = to;
            Precisely = precisely;
            InterpolationFunc = interp;
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

        [JsonIgnore, NodeAttribute("function(x) return x end")]
        public string InterpolationFunc
        {
            get => DoubleCheckAttr(4, "code").attrInput;
            set => DoubleCheckAttr(4, "code").attrInput = value;
        }

        public override object Clone()
        {
            var n = new CustomInterpolationVariable(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            string offchar = Precisely == "true" ? "(Precisely)" : "(Expect next value IS)";
            string[] splited = NonMacrolize(4).Split('\n');
            string shortTerm = splited.Length > 1 ? splited[0].Trim() + " ..." : splited[0].Trim();
            return $"{NonMacrolize(0)} : {NonMacrolize(1)} => {NonMacrolize(2)} {offchar}"
                + $", interpolate by: {shortTerm}";
        }

        public override Tuple<string, string> GetInformation(string times)
        {
            string offchar = Precisely == "true" ? "-1" : "";
            string beg = $"_beg_{NonMacrolize(0)}";
            string end = $"_end_{NonMacrolize(0)}";
            string func = $"_func_{NonMacrolize(0)}";
            string begin = $"local _beg_{NonMacrolize(0)}={Macrolize(1)} local {NonMacrolize(0)}={beg} "
                + $" local _func_{NonMacrolize(0)}={Macrolize(4)}"
                + $" local _w_{NonMacrolize(0)}=0 local _end_{NonMacrolize(0)}={Macrolize(2)}"
                + $" local _d_w_{NonMacrolize(0)}=1/({times}{offchar})\n";
            string repeat = $"_w_{NonMacrolize(0)}=_w_{NonMacrolize(0)}+_d_w_{NonMacrolize(0)}"
                + $" {NonMacrolize(0)}=({end}-{beg})*{func}(_w_{NonMacrolize(0)})+{beg}\n";
            return new Tuple<string, string>(begin, repeat);
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }
    }
}

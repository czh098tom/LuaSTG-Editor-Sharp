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
    [Serializable, NodeIcon("LinearVariable.png")]
    [RequireParent(typeof(VariableCollection))]
    [LeafNode]
    [CreateInvoke(0)]
    public class LinearVariable : VariableTransformation
    {
        [JsonConstructor]
        public LinearVariable() : base() { }

        public LinearVariable(DocumentData workSpaceData) : this(workSpaceData, "", "0", "0", "false", "MOVE_NORMAL") { }

        public LinearVariable(DocumentData workSpaceData, string name, string from, string to, string precisely, string mode)
            : base(workSpaceData)
        {
            Name = name;
            From = from;
            To = to;
            Precisely = precisely;
            Mode = mode;
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

        [JsonIgnore, NodeAttribute("MOVE_NORMAL")]
        public string Mode
        {
            get => DoubleCheckAttr(4, "interpolation").attrInput;
            set => DoubleCheckAttr(4, "interpolation").attrInput = value;
        }

        public override object Clone()
        {
            var n = new LinearVariable(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            string offchar = Precisely == "true" ? "(Precisely)" : "(Expect next value IS)";
            string interp = "";
            switch (NonMacrolize(4))
            {
                case "MOVE_ACCEL":
                    interp = ", accelerate";
                    break;
                case "MOVE_DECEL":
                    interp = ", deaccelerate";
                    break;
                case "MOVE_ACC_DEC":
                    interp = ", accelerate, then deaccelerate";
                    break;
                default:
                    break;
            }
            return $"{NonMacrolize(0)} : {NonMacrolize(1)} => {NonMacrolize(2)} {offchar} {interp}";
        }

        public override Tuple<string, string> GetInformation(string times)
        {
            string offchar = Precisely == "true" ? "-1" : "";
            string beg = $"_beg_{NonMacrolize(0)}";
            string end = $"_end_{NonMacrolize(0)}";
            string begin, repeat;
            switch (NonMacrolize(4))
            {
                case "MOVE_ACCEL":
                    begin = $"local _beg_{NonMacrolize(0)}={Macrolize(1)} local {NonMacrolize(0)}={beg}"
                        + $" local _end_{NonMacrolize(0)}={Macrolize(2)} local _w_{NonMacrolize(0)}=0 "
                        + $" local _d_w_{NonMacrolize(0)}=1/({times}{offchar})\n";
                    repeat = $"_w_{NonMacrolize(0)}=_w_{NonMacrolize(0)}+_d_w_{NonMacrolize(0)}" 
                        + $" {NonMacrolize(0)}=({end}-{beg})*_w_{NonMacrolize(0)}^2+{beg}\n";
                    break;
                case "MOVE_DECEL":
                    begin = $"local _beg_{NonMacrolize(0)}={Macrolize(1)} local {NonMacrolize(0)}={beg}"
                        + $" local _end_{NonMacrolize(0)}={Macrolize(2)} local _w_{NonMacrolize(0)}=0"
                        + $" local _d_w_{NonMacrolize(0)}=1/({times}{offchar})\n";
                    repeat = $"_w_{NonMacrolize(0)}=_w_{NonMacrolize(0)}+_d_w_{NonMacrolize(0)}" 
                        + $" {NonMacrolize(0)}=({beg}-{end})*(_w_{NonMacrolize(0)}-1)^2+{end}\n";
                    break;
                case "MOVE_ACC_DEC":
                    begin = $"local _beg_{NonMacrolize(0)}={Macrolize(1)} local {NonMacrolize(0)}={beg}"
                        + $" local _end_{NonMacrolize(0)}={Macrolize(2)} local _w_{NonMacrolize(0)}=0"
                        + $" local _d_w_{NonMacrolize(0)}=1/({times}{offchar})\n";
                    repeat = $"_w_{NonMacrolize(0)}=_w_{NonMacrolize(0)}+_d_w_{NonMacrolize(0)}" 
                        + $" if _w_{NonMacrolize(0)}<0.5 then {NonMacrolize(0)}=2*({end}-{beg})*_w_{NonMacrolize(0)}^2+{beg} else"
                        + $" {NonMacrolize(0)}=({end}-{beg})*(-2*_w_{NonMacrolize(0)}^2+4*_w_{NonMacrolize(0)}-1)+{beg} end\n";
                    break;
                default:
                    begin = $"local _beg_{NonMacrolize(0)}={Macrolize(1)} local {NonMacrolize(0)}={beg}"
                        + $" local _end_{NonMacrolize(0)}={Macrolize(2)} local _d_{NonMacrolize(0)}=({end}-{beg})/({times}{offchar})\n";
                    repeat = $"{NonMacrolize(0)}={NonMacrolize(0)}+_d_{NonMacrolize(0)}\n";
                    break;
            }
            return new Tuple<string, string>(begin, repeat);
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }
    }
}

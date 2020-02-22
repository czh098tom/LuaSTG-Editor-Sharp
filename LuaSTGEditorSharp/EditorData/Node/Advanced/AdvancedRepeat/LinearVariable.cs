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
    [Serializable, NodeIcon("images/16x16/LinearVariable.png")]
    [RequireParent(typeof(VariableCollection))]
    [LeafNode]
    [CreateInvoke(0)]
    public class LinearVariable : VariableTransformation
    {
        [JsonConstructor]
        public LinearVariable() : base() { }

        public LinearVariable(DocumentData workSpaceData) : this(workSpaceData, "", "0", "0", "false") { }

        public LinearVariable(DocumentData workSpaceData, string name, string from, string to, string precisely)
            : base(workSpaceData)
        {
            Name = name;
            From = from;
            To = to;
            Precisely = precisely;
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

        public override object Clone()
        {
            var n = new LinearVariable(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            string offchar = Precisely == "true" ? "(Precisely)" : "(Expect next value IS)";
            return $"{NonMacrolize(0)} : {NonMacrolize(1)} => {NonMacrolize(2)} {offchar}";
        }

        public override Tuple<string, string> GetInformation(string times)
        {
            string offchar = Precisely == "true" ? "-1" : "";
            string beg = Macrolize(1);
            string end = Macrolize(2);
            string begin = $"local {NonMacrolize(0)}={beg}"
                + $" local _d_{NonMacrolize(0)}=({end}-({beg}))/({times}{offchar})\n";
            string repeat = $"{NonMacrolize(0)}={NonMacrolize(0)}+_d_{NonMacrolize(0)}\n";
            return new Tuple<string, string>(begin, repeat);
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

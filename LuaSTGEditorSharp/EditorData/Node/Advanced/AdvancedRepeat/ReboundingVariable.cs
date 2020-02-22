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
    [Serializable, NodeIcon("images/16x16/ReboundingVariable.png")]
    [RequireParent(typeof(VariableCollection))]
    [LeafNode]
    [CreateInvoke(0)]
    public class ReboundingVariable : VariableTransformation
    {
        [JsonConstructor]
        public ReboundingVariable() : base() { }

        public ReboundingVariable(DocumentData workSpaceData) : this(workSpaceData, "", "1", "-1") { }

        public ReboundingVariable(DocumentData workSpaceData, string name, string init, string inc)
            : base(workSpaceData)
        {
            Name = name;
            Init = init;
            Another = inc;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Init
        {
            get => DoubleCheckAttr(1, name: "Initial value").attrInput;
            set => DoubleCheckAttr(1, name: "Initial value").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Another
        {
            get => DoubleCheckAttr(2, name: "Another value").attrInput;
            set => DoubleCheckAttr(2, name: "Another value").attrInput = value;
        }

        public override object Clone()
        {
            var n = new ReboundingVariable(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            return $"{NonMacrolize(0)} : {NonMacrolize(1)}(Initial) <=> {NonMacrolize(2)}";
        }

        public override Tuple<string, string> GetInformation(string times)
        {
            string begin = $"local {NonMacrolize(0)}={Macrolize(1)}"
                + $" local _n_{NonMacrolize(0)}=({Macrolize(1)})+({Macrolize(2)})\n";
            string repeat = $"{NonMacrolize(0)}=-({NonMacrolize(0)})+_n_{NonMacrolize(0)}\n";
            return new Tuple<string, string>(begin, repeat);
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

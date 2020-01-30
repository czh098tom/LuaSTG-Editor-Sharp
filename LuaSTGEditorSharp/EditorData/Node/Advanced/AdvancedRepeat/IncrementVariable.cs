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
    public class IncrementVariable : VariableTransformation
    {
        [JsonConstructor]
        public IncrementVariable() : base() { }

        public IncrementVariable(DocumentData workSpaceData) : this(workSpaceData, "", "0", "0") { }

        public IncrementVariable(DocumentData workSpaceData, string name, string init, string inc)
            : base(workSpaceData)
        {
            Name = name;
            Init = init;
            Increment = inc;
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
        public string Increment
        {
            get => DoubleCheckAttr(2).attrInput;
            set => DoubleCheckAttr(2).attrInput = value;
        }

        public override object Clone()
        {
            var n = new IncrementVariable(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            return $"{NonMacrolize(0)} : {NonMacrolize(1)} , +({NonMacrolize(2)}) each loop";
        }

        public override Tuple<string, string> GetInformation(string times)
        {
            string begin = $"local {NonMacrolize(0)}={NonMacrolize(1)}"
                + $" local _d_{NonMacrolize(0)}=({Macrolize(2)})\n";
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

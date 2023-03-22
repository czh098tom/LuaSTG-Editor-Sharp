using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Data
{
    [Serializable, NodeIcon("CaptureVariable.png")]
    [CreateInvoke(0), RCInvoke(0)]
    public class CaptureVariable : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private CaptureVariable() : base() { }

        public CaptureVariable(DocumentData workSpaceData) : this(workSpaceData, "") { }

        public CaptureVariable(DocumentData workSpaceData, string variables) : base(workSpaceData)
        {
            Variables = variables;
        }

        [JsonIgnore, NodeAttribute]
        public string Variables
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string sp1 = Indent(spacing + 1);
            yield return $"{sp}do\n";
            yield return $"{sp1}local {Macrolize(0)} = {Macrolize(0)}\n";
            yield return $"{sp1}do\n";
            foreach (var child in base.ToLua(spacing + 2)) 
            {
                yield return child;
            }
            yield return $"{sp1}end\n";
            yield return $"{sp}end\n";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(3, this);
            foreach (Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNodeBase>(2, this);
        }

        public override string ToString()
        {
            return $"Capture Variable: {NonMacrolize(0)}";
        }

        public override object Clone()
        {
            var n = new CaptureVariable(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

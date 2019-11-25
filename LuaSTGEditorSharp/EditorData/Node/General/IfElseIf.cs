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

namespace LuaSTGEditorSharp.EditorData.Node.General
{
    [Serializable, NodeIcon("images/16x16/elseif.png")]
    [RequireParent(typeof(IfNode))]
    public class IfElseIf : TreeNode, IIfChild
    {
        [JsonConstructor]
        public IfElseIf() : base() { }

        public IfElseIf(DocumentData workSpaceData) : this(workSpaceData, "") { }

        public IfElseIf(DocumentData workSpaceData, string cond) : base(workSpaceData)
        {
            Condition = cond;
        }

        [JsonIgnore, NodeAttribute]
        public string Condition
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "elseif " + Macrolize(0) + " then\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
        }

        public override string ToString()
        {
            return "else if " + NonMacrolize(0);
        }

        public override object Clone()
        {
            var n = new IfElseIf(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        [JsonIgnore]
        public int Priority
        {
            get => 0;
        }
    }
}

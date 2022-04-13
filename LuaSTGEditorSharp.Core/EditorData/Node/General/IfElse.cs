using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.General
{
    [Serializable, NodeIcon("else.png")]
    [RequireParent(typeof(IfNode)), Uniqueness]
    public class IfElse : FixedAttributeTreeNode, IIfChild
    {
        [JsonConstructor]
        private IfElse() : base() { }

        public IfElse(DocumentData workSpaceData)
            : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "else\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
            foreach (Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override string ToString()
        {
            return "else";
        }

        public override object Clone()
        {
            var n = new IfElse(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        [JsonIgnore]
        public int Priority => 1;
    }
}

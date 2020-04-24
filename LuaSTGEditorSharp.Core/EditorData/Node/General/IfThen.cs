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
    [Serializable, NodeIcon("then.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(IfNode)), Uniqueness]
    public class IfThen : TreeNode, IIfChild
    {
        [JsonConstructor]
        private IfThen() : base() { }

        public IfThen(DocumentData workSpaceData)
            : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            yield return " then\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override string ToString()
        {
            return "then";
        }

        public override object Clone()
        {
            var n = new IfThen(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        [JsonIgnore]
        public int Priority => -1;
    }
}

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
    [Serializable, NodeIcon("images/16x16/callbackfunc.png")]
    [RequireParent(typeof(AdvancedRepeat))]
    [Uniqueness]
    [CannotDelete, CannotBan]
    public class VariableCollection : TreeNode
    {
        [JsonConstructor]
        public VariableCollection() : base() { }

        public VariableCollection(DocumentData workSpaceData) : base(workSpaceData) { }

        public override object Clone()
        {
            var n = new VariableCollection(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            return "Variables";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield break;
        }

        public IEnumerable<VariableTransformation> GetVariableTransformations()
        {
            foreach(TreeNode t in GetLogicalChildren())
            {
                if (t is VariableTransformation && !t.IsBanned) yield return t as VariableTransformation;
            }
        }
    }
}

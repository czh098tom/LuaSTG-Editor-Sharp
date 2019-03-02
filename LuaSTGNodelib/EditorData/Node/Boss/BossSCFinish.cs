using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bossscstart.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(BossSpellCard)), Uniqueness]
    public class BossSCFinish : TreeNode
    {
        [JsonConstructor]
        private BossSCFinish() { }

        public BossSCFinish(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "function _tmp_sc:del()\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "on finish";
        }

        public override object Clone()
        {
            var n = new BossSCFinish(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

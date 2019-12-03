using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Message;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bossexplode.png")]
    [RequireAncestor(typeof(BossAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class BossExplode : TreeNode
    {
        [JsonConstructor]
        private BossExplode() : base() { }

        public BossExplode(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "boss.explode(self,true)\n";
        }

        public override string ToString()
        {
            return "Boss explode";
        }

        public override object Clone()
        {
            var n = new BossExplode(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

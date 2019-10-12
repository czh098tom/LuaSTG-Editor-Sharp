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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bosscast.png")]
    [RequireAncestor(typeof(BossAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class BossCast : TreeNode
    {
        [JsonConstructor]
        private BossCast() : base() { }

        public BossCast(DocumentData workSpaceData) : this(workSpaceData, "60") { }

        public BossCast(DocumentData workSpaceData, string time) : base(workSpaceData)
        {
            Time = time;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "boss.cast(self," + Macrolize(0) + ")\n";
        }

        public override string ToString()
        {
            return "Play cast animation in " + NonMacrolize(0) + " frame(s)";
        }

        public override object Clone()
        {
            var n = new BossCast(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

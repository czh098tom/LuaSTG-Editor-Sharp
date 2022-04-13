using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("bossshowaura.png")]
    [RequireAncestor(typeof(BossAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class BossAura : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public BossAura() : base() { }

        public BossAura(DocumentData workSpaceData) : this(workSpaceData, "false") { }

        public BossAura(DocumentData workSpaceData, string showHideBossAura) : base(workSpaceData)
        {
            ShowHideBossAura = showHideBossAura;
        }

        [JsonIgnore, NodeAttribute]
        public string ShowHideBossAura
        {
            get => DoubleCheckAttr(0, "bool", "Show/Hide Aura").attrInput;
            set => DoubleCheckAttr(0, "bool", "Show/Hide Aura").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "boss.show_aura(self," + Macrolize(0) + ")\n";
        }

        public override string ToString()
        {
            if (NonMacrolize(0) == "true") return "Show aura";
            else if (NonMacrolize(0) == "false") return "Hide aura";
            return "Show/Hide aura";
        }

        public override object Clone()
        {
            var n = new BossAura(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }
    }
}

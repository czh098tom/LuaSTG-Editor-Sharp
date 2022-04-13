using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("bulletinit.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(BossDefine)), Uniqueness]
    public class BossInit : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private BossInit() : base() { }

        public BossInit(DocumentData workSpaceData)
            : this(workSpaceData, "240,384", "") { }

        public BossInit(DocumentData workSpaceData, string pos, string scbg)
            : base(workSpaceData)
        {
            Position = pos;
            SCBG = scbg;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(0, "position").attrInput;
            set => DoubleCheckAttr(0, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string SCBG
        {
            get => DoubleCheckAttr(1, "bossBG", "Spell Card Background").attrInput;
            set => DoubleCheckAttr(1, "bossBG", "Spell Card Background").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(spacing);
            TreeNodeBase parent = GetLogicalParent();
            string parentName = DefinitionWithDifficulty.GetNameWithDifficulty(parent);
            string parentStr = "_editor_class[\"" + parentName + "\"]";
            string scbg = Macrolize(1);
            scbg = string.IsNullOrEmpty(scbg) || scbg == "\"\"" ? "spellcard_background" : "_editor_class[" + scbg + "]";
            yield return sp + parentStr + ".init=function(self,cards)\n"
                         + sp + s1 + "boss.init(self," + Macrolize(0) + "," + parentStr + ".name,cards,New(" + scbg + ")," 
                         + parentStr + ".difficulty)\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(2, this);
            foreach(Tuple<int,TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "on init()";
        }

        public override object Clone()
        {
            var n = new BossInit(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            a.AddRange(DefinitionWithDifficulty.PopulateMessageOfFinding(GetLogicalParent(), this));
            return a;
        }
    }
}

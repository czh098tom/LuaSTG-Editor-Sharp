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
    [Serializable, NodeIcon("bosssetui.png")]
    [RequireAncestor(typeof(BossAlikeTypes))]
    [LeafNode]
    public class BossUI : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public BossUI() : base() { }

        public BossUI(DocumentData workSpaceData) : this(workSpaceData, "true", "true", "true", "true", "true") { }

        public BossUI(DocumentData workSpaceData, string showhpcircle, string showbossname
            , string showtimecounter, string showspellname, string showptr) : base(workSpaceData)
        {
            ShowHPCircle = showhpcircle;
            ShowBossName = showbossname;
            ShowTimeCounter = showtimecounter;
            ShowSpellName = showspellname;
            ShowPtr = showptr;
        }

        [JsonIgnore, NodeAttribute]
        public string ShowHPCircle
        {
            get => DoubleCheckAttr(0, "bool", "Show HP Circle").attrInput;
            set => DoubleCheckAttr(0, "bool", "Show HP Circle").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ShowBossName
        {
            get => DoubleCheckAttr(1, "bool", "Show Boss Name").attrInput;
            set => DoubleCheckAttr(1, "bool", "Show Boss Name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ShowTimeCounter
        {
            get => DoubleCheckAttr(2, "bool", "Show Time Counter").attrInput;
            set => DoubleCheckAttr(2, "bool", "Show Time Counter").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ShowSpellName
        {
            get => DoubleCheckAttr(3, "bool", "Show Spell Name").attrInput;
            set => DoubleCheckAttr(3, "bool", "Show Spell Name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ShowPtr
        {
            get => DoubleCheckAttr(4, "bool", "Show Boss Pointer").attrInput;
            set => DoubleCheckAttr(4, "bool", "Show Boss Pointer").attrInput = value;
        }


        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "boss.SetUIDisplay(self," + Macrolize(0) + ","
                + Macrolize(1) + "," + Macrolize(2) + "," + Macrolize(3) + ",true," + Macrolize(4) + ")\n";
        }

        public override string ToString()
        {
            return "Set boss UI display";
        }

        public override object Clone()
        {
            var n = new BossUI(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }
    }
}

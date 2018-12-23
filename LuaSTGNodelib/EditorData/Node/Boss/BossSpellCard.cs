using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bossspellcard.png")]
    [RequireParent(typeof(BossDefine))]
    [CreateInvoke(0), RCInvoke(4)]
    public class BossSpellCard : TreeNode
    {
        [JsonConstructor]
        private BossSpellCard() : base() { }

        public BossSpellCard(DocumentData workSpaceData)
            : this(workSpaceData, "", "2", "15", "60", "600", "0", "0", "0", "false", "false", "") { }

        public BossSpellCard(DocumentData workSpaceData, string name, string protectT, string reductT, string totalT
            , string HP, string power, string faith, string point, string bombImmune, string perform, string OID)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Name", name, this, "SCName"));
            attributes.Add(new AttrItem("Protect time (sec)", protectT, this));
            attributes.Add(new AttrItem("DMG redu. time (sec)", reductT, this));
            attributes.Add(new AttrItem("Total time (sec)", totalT, this));
            attributes.Add(new AttrItem("Hit point", HP, this));
            attributes.Add(new AttrItem("Drop power", power, this));
            attributes.Add(new AttrItem("Drop faith", faith, this));
            attributes.Add(new AttrItem("Drop point", point, this));
            attributes.Add(new AttrItem("Immune to bomb", bombImmune, this, "bool"));
            attributes.Add(new AttrItem("Performing action", perform, this, "bool"));
            attributes.Add(new AttrItem("Optional ID", OID, this));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string tmpD = Lua.StringParser.ParseLua(NonMacrolize(Parent.attributes[1]));
            string difficultyS = tmpD == "All" ? "" : ":" + tmpD;
            string className = "\"" + Lua.StringParser.ParseLua(NonMacrolize(Parent.attributes[0]) + difficultyS) + "\"";
            string cardName = Lua.StringParser.ParseLua(NonMacrolize(0));
            string a = "";
            if(!string.IsNullOrEmpty(Macrolize(10)))
            {
                a = "_editor_cards." + Macrolize(10) + "=_tmp_sc\n";
            }
            yield return "_tmp_sc=boss.card.New(\"" + cardName + "\"," + Macrolize(1) + "," + Macrolize(2) + "," + Macrolize(3) + ","
                       + Macrolize(4) + ",{" + Macrolize(5) + "," + Macrolize(6) + "," + Macrolize(7) + "}," + Macrolize(8) + ")\n";
            foreach (var m in base.ToLua(spacing))
            {
                yield return m;
            }
            yield return a + "table.insert(_editor_class[" + className + "].cards,_tmp_sc)\n"
                      + (!string.IsNullOrEmpty(NonMacrolize(0)) ? "table.insert(_sc_table,{" + className + ",\"" + cardName
                      + "\",_tmp_sc,#_editor_class[" + className + "].cards," + Macrolize(9) + "})\n" : "\n");
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            int i = 2;
            if (!string.IsNullOrEmpty(Macrolize(10))) i++;
            yield return new Tuple<int, TreeNode>(i, this);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(NonMacrolize(0)))
            {
                return "Non-spell card";
            }
            else
            {
                return "Spell Card \"" + NonMacrolize(0) + "\"";
            }
        }

        public override object Clone()
        {
            var n = new BossSpellCard(parentWorkSpace)
            {
                attributes = new ObservableCollection<AttrItem>(from AttrItem a in attributes select (AttrItem)a.Clone()),
                Children = new ObservableCollection<TreeNode>(from TreeNode t in Children select (TreeNode)t.Clone()),
                _parent = _parent,
                isExpanded = isExpanded
            };
            n.FixAttrParent();
            n.FixChildrenParent();
            return n;
        }
    }
}

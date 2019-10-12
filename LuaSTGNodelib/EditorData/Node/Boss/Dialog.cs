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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/dialog.png")]
    [RequireParent(typeof(BossDefine))]
    public class Dialog : TreeNode
    {
        [JsonConstructor]
        public Dialog() : base() { }

        public Dialog(DocumentData workSpaceData) : this(workSpaceData, "true", "true") { }

        public Dialog(DocumentData workSpaceData, string canskip, string suspend) : base(workSpaceData)
        {
            CanSkip = canskip;
            SuspendGameplay = suspend;
        }

        [JsonIgnore, NodeAttribute]
        public string CanSkip
        {
            get => DoubleCheckAttr(0, "bool", "Can skip").attrInput;
            set => DoubleCheckAttr(0, "bool", "Can skip").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string SuspendGameplay
        {
            get => DoubleCheckAttr(1, "bool", "Suspend gameplay").attrInput;
            set => DoubleCheckAttr(1, "bool", "Suspend gameplay").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            if (Parent?.attributes != null && Parent.AttributeCount >= 2)
            {
                parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0) +
                    (Parent.NonMacrolize(1) == "All" ? "" : ":" + Parent.NonMacrolize(1)));
            }
            yield return "_tmp_sc=boss.dialog.New(" + Macrolize(0) + ")\n";
            yield return "function _tmp_sc:init()\n";
            yield return "    lstg.player.dialog=" + Macrolize(1) + "\n";
            yield return "    _dialog_can_skip=" + Macrolize(0) + "\n";
            yield return "    self.dialog_displayer=New(dialog_displayer)\n";
            foreach (string s in base.ToLua(spacing + 1))
            {
                yield return s;
            }
            yield return "end\n";
            yield return "table.insert(_editor_class[\"" + parentName + "\"].cards,_tmp_sc)\n";
        }

        public override string ToString()
        {
            return "Dialog";
        }

        public override object Clone()
        {
            var n = new Dialog(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(5, this);
            foreach(Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(2, this);
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            TreeNode p = GetLogicalParent();
            if (p?.attributes == null || p.AttributeCount < 2)
            {
                a.Add(new CannotFindAttributeInParent(2, this));
            }
            return a;
        }
    }
}

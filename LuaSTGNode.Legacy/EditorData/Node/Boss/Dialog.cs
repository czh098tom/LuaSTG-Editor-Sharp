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
    [Serializable, NodeIcon("dialog.png")]
    [RequireParent(typeof(BossDefine))]
    public class Dialog : FixedAttributeTreeNode
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
            string sp = Indent(spacing);
            string s1 = Indent(1);
            TreeNodeBase parent = GetLogicalParent();
            string parentName = DefinitionWithDifficulty.GetNameWithDifficulty(parent);
            yield return sp + "_tmp_sc=boss.dialog.New(" + Macrolize(0) + ")\n";
            yield return sp + "function _tmp_sc:init()\n";
            yield return sp + s1 + "lstg.player.dialog=" + Macrolize(1) + "\n";
            yield return sp + s1 + "_dialog_can_skip=" + Macrolize(0) + "\n";
            yield return sp + s1 + "self.dialog_displayer=New(dialog_displayer)\n";
            foreach (string s in base.ToLua(spacing + 1))
            {
                yield return s;
            }
            yield return sp + "end\n";
            yield return sp + "table.insert(_editor_class[\"" + parentName + "\"].cards,_tmp_sc)\n";
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

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(5, this);
            foreach(Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNodeBase>(2, this);
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            a.AddRange(DefinitionWithDifficulty.PopulateMessageOfFinding(GetLogicalParent(), this));
            return a;
        }
    }
}

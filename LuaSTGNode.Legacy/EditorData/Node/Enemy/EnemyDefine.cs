using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Enemy
{
    [Serializable, NodeIcon("enemydefine.png")]
    [ClassNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class EnemyDefine : DefinitionWithDifficulty
    {
        [JsonConstructor]
        public EnemyDefine() : base() { }

        public EnemyDefine(DocumentData workSpaceData) : this(workSpaceData, "", "All") { }

        public EnemyDefine(DocumentData workSpaceData, string name, string difficulty) : base(workSpaceData)
        {
            Name = name;
            Difficulty = difficulty;
        }

        public override object Clone()
        {
            var n = new EnemyDefine(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "_editor_class[\"" + GetParsedNameWithDifficulty() + "\"]=Class(enemy)\n";
            foreach (var a in base.ToLua(spacing))
            {
                yield return a;
            }
        }

        public override string ToString()
        {
            return "Define enemy type \"" + GetNameWithDifficulty() + "\"";
        }

        public override MetaInfo GetMeta()
        {
            return new EnemyDefineMetaInfo(this);
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
            foreach (Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (string.IsNullOrEmpty(NonMacrolize(0)))
                messages.Add(new ArgNotNullMessage(attributes[0].AttrCap, 0, this));
            return messages;
        }
    }
}

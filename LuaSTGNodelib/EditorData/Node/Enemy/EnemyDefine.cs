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
    public class EnemyDefine : TreeNode
    {
        [JsonConstructor]
        public EnemyDefine() : base() { }

        public EnemyDefine(DocumentData workSpaceData) : this(workSpaceData, "", "All") { }

        public EnemyDefine(DocumentData workSpaceData, string name, string difficulty) : base(workSpaceData)
        {
            Name = name;
            Difficulty = difficulty;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Difficulty
        {
            get => DoubleCheckAttr(1, "objDifficulty").attrInput;
            set => DoubleCheckAttr(1, "objDifficulty").attrInput = value;
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
            string difficultyS = NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1);
            yield return sp + "_editor_class[\"" + Lua.StringParser.ParseLua(NonMacrolize(0) + difficultyS) + "\"]=Class(enemy)\n";
            foreach (var a in base.ToLua(spacing))
            {
                yield return a;
            }
        }

        public override string ToString()
        {
            string difficultyS = NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1);
            return "Define enemy type \"" + NonMacrolize(0) + difficultyS + "\"";
        }

        public override MetaInfo GetMeta()
        {
            return new EnemyDefineMetaInfo(this);
        }

        public override string GetDifficulty()
        {
            return NonMacrolize(1) == "All" ? "" : NonMacrolize(1);
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Laser
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/laserbentdefine.png")]
    [ClassNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class BentLaserDefine : TreeNode
    {
        [JsonConstructor]
        private BentLaserDefine() : base() { }

        public BentLaserDefine(DocumentData workSpaceData)
            : this(workSpaceData, "", "All") { }

        public BentLaserDefine(DocumentData workSpaceData, string name, string difficulty)
            : base(workSpaceData)
        {
            Name = name;
            Difficulty = difficulty;
            /*
            attributes.Add(new AttrItem("Name", name, this));
            attributes.Add(new AttrItem("Difficulty", difficulty, this, "objDifficulty"));
            */
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

        public override IEnumerable<string> ToLua(int spacing)
        {
            string difficultyS = NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1);
            yield return "_editor_class[\"" + Lua.StringParser.ParseLua(NonMacrolize(0) + difficultyS) + "\"]=Class(laser_bent)\n";
            foreach (var a in base.ToLua(spacing))
            {
                yield return a;
            }
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override string ToString()
        {
            string difficultyS = NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1);
            return "Define bent laser type\"" + NonMacrolize(0) + difficultyS + "\"";
        }

        public override object Clone()
        {
            var n = new BentLaserDefine(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override MetaInfo GetMeta()
        {
            return new BentLaserDefineMetaInfo(this);
        }

        public override string GetDifficulty()
        {
            return NonMacrolize(1) == "All" ? "" : NonMacrolize(1);
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

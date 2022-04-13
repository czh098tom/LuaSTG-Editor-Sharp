using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Message;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public abstract class DefinitionWithDifficulty : FixedAttributeTreeNode
    {
        public static bool HasProperties(TreeNodeBase treeNode)
        {
            return treeNode.HasPreferredProperty(0, "Name") && treeNode.HasPreferredProperty(1, "Difficulty");
        }

        public static string GetNameWithDifficulty(TreeNodeBase node)
        {
            if (node != null && HasProperties(node))
            {
                string diff = node.PreferredNonMacrolize(1, "Difficulty");
                return Lua.StringParser.ParseLua(node.PreferredNonMacrolize(0, "Name") +
                   (diff == "All" ? "" : ":" + diff));
            }
            else
            {
                return "";
            }
        }

        public static IEnumerable<MessageBase> PopulateMessageOfFinding(TreeNodeBase p, TreeNodeBase caller)
        {
            List<MessageBase> m = new List<MessageBase>(1);
            if (p == null || !HasProperties(p))
            {
                m.Add(new CannotFindAttributeInParent(2, caller));
            }
            return m;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Name")]
        //[DefaultValue("")]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Difficulty")]
        //[DefaultValue("All")]
        public string Difficulty
        {
            get => DoubleCheckAttr(1, "objDifficulty").attrInput;
            set => DoubleCheckAttr(1, "objDifficulty").attrInput = value;
        }

        protected DefinitionWithDifficulty() : base() { }
        protected DefinitionWithDifficulty(DocumentData parent) : base(parent) { }

        public override sealed string GetDifficulty()
        {
            return NonMacrolize(1) == "All" ? "" : NonMacrolize(1);
        }

        protected string GetNameWithDifficulty()
        {
            return NonMacrolize(0) + (NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1));
        }

        protected string GetParsedNameWithDifficulty()
        {
            return Lua.StringParser.ParseLua(GetNameWithDifficulty());
        }
    }
}

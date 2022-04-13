using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("bgdefine.png")]
    [ClassNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class BossBGDefine : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private BossBGDefine() : base() { }

        public BossBGDefine(DocumentData workSpaceData)
            : this(workSpaceData, "") { }

        public BossBGDefine(DocumentData workSpaceData, string name)
            : base(workSpaceData)
        {
            //attributes.Add(new AttrItem("Name", name, this));
            Name = name;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override string ToString()
        {
            return "Define boss spell card background \"" + NonMacrolize(0) + "\"";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            string luaStrName = Lua.StringParser.ParseLua(NonMacrolize(0));
            yield return sp + "_editor_class[\"" + luaStrName + "\"]=Class(_spellcard_background)\n" 
                + sp + "_editor_class[\"" + luaStrName + "\"].init=function(self)\n"
                + sp + s1 + "_spellcard_background.init(self)\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(3, this);
            foreach (Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override object Clone()
        {
            var n = new BossBGDefine(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override MetaInfo GetMeta()
        {
            return new BossBGDefineMetaInfo(this);
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

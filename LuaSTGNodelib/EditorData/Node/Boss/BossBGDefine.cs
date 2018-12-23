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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bgdefine.png")]
    [ClassNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class BossBGDefine : TreeNode
    {
        [JsonConstructor]
        private BossBGDefine() : base() { }

        public BossBGDefine(DocumentData workSpaceData)
            : this(workSpaceData, "") { }

        public BossBGDefine(DocumentData workSpaceData, string name)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Name", name, this));
        }

        public override string ToString()
        {
            return "Define boss spell card background \"" + NonMacrolize(0) + "\"";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string luaStrName = Lua.StringParser.ParseLua(NonMacrolize(0));
            yield return "_editor_class[\"" + luaStrName + "\"]=Class(_spellcard_background)\n_editor_class[\""
                + luaStrName + "\"].init=function(self)\n    _spellcard_background.init(self)\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(3, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override object Clone()
        {
            var n = new BossBGDefine(parentWorkSpace)
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

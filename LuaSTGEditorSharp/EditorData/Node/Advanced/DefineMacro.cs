using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Compile;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Advanced
{
    [Serializable, NodeIcon("images/16x16/definemacro.png")]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class DefineMacro : TreeNode
    {
        [JsonConstructor]
        private DefineMacro() : base() { }

        public DefineMacro(DocumentData workSpaceData) : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Replace", this));
            attributes.Add(new AttrItem("By", this));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "-- #define " + NonMacrolize(0) + " " + NonMacrolize(1) + sp + "\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Macro: Replace " + attributes[0].AttrInput + " by " + attributes[1].AttrInput + " in compile process";
        }

        protected override void AddCompileSettings()
        {
            parentWorkSpace.CompileProcess.marcoDefination.Add(
                new DefineMarco() { ToBeReplaced = NonMacrolize(0)
                , New = Macrolize(1) });
        }

        public override object Clone()
        {
            var n = new DefineMacro(parentWorkSpace)
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

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (string.IsNullOrEmpty(NonMacrolize(0)))
                messages.Add(new ArgNotNullMessage(attributes[0].AttrCap, 0, this));
            return messages;
        }
    }
}

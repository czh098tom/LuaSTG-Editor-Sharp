using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("dropitem.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    class DropItem : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private DropItem() : base() { }

        public DropItem(DocumentData workSpaceData)
            : this(workSpaceData, "item_extend", "1", "self.x,self.y") { }

        public DropItem(DocumentData workSpaceData, string item, string number, string position)
            : base(workSpaceData)
        {
            Item = item;
            Number = number;
            Position = position;
        }

        [JsonIgnore, NodeAttribute]
        public string Item
        {
            get => DoubleCheckAttr(0, "item").attrInput;
            set => DoubleCheckAttr(0, "item").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Number
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(2, "position").attrInput;
            set => DoubleCheckAttr(2, "position").attrInput = value;
        }

        public override string ToString()
        {
            return "Drop " + NonMacrolize(1) + " " + NonMacrolize(0) + "(s) in (" + NonMacrolize(2) + ")";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"_drop_item({Macrolize(0)},{Macrolize(1)},{Macrolize(2)})\n";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override object Clone()
        {
            var n = new DropItem(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
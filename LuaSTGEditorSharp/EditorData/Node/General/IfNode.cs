using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.General
{
    [Serializable, NodeIcon("images/16x16/if.png")]
    [RCInvoke(0)]
    public class IfNode : TreeNode
    {
        [JsonConstructor]
        private IfNode() : base() { }

        public IfNode(DocumentData workSpaceData)
            : this(workSpaceData, "") { }

        public IfNode(DocumentData workSpaceData, string code)
            : base(workSpaceData)
        {
            //attributes.Add(new AttrItem("Condition", this) { AttrInput = code });
            Condition = code;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Condition")]
        public string Condition
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public IEnumerable<string> BaseToLua(int spacing, IEnumerable<TreeNode> children)
        {
            return base.ToLua(spacing, children);
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            var i = GetLogicalChildren().OrderBy((s) => (s as IIfChild)?.Priority ?? 0);
            List<TreeNode> t = new List<TreeNode>(i);

            yield return sp + "if " + Macrolize(0);
            foreach (var a in BaseToLua(spacing + 1, i))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "if (" + NonMacrolize(0) + ")";
        }

        public override object Clone()
        {
            var n = new IfNode(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

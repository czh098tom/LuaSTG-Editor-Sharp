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

        [JsonIgnore, XmlAttribute("Condition")]
        //[DefaultValue("")]
        public string Condition
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            if (Children[0].GetType() != typeof(IfThen))
            {
                IfNode dup = Clone() as IfNode;
                TreeNode tmp = dup.Children[0];
                dup.Children[0] = dup.Children[1];
                dup.Children[1] = tmp;
                foreach (var a in dup.ToLua(spacing))
                {
                    yield return a;
                }
            }
            else
            {
                yield return sp + "if " + Macrolize(0);
                foreach (var a in base.ToLua(spacing))
                {
                    yield return a;
                }
                yield return sp + "end\n";
            }
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

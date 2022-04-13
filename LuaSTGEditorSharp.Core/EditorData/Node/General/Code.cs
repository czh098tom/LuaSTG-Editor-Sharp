using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.General
{
    [Serializable, NodeIcon("code.png")]
    [LeafNode]
    [RCInvoke(0)]
    public class Code : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private Code() : base() { }

        public Code(DocumentData workSpaceData)
            : this(workSpaceData, "") { }

        public Code(DocumentData workSpaceData, string code) : base(workSpaceData)
        {
            //attributes.Add(new AttrItem("Code", this, "code") { AttrInput = code });
            CodeContent = code;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Code")]
        public string CodeContent
        {
            get => DoubleCheckAttr(0, "code", "Code").attrInput;
            set => DoubleCheckAttr(0, "code", "Code").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            Regex r = new Regex("\\n");
            string sp = Indent(spacing);
            string nsp = "\n" + sp;
            yield return sp + r.Replace(Macrolize(0), nsp) + "\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            string s = Macrolize(0);
            int i = 1;
            foreach(char c in s)
            {
                if (c == '\n') i++;
            }
            yield return new Tuple<int, TreeNodeBase>(i, this);
        }

        public override string ToString()
        {
            return attributes[0].AttrInput;
        }

        public override object Clone()
        {
            var n = new Code(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

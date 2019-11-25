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
    [Serializable, NodeIcon("images/16x16/codesegment.png")]
    [IgnoreValidation]
    [RCInvoke(0)]
    public class CodeSegment : TreeNode
    {
        [JsonConstructor]
        private CodeSegment() : base() { }

        public CodeSegment(DocumentData workSpaceData) 
            : this(workSpaceData, "do", "end") { }

        public CodeSegment(DocumentData workSpaceData, string head, string tail) 
            : base(workSpaceData)
        {
            /*
            attributes.Add(new AttrItem("Head", head, this, "code"));
            attributes.Add(new AttrItem("Tail", tail, this, "code"));
            */
            Head = head;
            Tail = tail;
        }

        [JsonIgnore, NodeAttribute("do"), XmlAttribute("Head")]
        public string Head
        {
            get => DoubleCheckAttr(0, "code").attrInput;
            set => DoubleCheckAttr(0, "code").attrInput = value;
        }

        [JsonIgnore, NodeAttribute("end"), XmlAttribute("Tail")]
        public string Tail
        {
            get => DoubleCheckAttr(1, "code").attrInput;
            set => DoubleCheckAttr(1, "code").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            Regex r = new Regex("\\n\\b");
            string sp = Indent(spacing);
            yield return sp + r.Replace(Macrolize(0), "\n" + sp) + "\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + r.Replace(Macrolize(1), "\n" + sp) + "\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            string s = Macrolize(0);
            int i = 1;
            foreach(char c in s)
            {
                if (c == '\n') i++;
            }
            yield return new Tuple<int, TreeNode>(i, this);
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            s = Macrolize(1);
            i = 1;
            foreach (char c in s)
            {
                if (c == '\n') i++;
            }
            yield return new Tuple<int, TreeNode>(i, this);
        }

        public override string ToString()
        {
            return attributes[0].AttrInput + "\n...";
        }

        public override object Clone()
        {
            var n = new CodeSegment(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

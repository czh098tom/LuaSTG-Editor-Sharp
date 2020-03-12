using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.General
{
    [Serializable, NodeIcon("codeblock.png")]
    public class CodeBlock : TreeNode
    {
        [JsonConstructor]
        public CodeBlock() : base() { }

        public CodeBlock(DocumentData workSpaceData) : this(workSpaceData, "") { }

        public CodeBlock(DocumentData workSpaceData, string name) : base(workSpaceData) 
        {
            Name = name;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override object Clone()
        {
            var n = new CodeBlock(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "do\n";
            foreach(var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override string ToString()
        {
            return NonMacrolize(0);
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach(var a in GetChildLines())
            {
                yield return a;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

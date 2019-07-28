using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.General
{
    [Serializable, NodeIcon("images/16x16/break.png")]
    [RequireAncestor(typeof(Repeat))]
    public class Break : TreeNode
    {
        [JsonConstructor]
        private Break() : base() { }

        public Break(DocumentData workSpaceData) : this(workSpaceData, "") { }

        public Break(DocumentData workSpaceData, string condition) : base(workSpaceData)
        {
            Condition = condition;
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
            string s = Macrolize(0);
            yield return string.IsNullOrEmpty(s) ? sp + "break\n" : sp + "if " + s + " then break end\n";
        }

        public override string ToString()
        {
            string s = NonMacrolize(0);
            return string.IsNullOrEmpty(s) ? "break" : "break if " + s;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override object Clone()
        {
            var n = new Break(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

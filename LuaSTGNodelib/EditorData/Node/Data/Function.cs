using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Data
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/func.png")]
    [CreateInvoke(0), RCInvoke(1)]
    public class Function : TreeNode
    {
        [JsonConstructor]
        private Function() : base() { }

        public Function(DocumentData workSpaceData)
            : this(workSpaceData, "", "", "true") { }

        public Function(DocumentData workSpaceData, string name, string param, string localized) : base(workSpaceData)
        {
            FuncName = name;
            Parameters = param;
            Localized = localized;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Name")]
        public string FuncName
        {
            get => DoubleCheckAttr(0, name: "Name").attrInput;
            set => DoubleCheckAttr(0, name: "Name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Parameters")]
        public string Parameters
        {
            get => DoubleCheckAttr(1, name: "Parameter List").attrInput;
            set => DoubleCheckAttr(1, name: "Parameter List").attrInput = value;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Localized")]
        public string Localized
        {
            get => DoubleCheckAttr(2, "bool").attrInput;
            set => DoubleCheckAttr(2, "bool").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp;
            if(bool.TryParse(NonMacrolize(2), out bool local))
            {
                if(local) yield return "local ";
            }
            yield return NonMacrolize(0);
            yield return " = function(";
            yield return NonMacrolize(1);
            yield return ")\n";
            yield return sp;
            foreach(string s in base.ToLua(spacing + 1))
            {
                yield return s;
            }
            yield return sp;
            yield return "end\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach(Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            string s = "define";
            if (bool.TryParse(NonMacrolize(2), out bool local))
            {
                if (local)
                {
                    s += " local";
                }
            }
            return s + " function " + NonMacrolize(0) + "(" + NonMacrolize(1) + ")";
        }

        public override object Clone()
        {
            TreeNode t = new Function(parentWorkSpace);
            t.DeepCopyFrom(this);
            return t;
        }
    }
}

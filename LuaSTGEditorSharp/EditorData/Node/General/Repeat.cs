using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.General
{
    [Serializable, NodeIcon("images/16x16/repeat.png")]
    [CreateInvoke(0), RCInvoke(0)]
    [IgnoreAttributesParityCheck]
    public class Repeat : TreeNode
    {
        [JsonConstructor]
        private Repeat() : base() { }

        public Repeat(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Times", this, "yield"));
            attributes.Add(new DependencyAttrItem("Number of Var", "1", this));
            attributes.Add(new AttrItem("Var 1 name", this));
            attributes.Add(new AttrItem("Var 1 init value", this));
            attributes.Add(new AttrItem("Var 1 increment", this));
        }

        public Repeat(DocumentData workSpaceData, string code)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Times", this, "yield") { AttrInput = code });
            attributes.Add(new DependencyAttrItem("Number of Var", "1", this));
            attributes.Add(new AttrItem("Var 1 name", this));
            attributes.Add(new AttrItem("Var 1 init value", this));
            attributes.Add(new AttrItem("Var 1 increment", this));
        }

        [JsonIgnore, XmlAttribute("RepeatTimes")]
        //[DefaultValue("_infinity")]
        public string RepeatTimes
        {
            get => DoubleCheckAttr(0, "yield", "Times").attrInput;
            set => DoubleCheckAttr(0, "yield", "Times").attrInput = value;
        }

        [JsonIgnore, XmlAttribute("NumOfVar")]
        //[DefaultValue("1")]
        public string NumOfVar
        {
            get => DoubleCheckAttr(1, name: "Number of Var", isDependency: true).attrInput;
            set => DoubleCheckAttr(1, name: "Number of Var", isDependency: true).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            string bres = "";
            string mres = "";
            string eres = "";
            bool first = true;
            if (!int.TryParse(attributes[1].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > App.mxUAttr ? App.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 2; i <= nAttr * 3 - 1; i += 3) 
            {
                if (attributes[i].AttrInput != "")
                {
                    if (!first)
                    {
                        bres += ",";
                        mres += ",";
                        eres += " ";
                    }
                    bres += NonMacrolize(i) + ",_d_" + NonMacrolize(i);
                    mres += "(" + Macrolize(i + 1) + "),(" + Macrolize(i + 2) + ")";
                    eres += NonMacrolize(i) + "=" + NonMacrolize(i) + "+_d_" + NonMacrolize(i);
                    first = false;
                }
            }
            if(first)
            {
                yield return sp + "for _=1," + Macrolize(0) + " do\n";
                foreach (var a in base.ToLua(spacing + 1))
                {
                    yield return a;
                }
                yield return sp + "end\n";
            }
            else
            {
                yield return sp + "do local " + bres + "=" + mres + " for _=1," + Macrolize(0) + " do\n";
                foreach (var a in base.ToLua(spacing + 1))
                {
                    yield return a;
                }
                yield return sp + eres + " end end\n";
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
            string bres = "";
            bool first = true;
            if (!int.TryParse(attributes[1].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > App.mxUAttr ? App.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 2; i <= nAttr * 3 - 1; i += 3)
            {
                if (attributes[i].AttrInput != "")
                {
                    if (!first)
                    {
                        bres += ",";
                    }
                    bres += "(" + attributes[i].AttrInput + " = " + attributes[i + 1].AttrInput
                        + ", increment " + attributes[i + 2].AttrInput + ")";
                    first = false;
                }
            }
            if (first)
            {
                return "repeat " + attributes[0].AttrInput + " times";
            }
            else
            {
                return "repeat " + attributes[0].AttrInput + " times, " + bres;
            }
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs args)
        {
            int n = (attributes.Count - 2) / 3;
            if (!int.TryParse(attributes[1].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > App.mxUAttr ? App.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            if (n != nAttr)
            {
                if (n < nAttr)
                {
                    for (int i = n + 1; i <= nAttr; i++) 
                    {
                        attributes.Add(new AttrItem("Var " + i + " name", this));
                        attributes.Add(new AttrItem("Var " + i + " init value", this));
                        attributes.Add(new AttrItem("Var " + i + " increment", this));
                    }
                }
                else
                {
                    for (int i = 0; i < 3 * (n - nAttr); i++)
                    {
                        attributes.RemoveAt(nAttr * 3 + 2);
                    }
                }
            }
        }

        public override object Clone()
        {
            var n = new Repeat(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (!int.TryParse(NonMacrolize(1), out int nAttr)) nAttr = 0;
            if (nAttr > App.mxUAttr)
            {
                nAttr = App.mxUAttr;
            }
            for (int i = 2; i < 3 * nAttr; i += 3)
            {
                if (!string.IsNullOrEmpty(attributes[i].AttrInput))
                {
                    if (!CheckVarName(NonMacrolize(i))) messages.Add(new VarNameInvalidMessage(attributes[i].AttrCap, this));
                }
            }
            return messages;
        }
    }
}

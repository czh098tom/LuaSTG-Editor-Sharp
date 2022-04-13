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

namespace LuaSTGEditorSharp.EditorData.Node.Data
{
    [Serializable, NodeIcon("variable.png")]
    [LeafNode]
    [CreateInvoke(1), RCInvoke(2)]
    [IgnoreAttributesParityCheck]
    public class LocalVar : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private LocalVar() : base() { }

        public LocalVar(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            attributes.Add(new DependencyAttrItem("Number of Var", "1", this));
            attributes.Add(new AttrItem("Var 1 name", this));
            attributes.Add(new AttrItem("Var 1 init value", this, "target"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            if (!int.TryParse(attributes[0].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 1; i <= nAttr * 2 - 1; i += 2)
            {
                if (!string.IsNullOrEmpty(attributes[i].AttrInput))
                {
                    if (!string.IsNullOrEmpty(attributes[i + 1].AttrInput))
                    {
                        yield return sp + "local " + NonMacrolize(i) + "=(" + Macrolize(i + 1) + ")\n";
                    }
                    else
                    {
                        yield return sp + "local " + NonMacrolize(i) + "\n";
                    }
                }
            }
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            int count = 0;
            if (!int.TryParse(attributes[0].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 1; i <= nAttr * 2 - 1; i += 2)
            {
                if (!string.IsNullOrEmpty(attributes[i].AttrInput))
                {
                    count++;
                }
            }
            yield return new Tuple<int, TreeNodeBase>(count, this);
        }

        public override string ToString()
        {
            string bres = "local:";
            if (!int.TryParse(attributes[0].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 1; i <= nAttr * 2 - 1; i += 2)
            {
                if (attributes[i].AttrInput != "")
                {
                    bres += "\n" + NonMacrolize(i) + 
                        (string.IsNullOrEmpty(attributes[i + 1].AttrInput) ? "" : " = " + NonMacrolize(i + 1));
                }
            }
            return bres;
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs args)
        {
            int n = (attributes.Count - 1) / 2;
            if (!int.TryParse(NonMacrolize(0), out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            if (n != nAttr)
            {
                if (n < nAttr)
                {
                    for (int i = n + 1; i <= nAttr; i++) 
                    {
                        attributes.Add(new AttrItem("Var " + i + " name", this));
                        attributes.Add(new AttrItem("Var " + i + " init value", this, "target"));
                    }
                }
                else
                {
                    for (int i = 0; i < 2 * (n - nAttr); i++)
                    {
                        attributes.RemoveAt(nAttr * 2 + 1);
                    }
                }
            }
        }

        public override object Clone()
        {
            var n = new LocalVar(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (!int.TryParse(NonMacrolize(0), out int nAttr)) nAttr = 0;
            if (nAttr > AppConstants.mxUAttr)
            {
                messages.Add(new ArgCountInfo(attributes[0].AttrCap, true, this));
                nAttr = AppConstants.mxUAttr;
            }
            if (nAttr <= 0) messages.Add(new ArgCountInfo(attributes[0].AttrCap, false, this));
            for(int i = 1; i < 2 * nAttr; i += 2)
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

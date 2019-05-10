using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Advanced
{
    [Serializable, NodeIcon("images/16x16/userdefinednode.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class UserDefinedNode : TreeNode
    {
        [JsonConstructor]
        private UserDefinedNode() : base() { }

        public UserDefinedNode(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Name", this));
            attributes.Add(new AttrItem("Head parse rule", this, "code"));
            attributes.Add(new AttrItem("Tail parse rule", this, "code"));
            attributes.Add(new DependencyAttrItem("Number of prop", "1", this));
            attributes.Add(new AttrItem("Prop 1 name", this));
            attributes.Add(new AttrItem("Prop 1 is string", "false", this, "bool"));
            attributes.Add(new AttrItem("Prop 1 edit window", this, "editWindow"));
        }

        public override MetaInfo GetMeta()
        {
            return new UserDefinedNodeMetaInfo(this);
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "-- define node named: " + NonMacrolize(0) + "\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            string bres = "";
            bool first = true;
            if (!int.TryParse(attributes[3].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > App.mxUAttr ? App.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 4; i <= nAttr * 3 + 3; i += 3)
            {
                if (attributes[i].AttrInput != "")
                {
                    if (!first)
                    {
                        bres += ", ";
                    }
                    bres += attributes[i].AttrInput;
                    first = false;
                }
            }
            if (first)
            {
                return "Define node " + attributes[0].AttrInput;
            }
            else
            {
                return "Define node \"" + attributes[0].AttrInput + "\" with property (" + bres + ")";
            }
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs args)
        {
            int n = (attributes.Count - 4) / 3;
            if (!int.TryParse(attributes[3].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > App.mxUAttr ? App.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            if (n != nAttr)
            {
                if (n < nAttr)
                {
                    for (int i = n + 1; i <= nAttr; i++) 
                    {
                        attributes.Add(new AttrItem("Prop " + i + " name", this));
                        attributes.Add(new AttrItem("Prop " + i + " is string", "false", this, "bool"));
                        attributes.Add(new AttrItem("Prop " + i + " edit window", this, "editWindow"));
                    }
                }
                else
                {
                    for (int i = 0; i < 3 * (n - nAttr); i++)
                    {
                        attributes.RemoveAt(nAttr * 3 + 4);
                    }
                }
            }
        }

        public override object Clone()
        {
            var n = new UserDefinedNode(parentWorkSpace);
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
            return messages;
        }
    }
}

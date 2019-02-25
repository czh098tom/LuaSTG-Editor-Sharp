using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Data
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/assignment.png")]
    [LeafNode]
    [CreateInvoke(1), RCInvoke(2)]
    public class Assignment : TreeNode
    {
        [JsonConstructor]
        public Assignment() : base() { }

        public Assignment(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            attributes.Add(new DependencyAttrItem("Number of Var", "1", this));
            attributes.Add(new AttrItem("Var 1 name", this));
            attributes.Add(new AttrItem("Var 1 value", this, "target"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            string bres = "";
            string mres = "";
            bool first = true;
            if (!int.TryParse(attributes[0].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > App.mxUAttr ? App.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 1; i <= nAttr * 2 - 1; i += 2)
            {
                if (attributes[i].AttrInput != "")
                {
                    if (!first)
                    {
                        bres += ",";
                        mres += ",";
                    }
                    bres += NonMacrolize(i);
                    mres += "(" + Macrolize(i + 1) + ")";
                    first = false;
                }
            }
            if(!first)
            {
                yield return sp + bres + "=" + mres + "\n";
            }
            else
            {
                yield return sp + "\n";
            }
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            string bres = "Assign:";
            if (!int.TryParse(attributes[0].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > App.mxUAttr ? App.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 1; i <= nAttr * 2 - 1; i += 2)
            {
                if (attributes[i].AttrInput != "")
                {
                    bres += "\n" + NonMacrolize(i) + " = " + NonMacrolize(i + 1);
                }
            }
            return bres;
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs args)
        {
            int n = (attributes.Count - 1) / 2;
            if (!int.TryParse(NonMacrolize(0), out int nAttr)) nAttr = 0;
            nAttr = nAttr > App.mxUAttr ? App.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            if (n != nAttr)
            {
                if (n < nAttr)
                {
                    for (int i = n + 1; i <= nAttr; i++)
                    {
                        attributes.Add(new AttrItem("Var " + i + " name", this));
                        attributes.Add(new AttrItem("Var " + i + " value", this, "target"));
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
            var n = new Assignment(parentWorkSpace)
            {
                attributes = new ObservableCollection<AttrItem>(from AttrItem a in attributes select (AttrItem)a.Clone()),
                Children = new ObservableCollection<TreeNode>(from TreeNode t in Children select (TreeNode)t.Clone()),
                _parent = _parent,
                isExpanded = isExpanded
            };
            n.FixAttrParent();
            n.FixChildrenParent();
            return n;
        }
    }
}

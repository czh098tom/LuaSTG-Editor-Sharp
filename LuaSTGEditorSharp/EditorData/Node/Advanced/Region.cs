using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Advanced
{
    [Serializable, NodeIcon("images/16x16/region.png")]
    [LeafNode]
    [RCInvoke(0)]
    public class Region : TreeNode
    {
        [JsonConstructor]
        private Region() : base() { }

        public Region(DocumentData workSpaceData) 
            : base(workSpaceData) { attributes.Add(new AttrItem("Name", this)); }

        public Region(DocumentData workSpaceData, string code) 
            : base(workSpaceData) { attributes.Add(new AttrItem("Name", this) { AttrInput = code }); }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "-- #region " + NonMacrolize(0) + "\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Region: " + attributes[0].AttrInput;
        }

        public override object Clone()
        {
            var n = new Region(parentWorkSpace)
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

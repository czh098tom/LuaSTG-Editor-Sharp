using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Node.General;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Project
{
    [Serializable, NodeIcon("images/16x16/folder.png")]
    [CannotDelete]
    public class ProjectRoot : TreeNode
    { 
        [JsonConstructor]
        private ProjectRoot() :base() { }

        public ProjectRoot(DocumentData workSpaceData)
            : base(workSpaceData) { attributes.Add(new AttrItem("Name", this)); }
        public ProjectRoot(DocumentData workSpaceData, string name) 
            : base(workSpaceData) { attributes.Add(new AttrItem("Name", this) { AttrInput = name }); }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override string ToString()
        {
            return attributes[0].AttrInput;
        }

        public override object Clone()
        {
            var n = new Folder(parentWorkSpace)
            {
                attributes = new ObservableCollection<AttrItem>(from AttrItem a in attributes select (AttrItem)a.Clone()),
                Children = new ObservableCollection<TreeNode>(from TreeNode t in Children select (TreeNode)t.Clone()),
                isExpanded = isExpanded
            };
            n.FixAttrParent();
            n.FixChildrenParent();
            return n;
        }
    }
}

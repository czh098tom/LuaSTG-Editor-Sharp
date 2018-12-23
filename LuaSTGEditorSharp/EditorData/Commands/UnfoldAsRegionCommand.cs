using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Node.General;
using LuaSTGEditorSharp.EditorData.Node.Advanced;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    public class UnfoldAsRegionCommand : Command
    {
        private ObservableCollection<TreeNode> toAggregate;
        private Region regionBegin;
        private Region regionEnd;
        private TreeNode folderP;

        public UnfoldAsRegionCommand(TreeNode folder)
        {
            folderP = folder;
        }

        public override void Execute()
        {
            string name = "region";
            if (folderP.attributes.Count != 0) name = folderP.attributes[0].AttrInput;
            if (regionBegin == null) regionBegin = new Region(folderP.parentWorkSpace, name);
            if (regionEnd == null) regionEnd = new Region(folderP.parentWorkSpace, name);
            toAggregate = new ObservableCollection<TreeNode>(from TreeNode t in folderP.Children select t);
            TreeNode parent = folderP.Parent;
            int index = parent.Children.IndexOf(folderP);
            parent.InsertChild(regionEnd, index);
            parent.InsertChild(regionBegin, index);
            parent.RemoveChild(folderP);
            for (int i = 0; i < toAggregate.Count; i++)
            {
                parent.InsertChild(toAggregate[i], index + i + 1);
            }
        }

        public override void Undo()
        {
            TreeNode parent = regionBegin.Parent;
            foreach (TreeNode t in toAggregate) 
            {
                parent.RemoveChild(t);
            }
            int index = parent.Children.IndexOf(regionBegin);
            parent.RemoveChild(regionBegin);
            if (regionEnd != null) parent.RemoveChild(regionEnd);
            parent.InsertChild(folderP, index);
        }
    }
}

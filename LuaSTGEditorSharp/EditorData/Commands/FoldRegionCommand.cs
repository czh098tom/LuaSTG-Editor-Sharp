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
    public class FoldRegionCommand : Command
    {
        private readonly ObservableCollection<TreeNode> toAggregate;
        private readonly Region regionBegin;
        private readonly Region regionEnd;
        private Folder folderP;

        public FoldRegionCommand(ObservableCollection<TreeNode> treeNodes, Region begin, Region end)
        {
            toAggregate = treeNodes;
            regionBegin = begin;
            regionEnd = end;
        }

        public override void Execute()
        {
            TreeNode parent = regionBegin.Parent;
            bool folderPNotExist = folderP == null;
            if (folderPNotExist)
                folderP = new Folder(parent.parentWorkSpace, regionBegin.attributes[0].AttrInput);
            foreach (TreeNode t in toAggregate) 
            {
                if (folderPNotExist) folderP.AddChild(t);
                parent.RemoveChild(t);
            }
            int index = parent.Children.IndexOf(regionBegin);
            parent.RemoveChild(regionBegin);
            if (regionEnd != null) parent.RemoveChild(regionEnd);
            parent.InsertChild(folderP, index);
        }

        public override void Undo()
        {
            TreeNode parent = folderP.Parent;
            int index = parent.Children.IndexOf(folderP);
            if (regionEnd != null) parent.InsertChild(regionEnd, index);
            parent.InsertChild(regionBegin, index);
            parent.RemoveChild(folderP);
            for (int i = 0; i < toAggregate.Count; i++)
            {
                parent.InsertChild(toAggregate[i], index + i + 1);
            }
        }
    }
}

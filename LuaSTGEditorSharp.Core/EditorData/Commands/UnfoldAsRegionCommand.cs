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
    /// <summary>
    /// <see cref="Command"/> that unfold a <see cref="TreeNode"/> with child to a region.
    /// </summary>
    public class UnfoldAsRegionCommand : Command
    {
        /// <summary>
        /// Stores the range that to be unfolded.
        /// </summary>
        private ObservableCollection<TreeNode> toAggregate;
        /// <summary>
        /// Store <see cref="Region"/> mark the beginning of range.
        /// </summary>
        private Region regionBegin;
        /// <summary>
        /// Store <see cref="Region"/> mark the ending of range.
        /// </summary>
        private Region regionEnd;
        /// <summary>
        /// Store the target <see cref="TreeNode"/>.
        /// </summary>
        private TreeNode folderP;

        /// <summary>
        /// Initialize <see cref="Command"/> by its target.
        /// </summary>
        /// <param name="folder">The target <see cref="TreeNode"/>.</param>
        public UnfoldAsRegionCommand(TreeNode folder)
        {
            folderP = folder;
        }

        /// <summary>
        /// Method for forward execution like do or redo.
        /// Redo will not generate a new <see cref="regionBegin"/> and a <see cref="regionEnd"/>.
        /// </summary>
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

        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
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

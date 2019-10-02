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
    /// <see cref="Command"/> that fold a target region.
    /// </summary>
    public class FoldRegionCommand : Command
    {
        /// <summary>
        /// Stores the range that to be folded (<see cref="Region"/> exclusive).
        /// </summary>
        private readonly ObservableCollection<TreeNode> toAggregate;
        /// <summary>
        /// Store <see cref="Region"/> mark the beginning of range.
        /// </summary>
        private readonly Region regionBegin;
        /// <summary>
        /// Store <see cref="Region"/> mark the ending of range. Can be null.
        /// </summary>
        private readonly Region regionEnd;
        /// <summary>
        /// Store the folder that generated after this command executed.
        /// </summary>
        private Folder folderP;

        /// <summary>
        /// Initializes <see cref="Command"/> by both marks and <see cref="TreeNode"/> in its range.
        /// </summary>
        /// <param name="treeNodes">Ranges that to be folded.</param>
        /// <param name="begin">The beginning of the range.</param>
        /// <param name="end">The ending of the range.</param>
        public FoldRegionCommand(ObservableCollection<TreeNode> treeNodes, Region begin, Region end)
        {
            toAggregate = treeNodes;
            regionBegin = begin;
            regionEnd = end;
        }

        /// <summary>
        /// Method for forward execution like do or redo.
        /// Redo will not generate a new <see cref="folderP"/>.
        /// </summary>
        public override void Execute()
        {
            TreeNode parent = regionBegin.Parent;
            bool folderPNotExist = folderP == null;
            if (folderPNotExist)
                folderP = new Folder(parent.parentWorkSpace, regionBegin.attributes[0].AttrInput);
            int index = parent.Children.IndexOf(regionBegin);
            parent.InsertChild(folderP, index);
            foreach (TreeNode t in toAggregate) 
            {
                if (folderPNotExist) folderP.AddChild(t);
                parent.RemoveChild(t);
            }
            parent.RemoveChild(regionBegin);
            if (regionEnd != null) parent.RemoveChild(regionEnd);
        }

        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
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

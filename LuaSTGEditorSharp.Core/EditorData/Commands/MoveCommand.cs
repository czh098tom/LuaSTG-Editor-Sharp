using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    /// <summary>
    /// <see cref="Command"/> that move a <see cref="TreeNode"/> to another <see cref="TreeNode"/>.
    /// </summary>
    internal class MoveCommand : Command
    {
        /// <summary>
        /// The source.
        /// </summary>
        readonly TreeNode source;
        /// <summary>
        /// Parent of the source.
        /// </summary>
        readonly TreeNode sourceParent;
        /// <summary>
        /// ID in parent of source.
        /// </summary>
        readonly int sourceID;
        /// <summary>
        /// Parent of target place.
        /// </summary>
        readonly TreeNode targetParent;
        /// <summary>
        /// ID in parent of target place.
        /// </summary>
        readonly int targetID;

        /// <summary>
        /// Initialize a <see cref="Command"/> with information of source and target.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="targetParent">The parent target.</param>
        /// <param name="targetID">The id of target.</param>
        public MoveCommand(TreeNode source, TreeNode targetParent, int targetID)
        {
            this.source = source;
            this.targetParent = targetParent;
            this.targetID = targetID;
            this.sourceParent = source.Parent;
            this.sourceID = sourceParent.Children.IndexOf(source);
        }

        /// <summary>
        /// Method for forward execution like do or redo.
        /// </summary>
        public override void Execute()
        {
            sourceParent.RemoveChild(source);
            targetParent.InsertChild(source, targetID);
        }

        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
        public override void Undo()
        {
            targetParent.RemoveChild(source);
            sourceParent.InsertChild(source, sourceID);
        }
    }
}

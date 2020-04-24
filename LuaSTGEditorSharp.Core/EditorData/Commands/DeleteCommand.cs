using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    /// <summary>
    /// <see cref="Command"/> that remove a <see cref="TreeNode"/> from a <see cref="WorkTree"/>.
    /// </summary>
    public class DeleteCommand : Command
    {
        /// <summary>
        /// Stores original index of target <see cref="TreeNode"/>.
        /// </summary>
        private readonly int index;
        /// <summary>
        /// Store reference to <see cref="TreeNode"/>
        /// </summary>
        private TreeNode _toOperate;

        /// <summary>
        /// Initializes <see cref="Command"/> by its target.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNode"/>.</param>
        public DeleteCommand(TreeNode toOp)
        {
            _toOperate = toOp;
            index = _toOperate.Parent.Children.IndexOf(_toOperate);
        }

        /// <summary>
        /// Method for forward execution like do or redo.
        /// </summary>
        public override void Execute()
        {
            _toOperate.Parent.RemoveChild(_toOperate);
        }

        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
        public override void Undo()
        {
            _toOperate.Parent.InsertChild(_toOperate, index);
        }
    }
}

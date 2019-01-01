using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    /// <summary>
    /// <see cref="Command"/> that insert a <see cref="TreeNode"/> after a <see cref="TreeNode"/>.
    /// </summary>
    public class InsertAfterCommand : InsertCommand
    {
        /// <summary>
        /// Initializes <see cref="Command"/> by target <see cref="TreeNode"/> 
        /// and <see cref="TreeNode"/> to insert.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNode"/>.</param>
        /// <param name="toIns">The <see cref="TreeNode"/> to insert.</param>
        public InsertAfterCommand(TreeNode toOp, TreeNode toIns) : base(toOp, toIns) { }

        /// <summary>
        /// Method for forward execution like do or redo.
        /// </summary>
        public override void Execute()
        {
            TreeNode parent = _toOperate.Parent;
            parent.InsertChild(_toInsert, parent.Children.IndexOf(_toOperate) + 1);
        }

        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
        public override void Undo()
        {
            _toOperate.Parent.RemoveChild(_toInsert);
        }
    }
}

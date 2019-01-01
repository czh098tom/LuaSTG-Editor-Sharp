using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    /// <summary>
    /// <see cref="Command"/> that insert a <see cref="TreeNode"/> as a child of a <see cref="TreeNode"/>.
    /// </summary>
    public class InsertAsChildCommand : InsertCommand
    {
        /// <summary>
        /// Initializes <see cref="Command"/> by target <see cref="TreeNode"/> 
        /// and <see cref="TreeNode"/> to insert.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNode"/>.</param>
        /// <param name="toIns">The <see cref="TreeNode"/> to insert.</param>
        public InsertAsChildCommand(TreeNode toOp, TreeNode toIns) : base(toOp, toIns) { }

        /// <summary>
        /// Method for forward execution like do or redo.
        /// </summary>
        public override void Execute()
        {
            _toOperate.AddChild(_toInsert);
        }

        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
        public override void Undo()
        {
            _toOperate.RemoveChild(_toInsert);
        }
    }
}

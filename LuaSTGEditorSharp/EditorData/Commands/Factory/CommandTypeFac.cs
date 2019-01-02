using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands.Factory
{
    /// <summary>
    /// Class to store state of insert commands.
    /// </summary>
    public abstract class CommandTypeFac
    {
        /// <summary>
        /// Validate the <see cref="TreeNode"/> insert feasibility and 
        /// return a <see cref="InsertCommand"/> if can.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNode"/>.</param>
        /// <param name="toIns">The <see cref="TreeNode"/> to insert.</param>
        /// <returns>A new <see cref="InsertCommand"/> if can, otherwise null.</returns>
        public InsertCommand ValidateAndNewInsert(TreeNode toOp, TreeNode toIns)
        {
            if (ValidateType(toOp, toIns))
            {
                return NewInsert(toOp, toIns);
            }
            else return null;
        }
        /// <summary>
        /// Create a new <see cref="InsertCommand"/> of target and new <see cref="TreeNode"/>.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNode"/>.</param>
        /// <param name="toIns">The <see cref="TreeNode"/> to insert.</param>
        /// <returns>A new <see cref="InsertCommand"/>.</returns>
        public abstract InsertCommand NewInsert(TreeNode toOp, TreeNode toIns);

        /// <summary>
        /// Validate the feasibility of insert.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNode"/>.</param>
        /// <param name="toIns">The <see cref="TreeNode"/> to insert.</param>
        /// <returns>A <see cref="bool"/> value, true for can.</returns>
        public abstract bool ValidateType(TreeNode toOp, TreeNode toIns);
    }
}

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
        /// Validate the <see cref="TreeNodeBase"/> insert feasibility and 
        /// return a <see cref="InsertCommand"/> if can.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNodeBase"/>.</param>
        /// <param name="toIns">The <see cref="TreeNodeBase"/> to insert.</param>
        /// <returns>A new <see cref="InsertCommand"/> if can, otherwise null.</returns>
        public InsertCommand ValidateAndNewInsert(TreeNodeBase toOp, TreeNodeBase toIns)
        {
            if (ValidateType(toOp, toIns))
            {
                return NewInsert(toOp, toIns);
            }
            else return null;
        }
        /// <summary>
        /// Create a new <see cref="InsertCommand"/> of target and new <see cref="TreeNodeBase"/>.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNodeBase"/>.</param>
        /// <param name="toIns">The <see cref="TreeNodeBase"/> to insert.</param>
        /// <returns>A new <see cref="InsertCommand"/>.</returns>
        public abstract InsertCommand NewInsert(TreeNodeBase toOp, TreeNodeBase toIns);

        /// <summary>
        /// Validate the feasibility of insert.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNodeBase"/>.</param>
        /// <param name="toIns">The <see cref="TreeNodeBase"/> to insert.</param>
        /// <returns>A <see cref="bool"/> value, true for can.</returns>
        public abstract bool ValidateType(TreeNodeBase toOp, TreeNodeBase toIns);
    }
}

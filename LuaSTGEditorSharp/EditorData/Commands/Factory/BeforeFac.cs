using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands.Factory
{
    /// <summary>
    /// Class to store insert before state of insert commands.
    /// </summary>
    public class BeforeFac : CommandTypeFac
    {
        /// <summary>
        /// Create a new <see cref="InsertBeforeCommand"/> of target and new <see cref="TreeNodeBase"/>.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNodeBase"/>.</param>
        /// <param name="toIns">The <see cref="TreeNodeBase"/> to insert.</param>
        /// <returns>A new <see cref="InsertCommand"/>.</returns>
        public override InsertCommand NewInsert(TreeNodeBase toOp, TreeNodeBase toIns)
        {
            return new InsertBeforeCommand(toOp, toIns);
        }

        /// <summary>
        /// Validate the feasibility of insert if new <see cref="TreeNodeBase"/> and old are children of same parent.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNodeBase"/>.</param>
        /// <param name="toIns">The <see cref="TreeNodeBase"/> to insert.</param>
        /// <returns>A <see cref="bool"/> value, true for can.</returns>
        public override bool ValidateType(TreeNodeBase toOp, TreeNodeBase toIns)
        {
            return toOp.Parent.ValidateChild(toIns);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    /// <summary>
    /// <see cref="Command"/> that insert a <see cref="TreeNode"/> to somewhere of another <see cref="TreeNode"/>.
    /// </summary>
    public abstract class InsertCommand : Command
    {
        /// <summary>
        /// The target <see cref="TreeNode"/>
        /// </summary>
        protected TreeNode _toOperate;
        /// <summary>
        /// The <see cref="TreeNode"/> to insert.
        /// </summary>
        protected TreeNode _toInsert;

        /// <summary>
        /// Initializes <see cref="Command"/> by target <see cref="TreeNode"/> 
        /// and <see cref="TreeNode"/> to insert.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNode"/>.</param>
        /// <param name="toIns">The <see cref="TreeNode"/> to insert.</param>
        public InsertCommand(TreeNode toOp, TreeNode toIns)
        {
            _toOperate = toOp;
            _toInsert = toIns;
        }
    }
}

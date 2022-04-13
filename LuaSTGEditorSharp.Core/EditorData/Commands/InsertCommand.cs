using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    /// <summary>
    /// <see cref="Command"/> that insert a <see cref="TreeNodeBase"/> to somewhere of another <see cref="TreeNodeBase"/>.
    /// </summary>
    public abstract class InsertCommand : Command
    {
        /// <summary>
        /// The target <see cref="TreeNodeBase"/>
        /// </summary>
        protected TreeNodeBase _toOperate;
        /// <summary>
        /// The <see cref="TreeNodeBase"/> to insert.
        /// </summary>
        protected TreeNodeBase _toInsert;

        /// <summary>
        /// Initializes <see cref="Command"/> by target <see cref="TreeNodeBase"/> 
        /// and <see cref="TreeNodeBase"/> to insert.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNodeBase"/>.</param>
        /// <param name="toIns">The <see cref="TreeNodeBase"/> to insert.</param>
        public InsertCommand(TreeNodeBase toOp, TreeNodeBase toIns)
        {
            _toOperate = toOp;
            _toInsert = toIns;
        }
    }
}

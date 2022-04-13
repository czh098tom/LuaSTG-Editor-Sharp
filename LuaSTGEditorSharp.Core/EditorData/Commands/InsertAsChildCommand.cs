using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    /// <summary>
    /// <see cref="Command"/> that insert a <see cref="TreeNodeBase"/> as a child of a <see cref="TreeNodeBase"/>.
    /// </summary>
    public class InsertAsChildCommand : InsertCommand
    {
        /// <summary>
        /// Initializes <see cref="Command"/> by target <see cref="TreeNodeBase"/> 
        /// and <see cref="TreeNodeBase"/> to insert.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNodeBase"/>.</param>
        /// <param name="toIns">The <see cref="TreeNodeBase"/> to insert.</param>
        public InsertAsChildCommand(TreeNodeBase toOp, TreeNodeBase toIns) : base(toOp, toIns) { }

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

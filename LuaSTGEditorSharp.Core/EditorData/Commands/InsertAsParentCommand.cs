using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    /// <summary>
    /// <see cref="Command"/> that insert a <see cref="TreeNodeBase"/> as the parent of a <see cref="TreeNodeBase"/>.
    /// </summary>
    public class InsertAsParentCommand : InsertCommand
    {
        /// <summary>
        /// Initializes <see cref="Command"/> by target <see cref="TreeNodeBase"/> 
        /// and <see cref="TreeNodeBase"/> to insert.
        /// </summary>
        /// <param name="toOp">The target <see cref="TreeNodeBase"/>.</param>
        /// <param name="toIns">The <see cref="TreeNodeBase"/> to insert.</param>
        public InsertAsParentCommand(TreeNodeBase toOp, TreeNodeBase toIns) : base(toOp, toIns) { }

        /// <summary>
        /// Method for forward execution like do or redo.
        /// </summary>
        public override void Execute()
        {
            TreeNodeBase parent = _toOperate.Parent;
            parent.InsertChild(_toInsert, parent.Children.IndexOf(_toOperate));
            parent.RemoveChild(_toOperate);
            _toInsert.AddChild(_toOperate);
        }

        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
        public override void Undo()
        {
            TreeNodeBase parent = _toInsert.Parent;
            parent.InsertChild(_toOperate, parent.Children.IndexOf(_toInsert));
            parent.RemoveChild(_toInsert);
            _toInsert.RemoveChild(_toOperate);
        }
    }
}

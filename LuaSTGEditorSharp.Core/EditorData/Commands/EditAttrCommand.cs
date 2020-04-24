using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    /// <summary>
    /// <see cref="Command"/> that modify input of an <see cref="AttrItem"/>.
    /// </summary>
    public class EditAttrCommand : Command
    {
        /// <summary>
        /// Stores reference to target <see cref="AttrItem"/>.
        /// </summary>
        private AttrItem _ToEdit { get; set; }
        /// <summary>
        /// Stores <see cref="string"/> before editing.
        /// </summary>
        private string Original { get; set; }
        /// <summary>
        /// Stores <see cref="string"/> after editing.
        /// </summary>
        private string NewString { get; set; }

        /// <summary>
        /// Initializes <see cref="Command"/> by target <see cref="AttrItem"/>
        /// , original input and new input.
        /// </summary>
        /// <param name="toEdit">The target <see cref="AttrItem"/>.</param>
        /// <param name="original">The original input.</param>
        /// <param name="newStr">The new input.</param>
        public EditAttrCommand(AttrItem toEdit, string original, string newStr)
        {
            _ToEdit = toEdit;
            Original = original;
            NewString = newStr;
        }

        /// <summary>
        /// Method for forward execution like do or redo.
        /// </summary>
        public override void Execute()
        {
            _ToEdit.AttrInput = NewString;
        }

        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
        public override void Undo()
        {
            _ToEdit.AttrInput = Original;
        }
    }
}

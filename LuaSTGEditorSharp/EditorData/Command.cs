using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData
{
    /// <summary>
    /// Base class for all editor revertable commands.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Method for forward execution like do or redo.
        /// </summary>
        public abstract void Execute();
        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
        public abstract void Undo();
    }
}

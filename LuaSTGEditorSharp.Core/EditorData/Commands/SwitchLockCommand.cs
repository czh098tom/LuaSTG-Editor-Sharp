using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    /// <summary>
    /// <see cref="Command"/> that switch lock state of a <see cref="TreeNodeBase"/>.
    /// </summary>
    internal class SwitchLockCommand : Command
    {
        /// <summary>
        /// The target value.
        /// </summary>
        readonly bool targetValue;
        /// <summary>
        /// The original value.
        /// </summary>
        readonly bool originalValue;
        /// <summary>
        /// The target <see cref="TreeNodeBase"/>.
        /// </summary>
        readonly TreeNodeBase target;

        /// <summary>
        /// Initialize a <see cref="Command"/> with information and target and target value.
        /// </summary>
        /// <param name="target">The target <see cref="TreeNodeBase"/></param>
        /// <param name="value">The target value.</param>
        internal SwitchLockCommand(TreeNodeBase target, bool value)
        {
            this.target = target;
            targetValue = value;
            originalValue = target.IsBanned;
        }

        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
        public override void Execute()
        {
            target.IsLocked = targetValue;
        }

        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
        public override void Undo()
        {
            target.IsLocked = originalValue;
        }
    }
}

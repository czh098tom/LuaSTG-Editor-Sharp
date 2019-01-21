using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    /// <summary>
    /// <see cref="Command"/> that switch ban state of a <see cref="TreeNode"/>.
    /// </summary>
    internal class SwitchBanCommand : Command
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
        /// The target <see cref="TreeNode"/>.
        /// </summary>
        readonly TreeNode target;

        /// <summary>
        /// Initialize a <see cref="Command"/> with information and target and target value.
        /// </summary>
        /// <param name="target">The target <see cref="TreeNode"/></param>
        /// <param name="value">The target value.</param>
        internal SwitchBanCommand(TreeNode target, bool value)
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
            target.IsBanned = targetValue;
        }

        /// <summary>
        /// Method for backward execution like undo.
        /// </summary>
        public override void Undo()
        {
            target.IsBanned = originalValue;
        }
    }
}

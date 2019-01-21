using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    internal class SwitchBanCommand : Command
    {
        readonly bool targetValue;
        readonly bool originalValue;
        readonly TreeNode target;

        internal SwitchBanCommand(TreeNode target, bool value)
        {
            this.target = target;
            targetValue = value;
            originalValue = target.IsBanned;
        }

        public override void Execute()
        {
            target.IsBanned = targetValue;
        }

        public override void Undo()
        {
            target.IsBanned = originalValue;
        }
    }
}

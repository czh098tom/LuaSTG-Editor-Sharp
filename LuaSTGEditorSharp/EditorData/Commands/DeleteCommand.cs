using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    public class DeleteCommand : Command
    {
        private readonly int index;
        private TreeNode _toOperate;

        public DeleteCommand(TreeNode toOp)
        {
            _toOperate = toOp;
            index = _toOperate.Parent.Children.IndexOf(_toOperate);
        }

        public override void Execute()
        {
            _toOperate.Parent.RemoveChild(_toOperate);
        }

        public override void Undo()
        {
            _toOperate.Parent.InsertChild(_toOperate, index);
        }
    }
}

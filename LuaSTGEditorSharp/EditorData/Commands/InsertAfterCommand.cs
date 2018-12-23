using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    public class InsertAfterCommand : InsertCommand
    {
        public InsertAfterCommand(TreeNode toOp, TreeNode toIns) : base(toOp, toIns) { }

        public override void Execute()
        {
            TreeNode parent = _toOperate.Parent;
            parent.InsertChild(_toInsert, parent.Children.IndexOf(_toOperate) + 1);
        }

        public override void Undo()
        {
            _toOperate.Parent.RemoveChild(_toInsert);
        }
    }
}

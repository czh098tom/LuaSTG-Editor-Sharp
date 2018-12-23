using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    public class InsertBeforeCommand : InsertCommand
    {
        public InsertBeforeCommand(TreeNode toOp, TreeNode toIns) : base(toOp, toIns) { }

        public override void Execute()
        {
            TreeNode parent = _toOperate.Parent;
            parent.InsertChild(_toInsert, parent.Children.IndexOf(_toOperate));
        }

        public override void Undo()
        {
            _toOperate.Parent.RemoveChild(_toInsert);
        }
    }
}

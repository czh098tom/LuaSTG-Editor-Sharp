using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    public class InsertAsParentCommand : InsertCommand
    {
        public InsertAsParentCommand(TreeNode toOp, TreeNode toIns) : base(toOp, toIns) { }

        public override void Execute()
        {
            TreeNode parent = _toOperate.Parent;
            parent.InsertChild(_toInsert, parent.Children.IndexOf(_toOperate));
            parent.RemoveChild(_toOperate);
            _toInsert.AddChild(_toOperate);
        }

        public override void Undo()
        {
            TreeNode parent = _toInsert.Parent;
            parent.InsertChild(_toOperate, parent.Children.IndexOf(_toInsert));
            parent.RemoveChild(_toInsert);
            _toInsert.RemoveChild(_toOperate);
        }
    }
}

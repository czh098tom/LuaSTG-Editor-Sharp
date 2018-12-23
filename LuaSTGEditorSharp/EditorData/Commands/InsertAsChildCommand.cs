using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    public class InsertAsChildCommand : InsertCommand
    {
        public InsertAsChildCommand(TreeNode toOp, TreeNode toIns) : base(toOp, toIns) { }

        public override void Execute()
        {
            _toOperate.AddChild(_toInsert);
        }

        public override void Undo()
        {
            _toOperate.RemoveChild(_toInsert);
        }
    }
}

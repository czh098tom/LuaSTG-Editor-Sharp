using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    public abstract class InsertCommand : Command
    {
        protected TreeNode _toOperate;
        protected TreeNode _toInsert;

        public InsertCommand(TreeNode toOp, TreeNode toIns)
        {
            _toOperate = toOp;
            _toInsert = toIns;
        }
    }
}

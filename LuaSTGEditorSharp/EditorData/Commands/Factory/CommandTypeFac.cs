using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands.Factory
{
    public abstract class CommandTypeFac
    {
        public InsertCommand ValidateAndNewInsert(TreeNode toOp, TreeNode toIns)
        {
            if (ValidateType(toOp, toIns))
            {
                return NewInsert(toOp, toIns);
            }
            else return null;
        }
        public abstract InsertCommand NewInsert(TreeNode toOp, TreeNode toIns);

        public abstract bool ValidateType(TreeNode toOp, TreeNode toIns);
    }
}

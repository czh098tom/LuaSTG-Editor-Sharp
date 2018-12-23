using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands.Factory
{
    public class BeforeFac : CommandTypeFac
    {
        public override InsertCommand NewInsert(TreeNode toOp, TreeNode toIns)
        {
            return new InsertBeforeCommand(toOp, toIns);
        }

        public override bool ValidateType(TreeNode toOp, TreeNode toIns)
        {
            return toOp.Parent.ValidateChildType(toIns);
        }
    }
}

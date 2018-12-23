using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Commands
{
    public class EditAttrCommand : Command
    {
        private AttrItem _ToEdit { get; set; }
        private string Original { get; set; }
        private string NewString { get; set; }

        public EditAttrCommand(AttrItem toEdit, string original, string newStr)
        {
            _ToEdit = toEdit;
            Original = original;
            NewString = newStr;
        }

        public override void Execute()
        {
            _ToEdit.AttrInput = NewString;
        }

        public override void Undo()
        {
            _ToEdit.AttrInput = Original;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace NodeTest.Imaginary
{
    public class ImaginaryNode : TreeNode
    {
        [NodeAttribute]
        public string A
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [NodeAttribute]
        public string C
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        public string D { get => ""; }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            throw new NotImplementedException();
        }
    }
}

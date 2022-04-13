using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.Advanced.AdvancedRepeat
{
    public abstract class VariableTransformation : FixedAttributeTreeNode
    {
        public VariableTransformation() : base() { }
        public VariableTransformation(DocumentData workSpaceData) : base(workSpaceData) { }

        public abstract Tuple<string, string> GetInformation(string times);
    }
}

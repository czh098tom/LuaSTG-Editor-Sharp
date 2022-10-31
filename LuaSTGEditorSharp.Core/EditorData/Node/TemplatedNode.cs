using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public abstract class TemplatedNode : TreeNodeBase
    {
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            throw new NotImplementedException();
        }
    }
}

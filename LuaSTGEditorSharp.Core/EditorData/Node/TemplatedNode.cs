using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public class TemplatedNode : TreeNode
    {
        private Dictionary<string, string> storedProperties;

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

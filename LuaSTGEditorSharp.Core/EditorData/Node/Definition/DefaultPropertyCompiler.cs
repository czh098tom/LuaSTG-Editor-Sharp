using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.Definition
{
    public class DefaultPropertyCompiler : PropertyCompilerBase
    {
        public string Head { get; set; }
        public string Tail { get; set; }

        public override string ConvertHead(IReadOnlyDictionary<string, string> properties)
        {
            throw new NotImplementedException();
        }
    }
}

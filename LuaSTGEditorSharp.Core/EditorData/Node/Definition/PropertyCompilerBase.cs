using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.Definition
{
    public abstract class PropertyCompilerBase
    {
        public abstract string ConvertHead(IReadOnlyDictionary<string, string> properties);
        public virtual string ConvertTail(IReadOnlyDictionary<string, string> properties) => string.Empty;
    }
}

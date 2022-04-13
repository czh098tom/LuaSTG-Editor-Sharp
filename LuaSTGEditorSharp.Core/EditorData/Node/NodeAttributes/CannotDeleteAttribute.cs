using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.NodeAttributes
{
    /// <summary>
    /// Identify a <see cref="TreeNodeBase"/> cannot delete.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CannotDeleteAttribute : Attribute
    {
    }
}

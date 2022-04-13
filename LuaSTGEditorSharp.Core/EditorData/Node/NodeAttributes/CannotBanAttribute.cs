using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.NodeAttributes
{
    /// <summary>
    /// Identify a <see cref="TreeNodeBase"/> cannot be banned.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CannotBanAttribute : Attribute
    {
    }
}

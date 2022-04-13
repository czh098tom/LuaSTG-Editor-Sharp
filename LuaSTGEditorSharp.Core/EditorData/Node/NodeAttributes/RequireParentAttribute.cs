using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.NodeAttributes
{
    /// <summary>
    /// Identify a <see cref="TreeNodeBase"/> need parent of a given type.
    /// Types are connected by OR operator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RequireParentAttribute : Attribute
    {
        public Type[] ParentType { get; }

        public RequireParentAttribute(params Type[] parent)
        {
            ParentType = parent;
        }
    }
}

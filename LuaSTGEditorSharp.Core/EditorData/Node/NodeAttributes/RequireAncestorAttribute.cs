using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.NodeAttributes
{
    /// <summary>
    /// Identify a <see cref="TreeNodeBase"/> need ancestor of a given type.
    /// Types are connected by OR operator.
    /// Multiple Attribute are connected by AND operator.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RequireAncestorAttribute : Attribute
    {
        public Type[] RequiredTypes { get; }

        public RequireAncestorAttribute(params Type[] types)
        {
            RequiredTypes = types;
        }
    }
}

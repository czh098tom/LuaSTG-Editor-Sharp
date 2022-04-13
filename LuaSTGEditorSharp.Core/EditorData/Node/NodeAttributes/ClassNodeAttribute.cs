using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.NodeAttributes
{
    /// <summary>
    /// Identify a <see cref="TreeNodeBase"/> must be placed at root node or folder directly to root. 
    /// This attribute will be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ClassNodeAttribute : Attribute
    {
    }
}

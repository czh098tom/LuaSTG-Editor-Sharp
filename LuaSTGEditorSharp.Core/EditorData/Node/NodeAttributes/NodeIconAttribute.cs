using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.NodeAttributes
{
    /// <summary>
    /// Identify the icon of a <see cref="TreeNode"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class NodeIconAttribute : Attribute
    {
        public string Path { get; }

        public NodeIconAttribute(string path) { Path = path; }
    }
}

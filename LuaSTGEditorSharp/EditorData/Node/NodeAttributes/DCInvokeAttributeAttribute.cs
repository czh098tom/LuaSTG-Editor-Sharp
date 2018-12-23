using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.NodeAttributes
{
    /// <summary>
    /// Identify a <see cref="TreeNode"/> invoke to edit an <see cref="AttrItem"/> when being right clicked.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RCInvokeAttribute : Attribute
    {
        public int id;

        public RCInvokeAttribute(int id)
        {
            this.id = id;
        }
    }
}

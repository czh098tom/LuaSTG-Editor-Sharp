using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.NodeAttributes
{
    /// <summary>
    /// Identify a <see cref="TreeNode"/> invoke to edit an <see cref="AttrItem"/> when create.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CreateInvokeAttribute : Attribute
    {
        public int id;

        public CreateInvokeAttribute(int id)
        {
            this.id = id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData
{
    public struct OnCreateEventArgs
    {
        public TreeNode parent;
    }

    public struct OnRemoveEventArgs
    {
        public TreeNode parent;
    }

    public struct DependencyAttributeChangedEventArgs
    {
        public string originalValue;
    }

    public delegate void OnCreateNodeHandler(OnCreateEventArgs args);
    public delegate void OnRemoveNodeHandler(OnRemoveEventArgs args);
    public delegate void OnDependencyAttributeChangedHandler(DependencyAttrItem o, DependencyAttributeChangedEventArgs args);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public struct TypeCacheData
    {
        public string icon;
        public bool canDelete;

        public bool classNode;
        public bool leaf;
        public Type[] requireParent;
        public Type[][] requireAncestor;
        public bool uniqueness;
        public bool ignoreValidation;

        public int? createInvokeID;
        public int? rightClickInvokeID;
    }
}

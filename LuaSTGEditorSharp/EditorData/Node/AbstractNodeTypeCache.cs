using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public abstract class AbstractNodeTypeCache
    {
        public abstract string Version { get; }

        public List<Type> NodeTypes { get; set; } = new List<Type>();

        public Dictionary<Type, TypeCacheData> NodeTypeInfo { get; set; } = new Dictionary<Type, TypeCacheData>();

        public Dictionary<Type, TreeNode> StandardNode { get; set; } = new Dictionary<Type, TreeNode>();

        public abstract void Initialize(params Assembly[] assembly);
    }
}

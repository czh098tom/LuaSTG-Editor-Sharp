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

        public virtual void Initialize(params Assembly[] a)
        {
            foreach (Assembly assembly in a)
            {
                NodeTypes.AddRange(from Type t in assembly.GetTypes()
                                   where t.IsSubclassOf(typeof(TreeNode)) && !t.IsAbstract
                                   select t);
            }
            foreach (Type t in NodeTypes)
            {
                TypeCacheData data = new TypeCacheData
                {
                    icon = $"/{t.Assembly.GetName().Name};component/images/16x16/{t.GetCustomAttribute<NodeIconAttribute>().Path}",
                    canDelete = !t.IsDefined(typeof(CannotDeleteAttribute), false),
                    canBeBanned = !t.IsDefined(typeof(CannotBanAttribute), false),
                    classNode = t.IsDefined(typeof(ClassNodeAttribute), false),
                    leaf = t.IsDefined(typeof(LeafNodeAttribute), false),
                    requireParent = GetTypes(t.GetCustomAttribute<RequireParentAttribute>()?.ParentType),
                    uniqueness = t.IsDefined(typeof(UniquenessAttribute), false),
                    ignoreValidation = t.IsDefined(typeof(IgnoreValidationAttribute), false),
                    createInvokeID = t.GetCustomAttribute<CreateInvokeAttribute>()?.id,
                    rightClickInvokeID = t.GetCustomAttribute<RCInvokeAttribute>()?.id
                };
                var attrs = t.GetCustomAttributes<RequireAncestorAttribute>();
                data.requireAncestor = null;
                if (attrs.Count() != 0)
                {
                    data.requireAncestor = (from RequireAncestorAttribute at in attrs
                                            select GetTypes(at.RequiredTypes)).ToArray();
                }
                NodeTypeInfo.Add(t, data);
                StandardNode.Add(t, t.GetConstructor(new Type[] { typeof(DocumentData) }).Invoke(new object[] { null }) as TreeNode);
            }
        }

        public static Type[] GetTypes(Type[] src)
        {
            if (src != null)
            {
                LinkedList<Type> types = new LinkedList<Type>();
                Type it = typeof(ITypeEnumerable);
                foreach (Type t in src)
                {
                    if (it.IsAssignableFrom(t))
                    {
                        ITypeEnumerable o = t.GetConstructor(Type.EmptyTypes).Invoke(new object[0]) as ITypeEnumerable;
                        foreach (Type ty in o)
                        {
                            types.AddLast(ty);
                        }
                    }
                    else
                    {
                        types.AddLast(t);
                    }
                }
                return types.ToArray();
            }
            return null;
        }
    }
}

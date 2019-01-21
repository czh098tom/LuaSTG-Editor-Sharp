using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public class NodeTypeCache : AbstractNodeTypeCache
    {
        public override string Version { get => "0.0.4"; }

        public override void Initialize(params Assembly[] a)
        {
            foreach (Assembly assembly in a)
            {
                NodeTypes.AddRange(from Type t in assembly.GetTypes()
                                   where t.IsSubclassOf(typeof(TreeNode))
                                   select t);
            }
            foreach (Type t in NodeTypes)
            {
                TypeCacheData data = new TypeCacheData
                {
                    icon = t.GetCustomAttribute<NodeIconAttribute>().Path,
                    canDelete = !t.IsDefined(typeof(CannotDeleteAttribute), false),
                    canBeBanned = !t.IsDefined(typeof(CannotBanAttribute), false),
                    classNode = t.IsDefined(typeof(ClassNodeAttribute), false),
                    leaf = t.IsDefined(typeof(LeafNodeAttribute), false),
                    requireParent = t.GetCustomAttribute<RequireParentAttribute>()?.ParentType,
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
                                            select at.RequiredTypes).ToArray();
                }
                NodeTypeInfo.Add(t, data);
                StandardNode.Add(t, t.GetConstructor(new Type[] { typeof(DocumentData) }).Invoke(new object[] { null }) as TreeNode);
            }
        }
    }
}

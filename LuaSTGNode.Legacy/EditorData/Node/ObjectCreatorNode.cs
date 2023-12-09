using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public abstract class ObjectCreatorNode : FixedAttributeTreeNode
    {
        protected ObjectCreatorNode()
        {
        }

        protected ObjectCreatorNode(DocumentData workSpaceData) : base(workSpaceData)
        {
        }

        protected IEnumerable<string> ParseChildrenIfValid(int spacing)
        {
            var sp = Indent(spacing);
            var sp1 = Indent(spacing + 1);
            if (GetLogicalChildren().Any())
            {
                yield return $"{sp}do\n";
                yield return $"{sp1}local self = last\n";
                foreach (var s in base.ToLua(spacing + 1))
                {
                    yield return s;
                }
                yield return $"{sp}end\n";
            }
            else
            {
                yield break;
            }
        }

        protected IEnumerable<Tuple<int, TreeNodeBase>> GetLinesForChildrenIfValid()
        {
            if (GetLogicalChildren().Any())
            {
                yield return new Tuple<int, TreeNodeBase>(2, this);
                foreach (var s in GetChildLines())
                {
                    yield return s;
                }
                yield return new Tuple<int, TreeNodeBase>(1, this);
            }
            else
            {
                yield break;
            }
        }
    }
}

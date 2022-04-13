using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("unitforeach.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [RCInvoke(0)]
    public class GroupForEach : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public GroupForEach() : base() { }

        public GroupForEach(DocumentData workSpaceData) : this(workSpaceData, "GROUP_INDES") { }

        public GroupForEach(DocumentData workSpaceData, string group) : base(workSpaceData) 
        {
            Group = group;
        }

        [JsonIgnore, NodeAttribute]
        public string Group
        {
            get => DoubleCheckAttr(0, "group").attrInput;
            set => DoubleCheckAttr(0, "group").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "for _,unit in ObjList(" + Macrolize(0) + ") do\n";
            foreach(var i in base.ToLua(spacing + 1))
            {
                yield return i;
            }
            yield return sp + "end\n";
        }

        public override string ToString()
        {
            return "For each unit in group " + NonMacrolize(0);
        }

        public override object Clone()
        {
            var n = new GroupForEach(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
            foreach(var i in GetChildLines())
            {
                yield return i;
            }
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }
    }
}

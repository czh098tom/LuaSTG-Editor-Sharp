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
    [Serializable, NodeIcon("setrelpos.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class SetRelativePosition : TreeNode
    {
        [JsonConstructor]
        public SetRelativePosition() : base() { }

        public SetRelativePosition(DocumentData workSpaceData) : this(workSpaceData, "0,0", "self.rot", "false") { }

        public SetRelativePosition(DocumentData workSpaceData, string pos, string rot, string follow) : base(workSpaceData)
        {
            Position = pos;
            Rotation = rot;
            Follow = follow;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(0, "position").attrInput;
            set => DoubleCheckAttr(0, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Rotation
        {
            get => DoubleCheckAttr(1, "rotation").attrInput;
            set => DoubleCheckAttr(1, "rotation").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Follow
        {
            get => DoubleCheckAttr(2, "bool", "Follow master's rotation", true).attrInput;
            set => DoubleCheckAttr(2, "bool", "Follow master's rotation", true).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "_set_rel_pos(self," + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + ")\n";
        }

        public override string ToString()
        {
            return "Set position to (" + NonMacrolize(0) + ") relatively to its master and set its rotation to "
                + NonMacrolize(1) + (NonMacrolize(2) == "true" ? " (Follow master's rotation)" : "");
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs e)
        {
            if(e.originalValue=="false" && relatedAttrItem.attrInput == "true")
            {
                attributes[1].AttrInput = "0";
            }
            if(e.originalValue=="true" && relatedAttrItem.attrInput == "false")
            {
                attributes[1].AttrInput = "self.rot";
            }
        }

        public override object Clone()
        {
            var n = new SetRelativePosition(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

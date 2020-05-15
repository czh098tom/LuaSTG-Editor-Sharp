using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("unitdel.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class Del : TreeNode
    {
        [JsonConstructor]
        private Del() : base() { }

        public Del(DocumentData workSpaceData)
            : this(workSpaceData, "self", "true") { }

        public Del(DocumentData workSpaceData, string tar, string trigger)
            : base(workSpaceData)
        {
            Target = tar;
            TriggerEvent = trigger;
            /*
            attributes.Add(new AttrItem("Target", tar, this, "target"));
            attributes.Add(new AttrItem("Trigger event", trigger, this, "bool"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string TriggerEvent
        {
            get => DoubleCheckAttr(1, "bool", "Trigger event").attrInput;
            set => DoubleCheckAttr(1, "bool", "Trigger event").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "_del(" + Macrolize(0) + "," + Macrolize(1) + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Delete " + NonMacrolize(0);
        }

        public override object Clone()
        {
            var n = new Del(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

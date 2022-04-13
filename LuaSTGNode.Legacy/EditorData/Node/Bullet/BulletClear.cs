using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Bullet
{
    [Serializable, NodeIcon("bulletclear.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class BulletClear : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private BulletClear() : base() { }

        public BulletClear(DocumentData workSpaceData)
            : this(workSpaceData, "true", "false")
        { }

        public BulletClear(DocumentData workSpaceData, string convToFaith, string clrIndes)
            : base(workSpaceData)
        {
            ConvToFaith = convToFaith;
            ClearIndest = clrIndes;
        }

        [JsonIgnore, NodeAttribute]
        public string ConvToFaith
        {
            get => DoubleCheckAttr(0, "bool", "Convert to faith").attrInput;
            set => DoubleCheckAttr(0, "bool", "Convert to faith").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ClearIndest
        {
            get => DoubleCheckAttr(1, "bool", "Clear indestructible").attrInput;
            set => DoubleCheckAttr(1, "bool", "Clear indestructible").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "_clear_bullet(" + Macrolize(0) + "," + Macrolize(1) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "Clear bullets" + (NonMacrolize(1) == "true" ? " (including indestructible)" : "") 
                + (NonMacrolize(0) == "true" ? " and convert them to faith" : "");
        }

        public override object Clone()
        {
            var n = new BulletClear(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

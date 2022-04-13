using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Advanced
{
    [Serializable, NodeIcon("region.png")]
    [LeafNode]
    [RCInvoke(0)]
    public class Region : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private Region() : base() { }

        public Region(DocumentData workSpaceData) : this(workSpaceData, "") { }

        public Region(DocumentData workSpaceData, string code) : base(workSpaceData)
        {
            //attributes.Add(new AttrItem("Name", this) { AttrInput = code });
            Name = code;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Name")]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "-- #region " + NonMacrolize(0) + "\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "Region: " + attributes[0].AttrInput;
        }

        public override object Clone()
        {
            var n = new Region(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

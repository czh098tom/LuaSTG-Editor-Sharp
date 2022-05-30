using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace LuaSTGEditorSharp.EditorData.Node.Advanced
{
    [Serializable, NodeIcon("unidentifiednode.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0)]
    [IgnoreAttributesParityCheck]
    public class UserDefinedDefaultValueNode : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private UserDefinedDefaultValueNode() : base() { }

        public UserDefinedDefaultValueNode(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            attributes.Add(new DependencyAttrItem("Source type", this, "userDefinedNode"));
            attributes.Add(new DependencyAttrItem("Name", this, "userDefinedNode"));
        }

        [JsonIgnore, XmlIgnore]
        protected override bool EnableParityCheck => false;

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "-- #default for " + NonMacrolize(0) + " : " + NonMacrolize(1) + sp + "\n";
        }

        public override string ToString()
        {
            MetaModel target = GetModel();
            if (target != null)
            {
                return "default for " + NonMacrolize(0) + " : " + NonMacrolize(1);
            }
            else
            {
                return "* Unknown node *";
            }
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs args)
        {
            //if (relatedAttrItem.attrInput != originalvalue)
            {
                while (attributes.Count > 2)
                {
                    attributes.RemoveAt(2);
                }
                MetaModel target = GetModel();
                if (target != null)
                {
                    string[,] props = UnidentifiedNode.GetProperties(target);
                    int n = props.GetLength(0);
                    for (int i = 0; i < n; i++)
                    {
                        attributes.Add(new AttrItem(props[i, 0], this, props[i, 2]));
                    }
                }
            }
        }

        public override object Clone()
        {
            var n = new UserDefinedDefaultValueNode(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        private MetaModel GetModel()
        {
            var metas = parentWorkSpace.Meta.UserDefinedData.GetAllSimpleWithDifficulty("").ToArray();
            return metas.FirstOrDefault((mm) => mm.FullName == attributes[0].AttrInput);
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }
    }
}

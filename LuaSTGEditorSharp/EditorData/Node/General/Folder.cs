using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.General
{
    [Serializable,NodeIcon("images/16x16/folder.png")]
    [RCInvoke(0)]
    public class Folder : TreeNode
    {
        [JsonConstructor]
        private Folder() : base() { }

        public Folder(DocumentData workSpaceData) : this(workSpaceData, "") { }
        public Folder(DocumentData workSpaceData, string name) : base(workSpaceData)
        {
            Name = name;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Name")]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override string ToString()
        {
            return attributes[0].AttrInput;
        }

        public override object Clone()
        {
            var n = new Folder(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public abstract class ResourceLoadNode : FixedAttributeTreeNode
    {
        [JsonIgnore, NodeAttribute]
        public string FilePath
        {
            get => DoubleCheckAttr(0, FileType, "Path", true).attrInput;
            set => DoubleCheckAttr(0, FileType, "Path", true).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ResourceName
        {
            get => DoubleCheckAttr(1, name: "Resource name").attrInput;
            set => DoubleCheckAttr(1, name: "Resource name").attrInput = value;
        }

        public ResourceLoadNode() : base() { }
        public ResourceLoadNode(DocumentData documentData) : base(documentData) { }

        public abstract string FileType { get; }
    }
}

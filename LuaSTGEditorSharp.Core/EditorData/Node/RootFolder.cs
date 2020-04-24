using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Node.General;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node
{
    [Serializable, NodeIcon("folder.png")]
    [CannotDelete, CannotBan]
    //[XmlRoot("Root")]
    public class RootFolder : TreeNode
    { 
        [JsonConstructor]
        private RootFolder() :base()
        {
            //activated = true; 
        }

        public RootFolder(DocumentData workSpaceData) : this(workSpaceData, "") { }
        public RootFolder(DocumentData workSpaceData, string name) : base(workSpaceData)
        {
            //attributes.Add(new AttrItem("Name", this) { AttrInput = name });
            Name = name;
            //activated = true;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Name")]
        //[DefaultValue("File")]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
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

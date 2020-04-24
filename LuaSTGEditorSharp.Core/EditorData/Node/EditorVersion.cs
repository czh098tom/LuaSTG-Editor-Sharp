using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Node.General;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node
{
    [Serializable, NodeIcon("setting.png")]
    [CannotDelete, CannotBan]
    [LeafNode]
    //[XmlType(TypeName = "EditorVersion")]
    public class EditorVersion : TreeNode
    {
        [JsonConstructor]
        private EditorVersion() : base() { }

        public EditorVersion(DocumentData workSpaceData) : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Editor version", PluginHandler.Plugin.NodeTypeCache.Version, this));
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("EditorVersion")]
        //[DefaultValue("")]
        public string Version
        {
            get => DoubleCheckAttr(0, name: "Editor version").attrInput;
            set => DoubleCheckAttr(0, name: "Editor version").attrInput = value;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(0, this);
        }

        public override string ToString()
        {
            return "Editor version";
        }

        public override object Clone()
        {
            var n = new Comment(parentWorkSpace, "Editor version: " + attributes[0].AttrInput);
            n.FixAttrParent();
            n.FixChildrenParent();
            return n;
        }
    }
}

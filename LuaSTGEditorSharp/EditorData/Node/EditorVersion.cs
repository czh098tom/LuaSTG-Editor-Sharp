using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [Serializable, NodeIcon("images/16x16/setting.png")]
    [CannotDelete, CannotBan]
    [LeafNode]
    public class EditorVersion : TreeNode
    {
        [JsonConstructor]
        private EditorVersion() : base() { }

        public EditorVersion(DocumentData workSpaceData) : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Editor version", PluginHandler.Plugin.NodeTypeCache.Version, this));
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

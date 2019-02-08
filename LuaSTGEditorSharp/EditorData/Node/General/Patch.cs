using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Lua;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.General
{
    [Serializable, NodeIcon("images/16x16/patch.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class Patch : TreeNode
    {
        [JsonConstructor]
        private Patch() : base() { }

        public Patch(DocumentData workSpaceData) : this(workSpaceData, "") { }

        public Patch(DocumentData workSpaceData, string code) : base(workSpaceData)
        {
            //attributes.Add(new AttrItem("Path", this, "luaFile") { AttrInput = code });
            PathContent = code;
        }

        [JsonIgnore, XmlAttribute("Path")]
        //[DefaultValue("")]
        public string PathContent
        {
            get => DoubleCheckAttr(0, "Path", "luaFile").attrInput;
            set => DoubleCheckAttr(0, "Path", "luaFile").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "Include\'" + StringParser.ParseLua(Path.GetFileName(NonMacrolize(0))) + "\'\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Patch: " + attributes[0].AttrInput;
        }

        protected override void AddCompileSettings()
        {
            if (!parentWorkSpace.CompileProcess.resourceFilePath.Contains(NonMacrolize(0)))
            {
                parentWorkSpace.CompileProcess.resourceFilePath.Add(attributes[0].AttrInput);
            }
        }

        public override object Clone()
        {
            var n = new Patch(parentWorkSpace)
            {
                attributes = new ObservableCollection<AttrItem>(from AttrItem a in attributes select (AttrItem)a.Clone()),
                Children = new ObservableCollection<TreeNode>(from TreeNode t in Children select (TreeNode)t.Clone()),
                _parent = _parent,
                isExpanded = isExpanded
            };
            n.FixAttrParent();
            n.FixChildrenParent();
            return n;
        }
    }
}

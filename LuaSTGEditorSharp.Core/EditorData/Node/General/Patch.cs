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
    [Serializable, NodeIcon("patch.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class Patch : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private Patch() : base() { }

        public Patch(DocumentData workSpaceData) : this(workSpaceData, "") { }

        public Patch(DocumentData workSpaceData, string code) : base(workSpaceData)
        {
            //attributes.Add(new AttrItem("Path", this, "luaFile") { AttrInput = code });
            PathContent = code;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Path")]
        public string PathContent
        {
            get => DoubleCheckAttr(0, "luaFile", "Path").attrInput;
            set => DoubleCheckAttr(0, "luaFile", "Path").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = GetPath(0);
            string sp = Indent(spacing);
            yield return sp + "Include\'" + sk + "\'\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "Patch: " + attributes[0].AttrInput;
        }

        protected override void AddCompileSettings()
        {
            string sk = parentWorkSpace.CompileProcess.archiveSpace + Path.GetFileName(NonMacrolize(0));
            parentWorkSpace.CompileProcess.AddFile(NonMacrolize(0), sk);
        }

        public override object Clone()
        {
            var n = new Patch(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

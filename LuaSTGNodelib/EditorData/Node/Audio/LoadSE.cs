using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Lua;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Audio
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/loadsound.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class LoadSE : TreeNode
    {
        [JsonConstructor]
        private LoadSE() : base() { }

        public LoadSE(DocumentData workSpaceData)
            : this(workSpaceData, "", "") { }

        public LoadSE(DocumentData workSpaceData, string path, string name)
            : base(workSpaceData)
        {
            /*
            attributes.Add(new DependencyAttrItem("Path", path, this, "seFile"));
            attributes.Add(new AttrItem("Resource name", name, this));
            */
            FilePath = path;
            ResName = name;
        }

        [JsonIgnore, NodeAttribute]
        public string FilePath
        {
            get => DoubleCheckAttr(0, "seFile", "Path", true).attrInput;
            set => DoubleCheckAttr(0, "seFile", "Path", true).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ResName
        {
            get => DoubleCheckAttr(1, name: "Resource name").attrInput;
            set => DoubleCheckAttr(1, name: "Resource name").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = GetPath(0);
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "LoadSound(\'se:\'..\'" + StringParser.ParseLua(NonMacrolize(1)) + "\',\'" 
                + sk + "\')\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Load sound effect \"" + NonMacrolize(1) + "\" from \"" + NonMacrolize(0) + "\"";
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs args)
        {
            if (relatedAttrItem.AttrInput != args.originalValue) 
            {
                attributes[1].AttrInput = Path.GetFileNameWithoutExtension(attributes[0].AttrInput);
            }
        }

        protected override void AddCompileSettings()
        {
            string sk = parentWorkSpace.CompileProcess.archiveSpace + Path.GetFileName(NonMacrolize(0));
            if (!parentWorkSpace.CompileProcess.resourceFilePath.ContainsKey(NonMacrolize(0)))
            {
                parentWorkSpace.CompileProcess.resourceFilePath.Add(sk, attributes[0].AttrInput);
            }
        }

        public override MetaInfo GetMeta()
        {
            return new SELoadMetaInfo(this);
        }

        public override object Clone()
        {
            var n = new LoadSE(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

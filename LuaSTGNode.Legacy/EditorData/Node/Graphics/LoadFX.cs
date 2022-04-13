using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Graphics
{
    [Serializable, NodeIcon("loadfx.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0)]
    public class LoadFX : ResourceLoadNode
    {
        [JsonConstructor]
        public LoadFX() : base() { }

        public LoadFX(DocumentData workSpaceData) : this(workSpaceData, "", "") { }
        public LoadFX(DocumentData workSpaceData, string path, string resName) : base(workSpaceData) 
        {
            FilePath = path;
            ResourceName = resName;
        }

        public override string FileType => "fxFile";

        public override string ToString()
        {
            return "Load FX \"" + NonMacrolize(1) + "\" from \"" + NonMacrolize(0) + "\"";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = GetPath(0);
            string sp = Indent(spacing);
            yield return sp + $"LoadFX(\'fx:\'..\'{Lua.StringParser.ParseLua(NonMacrolize(1))}\',\'{sk}\')\n";
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs args)
        {
            if (relatedAttrItem.AttrInput != args.originalValue)
            {
                attributes[1].AttrInput = System.IO.Path.GetFileNameWithoutExtension(attributes[0].AttrInput);
            }
        }

        protected override void AddCompileSettings()
        {
            string sk = parentWorkSpace.CompileProcess.archiveSpace + System.IO.Path.GetFileName(NonMacrolize(0));
            parentWorkSpace.CompileProcess.AddFile(NonMacrolize(0), sk);
        }

        public override MetaInfo GetMeta()
        {
            return new FXLoadMetaInfo(this);
        }

        public override object Clone()
        {
            var n = new LoadFX(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }
    }
}

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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/loadbgm.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class LoadBGM : TreeNode
    {
        [JsonConstructor]
        private LoadBGM() : base() { }

        public LoadBGM(DocumentData workSpaceData)
            : this(workSpaceData, "", "", "", "") { }

        public LoadBGM(DocumentData workSpaceData, string path, string name, string lend, string llen)
            : base(workSpaceData)
        {
            /*
            attributes.Add(new DependencyAttrItem("Path", path, this, "audioFile"));
            attributes.Add(new AttrItem("Resource name", name, this));
            attributes.Add(new AttrItem("Loop end (sec)", lend, this));
            attributes.Add(new AttrItem("Loop length (sec)", llen, this));
            */
            FilePath = path;
            ResName = name;
            LoopEnd = lend;
            LoopLength = llen;
        }

        [JsonIgnore, NodeAttribute]
        public string FilePath
        {
            get => DoubleCheckAttr(0, "audioFile", "Path", true).attrInput;
            set => DoubleCheckAttr(0, "audioFile", "Path", true).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ResName
        {
            get => DoubleCheckAttr(1, name: "Resource name").attrInput;
            set => DoubleCheckAttr(1, name: "Resource name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string LoopEnd
        {
            get => DoubleCheckAttr(2, name: "Loop end (sec)").attrInput;
            set => DoubleCheckAttr(2, name: "Loop end (sec)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string LoopLength
        {
            get => DoubleCheckAttr(3, name: "Loop length (sec)").attrInput;
            set => DoubleCheckAttr(3, name: "Loop length (sec)").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = GetPath(0);
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "MusicRecord(\'bgm:\'..\'" + StringParser.ParseLua(NonMacrolize(1)) + "\',\'" 
                + sk
                + "\'," + Macrolize(2) + "," + Macrolize(3) + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            string s = "";
            if(!(string.IsNullOrEmpty(NonMacrolize(2))|| string.IsNullOrEmpty(NonMacrolize(3))))
            {
                s = ", loop";
            }
            return "Load background music \"" + NonMacrolize(1) + "\" from \"" + NonMacrolize(0) + "\"" + s;
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
            return new BGMLoadMetaInfo(this);
        }

        public override object Clone()
        {
            var n = new LoadBGM(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

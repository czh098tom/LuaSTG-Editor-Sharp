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
            attributes.Add(new DependencyAttrItem("Path", path, this, "audioFile"));
            attributes.Add(new AttrItem("Resource name", name, this));
            attributes.Add(new AttrItem("Loop end (sec)", lend, this));
            attributes.Add(new AttrItem("Loop length (sec)", llen, this));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "MusicRecord(\'" + StringParser.ParseLua(NonMacrolize(1)) + "\',\'" 
                + StringParser.ParseLua(Path.GetFileName(NonMacrolize(0)))
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
            if (!parentWorkSpace.CompileProcess.resourceFilePath.Contains(NonMacrolize(0)))
            {
                parentWorkSpace.CompileProcess.resourceFilePath.Add(attributes[0].AttrInput);
            }
        }

        public override MetaInfo GetMeta()
        {
            return new BGMLoadMetaInfo(this);
        }

        public override object Clone()
        {
            var n = new LoadBGM(parentWorkSpace)
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

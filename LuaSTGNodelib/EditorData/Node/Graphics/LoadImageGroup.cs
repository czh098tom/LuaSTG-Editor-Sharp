using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Graphics
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/loadimagegroup.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(4)]
    public class LoadImageGroup : TreeNode
    {
        [JsonConstructor]
        private LoadImageGroup() : base() { }

        public LoadImageGroup(DocumentData workSpaceData)
            : this(workSpaceData, "", "", "true", "0,0", "0,0", "false") { }

        public LoadImageGroup(DocumentData workSpaceData, string path, string name, string mipmap, string cr, string collis, string rect)
            : base(workSpaceData)
        {
            attributes.Add(new DependencyAttrItem("Path", path, this, "imageFile"));
            attributes.Add(new AttrItem("Resource name", name, this));
            attributes.Add(new AttrItem("Mipmap", mipmap, this, "bool"));
            attributes.Add(new AttrItem("Cols and rows", cr, this, "colrow"));
            attributes.Add(new AttrItem("Collision size", collis, this, "size"));
            attributes.Add(new AttrItem("Rectangle collision", rect, this, "bool"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = GetPath(0);
            string sp = "".PadLeft(spacing * 4);
            //Data Layer: Please add an additional character before numbers so that file name ended with numbers can be splitted correctly
            yield return sp + "_LoadImageGroupFromFile(\'image:\'..\'" + Lua.StringParser.ParseLua(NonMacrolize(1))
                + "\',\'" + sk
                + "\'," + Macrolize(2) + "," + Macrolize(3) + "," + Macrolize(4) + "," + Macrolize(5) + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Load image group \"" + NonMacrolize(1) + "\" from \"" + NonMacrolize(0) + "\", with column and rows (" 
                + NonMacrolize(3) + ")";
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs args)
        {
            if (relatedAttrItem.AttrInput != args.originalValue) 
            {
                string s = Path.GetFileNameWithoutExtension(attributes[0].AttrInput);
                //help editor to split string.
                if (Regex.IsMatch(s, @"\d$"))
                {
                    s += "_";
                }
                attributes[1].AttrInput = s;
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
            return new ImageGroupLoadMetaInfo(this);
        }

        public override object Clone()
        {
            var n = new LoadImageGroup(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

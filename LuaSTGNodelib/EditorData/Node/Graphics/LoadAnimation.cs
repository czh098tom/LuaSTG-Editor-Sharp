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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/loadani.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(3)]
    public class LoadAnimation : TreeNode
    {
        [JsonConstructor]
        public LoadAnimation() : base() { }

        public LoadAnimation(DocumentData workSpaceData)
            : this(workSpaceData, "", "", "true", "1,1", "4", "0,0", "false") { }

        public LoadAnimation(DocumentData workSpaceData, string path, string resName, string mipMap
            , string colRow, string animI, string colliSize, string rect) : base(workSpaceData)
        {
            Path = path;
            ResourceName = resName;
            Mipmap = mipMap;
            ColRow = colRow;
            AnimInterval = animI;
            CollisionSize = colliSize;
            RectangleCollision = rect;
        }

        [JsonIgnore, NodeAttribute]
        public string Path
        {
            get => DoubleCheckAttr(0, "imageFile", isDependency: true).attrInput;
            set => DoubleCheckAttr(0, "imageFile", isDependency: true).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ResourceName
        {
            get => DoubleCheckAttr(1, name: "Resource name").attrInput;
            set => DoubleCheckAttr(1, name: "Resource name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Mipmap
        {
            get => DoubleCheckAttr(2, "bool").attrInput;
            set => DoubleCheckAttr(2, "bool").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ColRow
        {
            get => DoubleCheckAttr(3, "colrow", "Cols and rows").attrInput;
            set => DoubleCheckAttr(3, "colrow", "Cols and rows").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string AnimInterval
        {
            get => DoubleCheckAttr(4, "animinterval", "Animation interval").attrInput;
            set => DoubleCheckAttr(4, "animinterval", "Animation interval").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string CollisionSize
        {
            get => DoubleCheckAttr(5, "size", "Collision size").attrInput;
            set => DoubleCheckAttr(5, "size", "Collision size").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string RectangleCollision
        {
            get => DoubleCheckAttr(6, "bool", "Rectangle collision").attrInput;
            set => DoubleCheckAttr(6, "bool", "Rectangle collision").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = GetPath(0);
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "LoadAniFromFile(\'ani:\'..\'" + Lua.StringParser.ParseLua(NonMacrolize(1))
                + "\',\'" + sk
                + "\'," + Macrolize(2) + "," + Macrolize(3) + "," + Macrolize(4) + "," + Macrolize(5) + "," + Macrolize(6) + ")\n";
        }

        public override string ToString()
        {
            return "Load animation \"" + NonMacrolize(1) + "\" from \"" + NonMacrolize(0) + "\"";
        }

        public override MetaInfo GetMeta()
        {
            return new AnimationLoadMetaInfo(this);
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
            if (!parentWorkSpace.CompileProcess.resourceFilePath.ContainsKey(NonMacrolize(0)))
            {
                parentWorkSpace.CompileProcess.resourceFilePath.Add(sk, attributes[0].AttrInput);
            }
        }

        public override object Clone()
        {
            var n = new LoadAnimation(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

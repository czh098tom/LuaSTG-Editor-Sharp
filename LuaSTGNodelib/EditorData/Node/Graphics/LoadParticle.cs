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
    [Serializable, NodeIcon("loadparticle.png")]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0)]
    public class LoadParticle : TreeNode
    {
        [JsonConstructor]
        public LoadParticle() : base() { }

        public LoadParticle(DocumentData workSpaceData) : this(workSpaceData, "", "", "", "0,0", "false") { }

        public LoadParticle(DocumentData workSpaceData, string path, string resName, string img, string colli, string rect)
            : base(workSpaceData)
        {
            Path = path;
            ResourceName = resName;
            Image = img;
            CollisionSize = colli;
            RectangleCollision = rect;
        }

        [JsonIgnore, NodeAttribute]
        public string Path
        {
            get => DoubleCheckAttr(0, "particleFile", isDependency: true).attrInput;
            set => DoubleCheckAttr(0, "particleFile", isDependency: true).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ResourceName
        {
            get => DoubleCheckAttr(1, name: "Resource name").attrInput;
            set => DoubleCheckAttr(1, name: "Resource name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(2, "image").attrInput;
            set => DoubleCheckAttr(2, "image").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string CollisionSize
        {
            get => DoubleCheckAttr(3, "size", "Collision size").attrInput;
            set => DoubleCheckAttr(3, "size", "Collision size").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string RectangleCollision
        {
            get => DoubleCheckAttr(4, "bool", "Rectangle collision").attrInput;
            set => DoubleCheckAttr(4, "bool", "Rectangle collision").attrInput = value;
        }

        public override object Clone()
        {
            var n = new LoadParticle(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
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

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sk = GetPath(0);
            string sp = Indent(spacing);
            yield return sp + "LoadPS(\'particle:\'..\'" + Lua.StringParser.ParseLua(NonMacrolize(1))
                + "\',\'" + sk
                + "\'," + Macrolize(2) + "," + Macrolize(3) + "," + Macrolize(4) + ")\n";
        }

        public override MetaInfo GetMeta()
        {
            return new ParticleLoadMetaInfo(this);
        }

        public override string ToString()
        {
            return "Load particle \"" + NonMacrolize(1) + "\" from \"" + NonMacrolize(0) 
                + "\" with image \"" + NonMacrolize(2) + "\"";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

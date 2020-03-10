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

namespace LuaSTGEditorSharp.EditorData.Node.Render
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/render4v.png")]
    [RequireAncestor(typeof(RenderAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0)]
    public class Render4V : TreeNode
    {
        [JsonConstructor]
        public Render4V() : base() { }

        public Render4V(DocumentData workSpaceData) : this(workSpaceData, "","0,0","0,0","0,0","0,0") { }

        public Render4V(DocumentData workSpaceData, string img, string p1, string p2, string p3, string p4) 
            : base(workSpaceData) 
        {
            Image = img;
            PositionUL = p1;
            PositionUR = p2;
            PositionLR = p3;
            PositionLL = p4;
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(0, "image").attrInput;
            set => DoubleCheckAttr(0, "image").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string PositionUL
        {
            get => DoubleCheckAttr(1, "position", "Upper Left Position").attrInput;
            set => DoubleCheckAttr(1, "position", "Upper Left Position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string PositionUR
        {
            get => DoubleCheckAttr(2, "position", "Upper Right Position").attrInput;
            set => DoubleCheckAttr(2, "position", "Upper Right Position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string PositionLR
        {
            get => DoubleCheckAttr(3, "position", "Lower Right Position").attrInput;
            set => DoubleCheckAttr(3, "position", "Lower Right Position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string PositionLL
        {
            get => DoubleCheckAttr(4, "position", "Lower Left Position").attrInput;
            set => DoubleCheckAttr(4, "position", "Lower Left Position").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string p1 = $"Render4V({Macrolize(0)}";
            string sn = "".PadLeft(p1.Length);
            yield return sp + p1 + $",{Macrolize(1)},0.5\n";
            yield return sp + sn + $",{Macrolize(2)},0.5\n";
            yield return sp + sn + $",{Macrolize(3)},0.5\n";
            yield return sp + sn + $",{Macrolize(4)},0.5)\n";
        }

        public override string ToString()
        {
            return $"Render {NonMacrolize(0)} in quadrilateral with vertex position "
                + $"({NonMacrolize(1)}), ({NonMacrolize(2)}), ({NonMacrolize(3)}), ({NonMacrolize(4)})";
        }

        public override object Clone()
        {
            var n = new Render4V(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(4, this);
        }
    }
}

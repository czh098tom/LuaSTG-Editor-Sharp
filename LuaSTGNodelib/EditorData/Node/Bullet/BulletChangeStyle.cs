using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Bullet
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bulletchangestyle.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class BulletChangeStyle : TreeNode
    {
        [JsonConstructor]
        private BulletChangeStyle() : base() { }

        public BulletChangeStyle(DocumentData workSpaceData)
            : this(workSpaceData, "self", "arrow_big", "COLOR_RED")
        { }

        public BulletChangeStyle(DocumentData workSpaceData, string target, string sty, string color)
            : base(workSpaceData)
        {
            Target = target;
            Style = sty;
            Color = color;
        }

        [JsonIgnore, NodeAttribute]
        public string Target
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Style
        {
            get => DoubleCheckAttr(1, "bulletStyle").attrInput;
            set => DoubleCheckAttr(1, "bulletStyle").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Color
        {
            get => DoubleCheckAttr(2, "color").attrInput;
            set => DoubleCheckAttr(2, "color").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "ChangeBulletImage(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Change bullet \"" + NonMacrolize(0) + "\"'s style to " + NonMacrolize(1) + " and color to " + NonMacrolize(2);
        }

        public override object Clone()
        {
            var n = new BulletChangeStyle(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

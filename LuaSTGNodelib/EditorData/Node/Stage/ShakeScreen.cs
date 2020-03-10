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

namespace LuaSTGEditorSharp.EditorData.Node.Stage
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/shakescreen.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class ShakeScreen : TreeNode
    {
        [JsonConstructor]
        public ShakeScreen() : base() { }

        public ShakeScreen(DocumentData workSpaceData) : this(workSpaceData, "240", "3") { }

        public ShakeScreen(DocumentData workSpaceData, string time, string amplitude) : base(workSpaceData) 
        {
            Time = time;
            Amplitude = amplitude;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Amplitude
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + $"misc.ShakeScreen({Macrolize(0)},{Macrolize(1)})\n";
        }

        public override string ToString()
        {
            return $"Shake screen {NonMacrolize(0)} frame(s), with amplitude of {NonMacrolize(1)}";
        }

        public override object Clone()
        {
            var n = new ShakeScreen(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

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

namespace LuaSTGEditorSharp.EditorData.Node.Audio
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/playsound.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0),RCInvoke(0)]
    public class PlaySE : TreeNode
    {
        [JsonConstructor]
        public PlaySE() : base() { }

        public PlaySE(DocumentData workSpaceData) : this(workSpaceData, "\"tan00\"", "0.1", "self.x/256", "false") { }

        public PlaySE(DocumentData workSpaceData, string name, string vol
            , string pan, string ignoredef) : base(workSpaceData)
        {
            Name = name;
            Volume = vol;
            Pan = pan;
            IgnoreDefault = ignoredef;
        }

        [JsonIgnore]
        public string Name
        {
            get => DoubleCheckAttr(0, "se").attrInput;
            set => DoubleCheckAttr(0, "se").attrInput = value;
        }

        [JsonIgnore]
        public string Volume
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        [JsonIgnore]
        public string Pan
        {
            get => DoubleCheckAttr(2).attrInput;
            set => DoubleCheckAttr(2).attrInput = value;
        }

        [JsonIgnore]
        public string IgnoreDefault
        {
            get => DoubleCheckAttr(3, name: "Ignore default volume").attrInput;
            set => DoubleCheckAttr(3, name: "Ignore default volume").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "PlaySound(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + "," + Macrolize(3) + ")\n";
        }

        public override string ToString()
        {
            return "Play sound " + NonMacrolize(0)
                + (NonMacrolize(3) == "true" ? " with default volume" : " with volume " + NonMacrolize(1)) + ", pan " + NonMacrolize(2);
        }

        public override object Clone()
        {
            var n = new PlaySE(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

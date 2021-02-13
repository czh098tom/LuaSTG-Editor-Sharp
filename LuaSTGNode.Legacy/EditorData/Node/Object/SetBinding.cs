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

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("connect.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    public class SetBinding : TreeNode
    {
        [JsonConstructor]
        public SetBinding() : base() { }

        public SetBinding(DocumentData workSpaceData) : this(workSpaceData, "self", "last", "0", "true") { }

        public SetBinding(DocumentData workSpaceData, string master, string servant, string dtr, string conndeath) : base(workSpaceData)
        {
            Master = master;
            Servant = servant;
            DMGTransRate = dtr;
            ConnectDeath = conndeath;
        }

        [JsonIgnore, NodeAttribute]
        public string Master
        {
            get => DoubleCheckAttr(0, "target").attrInput;
            set => DoubleCheckAttr(0, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Servant
        {
            get => DoubleCheckAttr(1, "target").attrInput;
            set => DoubleCheckAttr(1, "target").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string DMGTransRate
        {
            get => DoubleCheckAttr(2, name: "Damage transfer rate").attrInput;
            set => DoubleCheckAttr(2, name: "Damage transfer rate").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ConnectDeath
        {
            get => DoubleCheckAttr(3, "bool", "Connect death").attrInput;
            set => DoubleCheckAttr(3, "bool", "Connect death").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "_connect(" + Macrolize(0) + "," + Macrolize(1) + "," + Macrolize(2) + "," + Macrolize(3) + ")\n";
        }

        public override string ToString()
        {
            return "Set " + NonMacrolize(1) + " as servant of " + NonMacrolize(0) + " with damage transfer rate " + NonMacrolize(2)
                + Lua.StaticAnalysis.BoolHint(NonMacrolize(3), ", connect death", "", "");
        }

        public override object Clone()
        {
            var n = new SetBinding(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

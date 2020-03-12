using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Stage
{
    [Serializable, NodeIcon("bgstage.png")]
    [LeafNode]
    [RequireAncestor(typeof(Stage))]
    [RCInvoke(0)]
    public class StageBG : TreeNode
    {
        [JsonConstructor]
        private StageBG() : base() { }

        public StageBG(DocumentData workSpaceData)
            : this(workSpaceData, "temple_background") { }

        public StageBG(DocumentData workSpaceData, string name)
            : base(workSpaceData)
        {
            Background = name;
            //attributes.Add(new AttrItem("Background", name, this, "BG"));
        }

        [JsonIgnore, NodeAttribute]
        public string Background
        {
            get => DoubleCheckAttr(0, "BG").attrInput;
            set => DoubleCheckAttr(0, "BG").attrInput = value;
        }

        public override string ToString()
        {
            return "Set current stage's background to \"" + NonMacrolize(0) + "\"";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "New(" + NonMacrolize(0) + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override object Clone()
        {
            var n = new StageBG(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Enemy
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/pactrometer.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [RCInvoke(0)]
    public class EnemyCharge : TreeNode
    {
        [JsonConstructor]
        public EnemyCharge() : base() { }

        public EnemyCharge(DocumentData workSpaceData) : this(workSpaceData, "self.x,self.y") { }

        public EnemyCharge(DocumentData workSpaceData, string pos) : base(workSpaceData)
        {
            Position = pos;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(0, "position").attrInput;
            set => DoubleCheckAttr(0, "position").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "New(boss_cast_ef," + Macrolize(0) + ")\n";
        }

        public override string ToString()
        {
            return "Charge in (" + NonMacrolize(0) + ")";
        }

        public override object Clone()
        {
            var n = new EnemyCharge(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }
    }
}

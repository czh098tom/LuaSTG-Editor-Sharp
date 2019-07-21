using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Laser
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/lasercreate.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(2)]
    public class CreateLaser : TreeNode
    {
        [JsonConstructor]
        private CreateLaser() : base() { }

        public CreateLaser(DocumentData workSpaceData)
            : this(workSpaceData, "", "self.x,self.y", "")
        { }

        public CreateLaser(DocumentData workSpaceData, string name, string pos, string param)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Name", name, this, "laserDef"));
            attributes.Add(new AttrItem("Position", pos, this, "position"));
            attributes.Add(new AttrItem("Parameters", param, this, "laserParam"));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            string p = Macrolize(2);
            if (string.IsNullOrEmpty(p)) p = "_";
            yield return sp + "last=New(_editor_class[" + Macrolize(0) + "]," + Macrolize(1) + "," + p + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Create laser of type " + NonMacrolize(0) + " at (" + NonMacrolize(1) + ") with parameter " + NonMacrolize(2);
        }

        public override object Clone()
        {
            var n = new CreateLaser(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

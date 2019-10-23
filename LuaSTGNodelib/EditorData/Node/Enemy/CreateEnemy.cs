using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Enemy
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/enemycreate.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(2)]
    public class CreateEnemy : TreeNode
    {
        [JsonConstructor]
        private CreateEnemy() : base() { }

        public CreateEnemy(DocumentData workSpaceData)
            : this(workSpaceData, "", "self.x,self.y", "")
        { }

        public CreateEnemy(DocumentData workSpaceData, string name, string pos, string param)
            : base(workSpaceData)
        {
            Name = name;
            Position = pos;
            Parameters = param;
            /*
            attributes.Add(new AttrItem("Name", name, this, "bulletDef"));
            attributes.Add(new AttrItem("Position", pos, this, "position"));
            attributes.Add(new AttrItem("Parameters", param, this, "bulletParam"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0, "enemyDef").attrInput;
            set => DoubleCheckAttr(0, "enemyDef").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(1, "position").attrInput;
            set => DoubleCheckAttr(1, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Parameters
        {
            get => DoubleCheckAttr(2, "enemyParam").attrInput;
            set => DoubleCheckAttr(2, "enemyParam").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            string p = Macrolize(2);
            if (string.IsNullOrEmpty(p)) p = "_";
            yield return sp + "last=New(_editor_class[" + Macrolize(0) + "]," + Macrolize(1) + "," + p + ")\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Create enemy of type " + NonMacrolize(0) + " at (" + NonMacrolize(1) + ") with parameter " + NonMacrolize(2);
        }

        public override object Clone()
        {
            var n = new CreateEnemy(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

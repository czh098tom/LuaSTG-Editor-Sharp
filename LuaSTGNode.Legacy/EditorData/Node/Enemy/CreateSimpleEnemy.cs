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
    [Serializable, NodeIcon("enemysimple.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [RCInvoke(2)]
    class CreateSimpleEnemy : FixedAttributeTreeNode
    {
        [JsonConstructor]
        public CreateSimpleEnemy() : base() { }

        public CreateSimpleEnemy(DocumentData workSpaceData)
            : this(workSpaceData, "1", "10", "self.x,self.y", "0", "0", "0", "1", "false", "true", "true") { }

        public CreateSimpleEnemy(DocumentData workSpaceData, string style, string hp, string pos, string droppow
            , string dropf, string droppoi, string protect, string clrbullet, string offcremove, string contactdmg)
            : base(workSpaceData)
        {
            Style = style;
            HitPoint = hp;
            Position = pos;
            DropPower = droppow;
            DropFaith = dropf;
            DropPoint = droppoi;
            ProtectTime = protect;
            ClearBullets = clrbullet;
            OffscreenRemoval = offcremove;
            ContactDamage = contactdmg;
        }

        public override object Clone()
        {
            var n = new CreateSimpleEnemy(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            return $"create simple enemy with task at ({NonMacrolize(2)})";
        }

        #region Attr
        [JsonIgnore, NodeAttribute]
        public string Style
        {
            get => DoubleCheckAttr(0, "enemyStyle").attrInput;
            set => DoubleCheckAttr(0, "enemyStyle").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string HitPoint
        {
            get => DoubleCheckAttr(1, name: "Hit point").attrInput;
            set => DoubleCheckAttr(1, name: "Hit point").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(2, "position").attrInput;
            set => DoubleCheckAttr(2, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string DropPower
        {
            get => DoubleCheckAttr(3, name: "Drop power").attrInput;
            set => DoubleCheckAttr(3, name: "Drop power").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string DropFaith
        {
            get => DoubleCheckAttr(4, name: "Drop faith").attrInput;
            set => DoubleCheckAttr(4, name: "Drop faith").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string DropPoint
        {
            get => DoubleCheckAttr(5, name: "Drop point").attrInput;
            set => DoubleCheckAttr(5, name: "Drop point").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ProtectTime
        {
            get => DoubleCheckAttr(6, name: "Protect (n frames)").attrInput;
            set => DoubleCheckAttr(6, name: "Protect (n frames)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ClearBullets
        {
            get => DoubleCheckAttr(7, "bool", "Clear bullets").attrInput;
            set => DoubleCheckAttr(7, "bool", "Clear bullets").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string OffscreenRemoval
        {
            get => DoubleCheckAttr(8, "bool", "Offscreen removal").attrInput;
            set => DoubleCheckAttr(8, "bool", "Offscreen removal").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ContactDamage
        {
            get => DoubleCheckAttr(9, "bool", "Contact damage").attrInput;
            set => DoubleCheckAttr(9, "bool", "Contact damage").attrInput = value;
        }
        #endregion

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string sp1 = Indent(spacing + 1);
            yield return sp + $"last=New(EnemySimple,{Macrolize(0)},{Macrolize(1)},{Macrolize(2)},"
                            + "{" + $"{Macrolize(3)},{Macrolize(4)},{Macrolize(5)}" + "},"
                            + $"{Macrolize(6)},{Macrolize(7)},{Macrolize(8)},{Macrolize(9)},function(self)\n";
            yield return sp1 + "task.New(self,function()\n";
            foreach (var a in base.ToLua(spacing + 2))
            {
                yield return a;
            }
            yield return sp1 + "end)\n";
            yield return sp + "end)\n";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
            foreach (Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }
    }
}

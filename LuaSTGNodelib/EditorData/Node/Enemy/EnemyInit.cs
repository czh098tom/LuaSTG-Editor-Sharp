using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Enemy
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/enemyinit.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(EnemyDefine)), Uniqueness]
    [RCInvoke(0)]
    public class EnemyInit : TreeNode
    {
        [JsonConstructor]
        public EnemyInit() : base() { }

        public EnemyInit(DocumentData workSpaceData) 
            : this(workSpaceData, "", "1", "10", "0", "0", "0", "1", "false", "true", "false") { }

        public EnemyInit(DocumentData workSpaceData, string para, string sty, string hp, string droppow
            , string dropf, string droppoi, string protect, string clrbullet, string offcremove, string contactdmg) 
            : base(workSpaceData) 
        {
            Parameters = para;
            Style = sty;
            HitPoint = hp;
            DropPower = droppow;
            DropFaith = dropf;
            DropPoint = droppoi;
            Protect = protect;
            ClearBullets = clrbullet;
            OffscreenRemoval = offcremove;
            ContactDamage = contactdmg;
        }

        [JsonIgnore, NodeAttribute]
        public string Parameters
        {
            get => DoubleCheckAttr(0, name: "Parameter List").attrInput;
            set => DoubleCheckAttr(0, name: "Parameter List").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Style
        {
            get => DoubleCheckAttr(1, "enemyStyle").attrInput;
            set => DoubleCheckAttr(1, "enemyStyle").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string HitPoint
        {
            get => DoubleCheckAttr(2, name: "Hit point").attrInput;
            set => DoubleCheckAttr(2, name: "Hit point").attrInput = value;
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
        public string Protect
        {
            get => DoubleCheckAttr(6, name: "Protect (frames)").attrInput;
            set => DoubleCheckAttr(6, name: "Protect (frames)").attrInput = value;
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

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(4);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";
            if (Parent?.attributes != null && Parent.AttributeCount >= 2)
            {
                parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0) +
                    (Parent.NonMacrolize(1) == "All" ? "" : ":" + Parent.NonMacrolize(1)));
            }
            string p = (!string.IsNullOrEmpty(NonMacrolize(0)) ? NonMacrolize(0) : "_");
            yield return "_editor_class[\"" + parentName + "\"].init=function(self,_x,_y," + p + ")\n"
                         + sp + "enemy.init(self," + Macrolize(1) + "," + Macrolize(2) + "," + Macrolize(7) 
                            + "," + Macrolize(8) + "," + Macrolize(9) + ")\n"
                         + sp + "self.x,self.y=_x,_y\n"
                         + sp + "self.drop={" + Macrolize(3) + "," + Macrolize(4) + "," + Macrolize(5) + "}\n"
                         + sp + "task.New(self,function() self.protect=true task.Wait(" + Macrolize(6) + ") self.protect=false end)\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return "end\n";
        }

        public override object Clone()
        {
            var n = new EnemyInit(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string ToString()
        {
            return "on init(" + NonMacrolize(0) + ")";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(5, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            TreeNode p = GetLogicalParent();
            if (p?.attributes == null || p.AttributeCount < 2)
            {
                a.Add(new CannotFindAttributeInParent(2, this));
            }
            return a;
        }
    }
}

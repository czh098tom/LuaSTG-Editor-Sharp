using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Node.Boss;
using LuaSTGEditorSharp.EditorData.Node.Object;
using LuaSTGEditorSharp.Windows;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{ 
    public class BossDefineMetaInfo : MetaInfo, IComparable<BulletDefineMetaInfo>
    {
        public BossDefineMetaInfo(BossDefine target) : base(target) { }

        public override string Name
        {
            get => Lua.StringParser.ParseLua(target.attributes[0].AttrInput);
        }

        public override string Difficulty
        {
            get => Lua.StringParser.ParseLua(target.attributes[1].AttrInput);
        }

        public string[] GetSC()
        {
            return (from TreeNode t 
                    in target.Children
                    where t is BossSpellCard && !string.IsNullOrEmpty(t.attributes[0]?.AttrInput)
                    select t.attributes[0].AttrInput).ToArray();
        }

        public string PreviewSC()
        {
            string[] vs = GetSC();
            if (vs.Length > 2)
            {
                return "SpellCards:\n" + vs[0] + "\n" + vs[1] + "\n...";
            }
            else if (vs.Length > 1)
            {
                return "SpellCards:\n" + vs[0] + "\n" + vs[1];
            }
            else if (vs.Length > 0) 
            {
                return "SpellCards:\n" + vs[0];
            }
            else
            {
                return "";
            }
        }

        public int CompareTo(BulletDefineMetaInfo other)
        {
            return Name.CompareTo(other.Name);
        }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.Boss].Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.Boss].Remove(meta);
        }

        public override string FullName
        {
            get
            {
                string d = Difficulty;
                d = d == "All" ? "" : ":" + d;
                return Name + d;
            }
        }

        public override string ScrString
        {
            get => "Name: " + Name + "\nDifficulty: " + Difficulty + "\n" + PreviewSC();
        }

        public override MetaModel GetFullMetaModel()
        {
            MetaModel parent = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/bossdefine.png",
                Text = Name
            };
            MetaModel child;
            string[] childs = GetSC();
            foreach (string s in childs)
            {
                child = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/bossspellcard.png",
                    Text = s
                };
                parent.Children.Add(child);
            }
            return parent;
        }

        public override MetaModel GetSimpleMetaModel()
        {
            return new MetaModel
            {
                Result = "\"" + FullName + "\"",
                Text = ScrString,
                Difficulty = Difficulty,
                FullName = FullName,
                Icon = "/LuaSTGNodeLib;component/images/16x16/bossdefine.png"
            };
        }
    }
}

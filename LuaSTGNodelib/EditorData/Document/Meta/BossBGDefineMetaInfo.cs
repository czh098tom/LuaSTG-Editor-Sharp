using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Node.Boss;
using LuaSTGEditorSharp.Windows;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public class BossBGDefineMetaInfo : MetaInfo, IComparable<BossBGDefineMetaInfo>
    {
        public BossBGDefineMetaInfo(BossBGDefine target) : base(target) { }

        public override string Name { get => Lua.StringParser.ParseLua(target.attributes[0].AttrInput); }

        public override string ScrString => Name;

        public override string FullName => Name;
        public override string Difficulty => "";

        public int CompareTo(BossBGDefineMetaInfo other)
        {
            return Name.CompareTo(other.Name);
        }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.BossBG].Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.BossBG].Remove(meta);
        }

        public string[] GetLayer()
        {
            return (from TreeNode t
                    in target.Children
                    where t is BossBGLayer
                    select t.attributes[0].AttrInput).ToArray();
        }

        public override MetaModel GetFullMetaModel()
        {
            MetaModel bossBG = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/bgdefine.png",
                Text = Name
            };
            MetaModel bossLayer;
            foreach (string s in GetLayer())
            {
                bossLayer = new MetaModel
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/bglayer.png",
                    Text = s
                };
                bossBG.Children.Add(bossLayer);
            }
            return bossBG;
        }

        public override MetaModel GetSimpleMetaModel()
        {
            return new MetaModel
            {
                Result = "\"" + Name + "\"",
                Text = ScrString,
                FullName = FullName,
                Icon = "/LuaSTGNodeLib;component/images/16x16/bgdefine.png"
            };
        }
    }
}

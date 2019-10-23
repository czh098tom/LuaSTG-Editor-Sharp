using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Node.Stage;
using LuaSTGEditorSharp.Windows;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public class StageGroupDefineMetaInfo : MetaInfo, IComparable<StageGroupDefineMetaInfo>
    {
        public StageGroupDefineMetaInfo(Node.Stage.StageGroup target) : base(target) { }

        public override string Name
        {
            get => Lua.StringParser.ParseLua(target.attributes[0].AttrInput);
        }

        public override string Difficulty
        {
            get => Lua.StringParser.ParseLua(target.attributes[0].AttrInput);
        }

        public int CompareTo(StageGroupDefineMetaInfo other)
        {
            return Name.CompareTo(other.Name);
        }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.StageGroup].Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.StageGroup].Remove(meta);
        }

        public string[] GetStages()
        {
            return (from TreeNode t
                    in target.GetLogicalChildren()
                    where (t is Stage)
                    select t.attributes[0].AttrInput).ToArray();
        }

        public override string FullName
        {
            get => Name;
        }

        public override string ScrString
        {
            get => "Name: " + Name + "\nDifficulty: " + Difficulty;
        }

        public override MetaModel GetFullMetaModel()
        {
            MetaModel parent = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/stagegroup.png",
                Text = Name
            };
            MetaModel child;
            string[] cn = GetStages();
            foreach(string s in cn)
            {
                child = new MetaModel()
                {
                    Icon = "/LuaSTGNodeLib;component/images/16x16/stage.png",
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
                Text = FullName,
                FullName = FullName,
                Icon = "/LuaSTGNodeLib;component/images/16x16/stagegroup.png"
            };
        }
    }
}

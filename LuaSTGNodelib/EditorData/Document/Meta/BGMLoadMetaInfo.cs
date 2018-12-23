using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Lua;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Node.Audio;
using LuaSTGEditorSharp.Windows;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public class BGMLoadMetaInfo : MetaInfo, IComparable<BGMLoadMetaInfo>
    {
        public override string Name
        {
            get => StringParser.ParseLua(target.attributes[1].AttrInput);
        }

        public override string FullName
        {
            get => Name;
        }

        public override string Difficulty => "";

        public string Path
        {
            get => StringParser.ParseLua(target.attributes[0].AttrInput);
        }

        public override string ScrString => Name;
        
        public BGMLoadMetaInfo(LoadBGM target) : base(target) { }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.BGMLoad].Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.BGMLoad].Remove(meta);
        }

        public override MetaModel GetFullMetaModel()
        {
            MetaModel metaModel = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/loadbgm.png",
                Text = Name
            };
            MetaModel path = new MetaModel
            {
                Icon = "/LuaSTGNodeLib;component/images/16x16/properties.png",
                Text = StringParser.ParseLua(target.attributes[0].AttrInput)
            };
            metaModel.Children.Add(path);
            return metaModel;
        }

        public int CompareTo(BGMLoadMetaInfo other)
        {
            return Name.CompareTo(other.Name);
        }

        public override MetaModel GetSimpleMetaModel()
        {
            return new MetaModel
            {
                Result = "\"" + FullName + "\"",
                Text = FullName,
                FullName = FullName,
                Icon = "/LuaSTGNodeLib;component/images/16x16/loadbgm.png"
            };
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Node.Project;
using LuaSTGEditorSharp.Windows;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public class ProjectFileMetaInfo : MetaInfo, IComparable<ProjectFileMetaInfo>
    {
        public override string Name
        {
            get => System.IO.Path.GetFileNameWithoutExtension(target.attributes[0].AttrInput);
        }

        public override string FullName
        {
            get => target.attributes[0].AttrInput;
        }

        public override string Difficulty => "";

        public string Path
        {
            get => target.attributes[0].AttrInput;
        }

        public override string ScrString => Name;
        
        public ProjectFileMetaInfo(ProjectFile target) : base(target) { }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.ProjFileData.Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.ProjFileData.Remove(meta);
        }

        public override MetaModel GetFullMetaModel()
        {
            MetaModel metaModel = new MetaModel
            {
                Icon = "/LuaSTGEditorSharp;component/images/16x16/patch.png",
                Text = Name
            };
            MetaModel path = new MetaModel
            {
                Icon = "/LuaSTGEditorSharp;component/images/16x16/properties.png",
                Text = target.attributes[0].AttrInput
            };
            metaModel.Children.Add(path);
            return metaModel;
        }

        public int CompareTo(ProjectFileMetaInfo other)
        {
            return Name.CompareTo(other.Name);
        }

        public override MetaModel GetSimpleMetaModel()
        {
            return new MetaModel
            {
                Result = "\"" + Name + "\"",
                Text = Name,
                FullName = FullName,
                Icon = "/LuaSTGEditorSharp;component/images/16x16/patch.png"
            };
        }
    }
}

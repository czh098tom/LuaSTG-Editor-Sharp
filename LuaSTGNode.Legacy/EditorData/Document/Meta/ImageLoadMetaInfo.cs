using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Node.Graphics;
using LuaSTGEditorSharp.Windows;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public class ImageLoadMetaInfo : ResourceLoadMetaInfo, IComparable<ImageLoadMetaInfo>
    {
        public override string Difficulty => "";

        public override string FullName
        {
            get => "image:" + Name;
        }

        public override string ScrString => Name;

        public ImageLoadMetaInfo(LoadImage target) : base(target) { }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.ImageLoad].Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.ImageLoad].Remove(meta);
        }

        public override MetaModel GetFullMetaModel()
        {
            MetaModel metaModel = new MetaModel
            {
                Icon = "/LuaSTGNode.Legacy;component/images/16x16/loadimage.png",
                Text = Name
            };
            MetaModel path = new MetaModel
            {
                Icon = "/LuaSTGEditorSharp.Core;component/images/16x16/properties.png",
                Text = Path
            };
            metaModel.Children.Add(path);
            return metaModel;
        }

        public int CompareTo(ImageLoadMetaInfo other)
        {
            return Name.CompareTo(other.Name);
        }

        public override MetaModel GetSimpleMetaModel()
        {
            DocumentData current = target.parentWorkSpace;
            string projPath = "";
            if (!string.IsNullOrEmpty(current.DocPath))
                projPath = System.IO.Path.GetDirectoryName(current.DocPath);
            string ppath = "";
            try
            {
                bool? undcPath = RelativePathConverter.IsRelativePath(Path);
                if (undcPath == true)
                {
                    ppath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projPath, Path));
                }
                else if (undcPath == false)
                {
                    ppath = Path;
                }
            }
            catch { }
            return new MetaModel
            {
                Result = "\"" + FullName + "\"",
                Text = FullName,
                FullName = FullName,
                Icon = "/LuaSTGNode.Legacy;component/images/16x16/loadimage.png",
                ExInfo1 = ppath
            };
        }
    }
}

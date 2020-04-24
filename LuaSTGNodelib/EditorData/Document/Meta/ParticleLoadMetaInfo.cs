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
    public class ParticleLoadMetaInfo : MetaInfo, IComparable<ParticleLoadMetaInfo>
    {
        public override string Name
        {
            get => Lua.StringParser.ParseLua(target.attributes[1].AttrInput);
        }

        public override string Difficulty => "";

        public override string FullName
        {
            get => "particle:" + Name;
        }

        public string Path
        {
            get => target.attributes[0].AttrInput;
        }

        public override string ScrString => Name;

        public ParticleLoadMetaInfo(LoadParticle target) : base(target) { }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.ParticleLoad].Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.ParticleLoad].Remove(meta);
        }

        public override MetaModel GetFullMetaModel()
        {
            MetaModel metaModel = new MetaModel
            {
                Icon = "/LuaSTGPlusNodeLib;component/images/16x16/loadparticle.png",
                Text = Name
            };
            MetaModel path = new MetaModel
            {
                Icon = "/LuaSTGPlusNodeLib;component/images/16x16/loadparticle.png",
                Text = target.attributes[0].AttrInput
            };
            metaModel.Children.Add(path);
            return metaModel;
        }

        public int CompareTo(ParticleLoadMetaInfo other)
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
                Icon = "/LuaSTGPlusNodeLib;component/images/16x16/loadparticle.png",
                ExInfo1 = ppath,
                ExInfo2 = target.attributes[2].attrInput
            };
        }
    }
}

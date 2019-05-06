using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Document
{
    public enum MetaType { proj, userDefined
            , stageGroup, Boss, Bullet, ImageLoad, ImageGroupLoad
            , BGMLoad, BossBG, Laser, BentLaser, __max }

    //List version, used in non-immediate update cases
    [Serializable]
    public class MetaData : AbstractMetaData
    {
        [JsonIgnore]
        public IMetaInfoCollection StageGroupDefineData { get => aggregatableMetas[(int)MetaType.stageGroup]; }
        [JsonIgnore]
        public IMetaInfoCollection BossDefineData { get => aggregatableMetas[(int)MetaType.Boss]; }
        [JsonIgnore]
        public IMetaInfoCollection BulletDefineData { get => aggregatableMetas[(int)MetaType.Bullet]; }
        [JsonIgnore]
        public IMetaInfoCollection ImageLoadData { get => aggregatableMetas[(int)MetaType.ImageLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection ImageGroupLoadData { get => aggregatableMetas[(int)MetaType.ImageGroupLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection BGMLoadData { get => aggregatableMetas[(int)MetaType.BGMLoad]; }
        [JsonIgnore]
        public IMetaInfoCollection BossBGDefineData { get => aggregatableMetas[(int)MetaType.BossBG]; }
        [JsonIgnore]
        public IMetaInfoCollection LaserDefineData { get => aggregatableMetas[(int)MetaType.Laser]; }
        [JsonIgnore]
        public IMetaInfoCollection BentLaserDefineData { get => aggregatableMetas[(int)MetaType.BentLaser]; }

        public MetaData()
        {
            aggregatableMetas = new MetaInfoCollection[new PluginEntry().MetaInfoCollectionTypeCount];
            for (int i = 0; i < aggregatableMetas.Length; i++)
            {
                aggregatableMetas[i] = new MetaInfoCollection();
            }
        }

        public MetaData(IMetaInfoCollection[] meta)
        {
            aggregatableMetas = meta;
        }
    }
}

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
    public enum MetaType { proj, stageGroup, Boss, Bullet, ImageLoad, BGMLoad, BossBG, Laser, BentLaser, __max }

    //List version, used in non-immediate update cases
    [Serializable]
    public class MetaData : AbstractMetaData
    {
        [JsonIgnore]
        public IMetaInfoCollection StageGroupDefineData { get => aggregatableMetas[1]; }
        [JsonIgnore]
        public IMetaInfoCollection BossDefineData { get => aggregatableMetas[2]; }
        [JsonIgnore]
        public IMetaInfoCollection BulletDefineData { get => aggregatableMetas[3]; }
        [JsonIgnore]
        public IMetaInfoCollection ImageLoadData { get => aggregatableMetas[4]; }
        [JsonIgnore]
        public IMetaInfoCollection BGMLoadData { get => aggregatableMetas[5]; }
        [JsonIgnore]
        public IMetaInfoCollection BossBGDefineData { get => aggregatableMetas[6]; }
        [JsonIgnore]
        public IMetaInfoCollection LaserDefineData { get => aggregatableMetas[7]; }
        [JsonIgnore]
        public IMetaInfoCollection BentLaserDefineData { get => aggregatableMetas[8]; }

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

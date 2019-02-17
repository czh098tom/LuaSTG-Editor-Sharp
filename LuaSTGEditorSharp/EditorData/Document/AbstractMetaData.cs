using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Document
{
    //List version, used in non-immediate update cases
    [Serializable]
    public abstract class AbstractMetaData
    {
        [JsonIgnore]
        protected DocumentData _parent = null;
        [JsonIgnore]
        public DocumentData Parent => _parent;

        [JsonProperty]
        public IMetaInfoCollection[] aggregatableMetas;

        [JsonIgnore]
        public IMetaInfoCollection ProjFileData { get => aggregatableMetas[0]; }
        [JsonIgnore]
        public IMetaInfoCollection UserDefinedData { get => aggregatableMetas[1]; }

        [JsonConstructor]
        protected AbstractMetaData() { }

        public AbstractMetaData(DocumentData parent)
        {
            aggregatableMetas = new MetaInfoCollection[PluginHandler.Plugin.MetaInfoCollectionTypeCount];
            for (int i = 0; i < aggregatableMetas.Length; i++)
            {
                aggregatableMetas[i] = new MetaInfoCollection();
            }
            _parent = parent;
        }

        public AbstractMetaData(DocumentData parent, IMetaInfoCollection[] meta) : this(parent)
        {
            aggregatableMetas = meta;
        }

        public virtual void CheckIntegrity()
        {

        }

        public static AbstractMetaData Combine(AbstractMetaData[] source)
        {
            AbstractMetaData md = PluginHandler.Plugin.GetMetaData();
            if (source.Count() > 0 && source[0] != null)
            {
                foreach (AbstractMetaData meta in source)
                {
                    for (int i = 0; i < md.aggregatableMetas.Length; i++)
                    {
                        foreach (IAggregatableMeta iam in meta.aggregatableMetas[i])
                        {
                            md.aggregatableMetas[i].QuietAdd(iam);
                        }
                    }
                }
            }
            return md;
        }
    }
}

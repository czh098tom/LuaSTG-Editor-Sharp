using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Interfaces;

namespace LuaSTGEditorSharp.Plugin.Default
{
    internal class DefaultMetaData : AbstractMetaData
    {
        public DefaultMetaData()
        {
            aggregatableMetas = new MetaInfoCollection[new DefaultPluginEntry().MetaInfoCollectionTypeCount];
            for (int i = 0; i < aggregatableMetas.Length; i++)
            {
                aggregatableMetas[i] = new MetaInfoCollection();
            }
        }

        public DefaultMetaData(IMetaInfoCollection[] meta)
        {
            aggregatableMetas = meta;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Message;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Document
{
    //Dict version, used in immediate upgradable case (OriginalMeta)
    [Serializable]
    public class MetaDataEntity : AbstractMetaData, INotifyPropertyChanged, IMessageThrowable
    {
        public ObservableCollection<MessageBase> Messages { get; } = new ObservableCollection<MessageBase>();

        [JsonConstructor]
        protected MetaDataEntity() { }

        public MetaDataEntity(DocumentData parent, bool supressMessage = false)
        {
            if (!supressMessage)
            {
                PropertyChanged += CheckMessage;
            }
            aggregatableMetas = new MetaInfoDict[PluginHandler.Plugin.MetaInfoCollectionTypeCount];
            for (int i = 0; i < aggregatableMetas.Length; i++) 
            {
                aggregatableMetas[i] = new MetaInfoDict(this);
            }
            _parent = parent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }

        public override void CheckIntegrity()
        {
            if (aggregatableMetas.Count() != PluginHandler.Plugin.MetaInfoCollectionTypeCount)
            {
                IMetaInfoCollection[] temp = new IMetaInfoCollection[PluginHandler.Plugin.MetaInfoCollectionTypeCount];
                for (int i = 0; i < temp.Count(); i++)
                {
                    if (i < aggregatableMetas.Count()) 
                    {
                        temp[i] = aggregatableMetas[i];
                    }
                    else
                    {
                        temp[i] = new MetaInfoDict(this);
                    }
                }
                aggregatableMetas = temp;
            }
        }

        public AbstractMetaData GetVirtualized()
        {
            return PluginHandler.Plugin.GetMetaData((from IMetaInfoCollection mic
                                 in aggregatableMetas
                                 select mic.GetVirtualized()).ToArray());
        }

        public void CheckMessage(object sender, PropertyChangedEventArgs e)
        {
            var a = GetMessage();
            Messages.Clear();
            foreach (MessageBase mb in a)
            {
                Messages.Add(mb);
            }
            MessageContainer.UpdateMessage(this);
        }

        public List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            IMetaInfoCollection[] metaInfoCollections;
            if(_parent is PlainDocumentData pdd)
            {
                if (pdd.parentProj != null) return messages;
            }
            //metaInfoCollections = aggregatableMetas;
            metaInfoCollections = _parent.Meta.aggregatableMetas;
            foreach (int[] arrs in PluginHandler.Plugin.MetaInfoCollectionWatchDict)
            {
                List<string> names = new List<string>();
                foreach(int a in arrs)
                {
                    foreach (IAggregatableMeta meta in metaInfoCollections[a])
                    {
                        string s = meta.GetFullName();
                        if (names.Contains(s))
                        {
                            MetaDataEntity source;
                            ProjectData parentProj = (_parent as PlainDocumentData)?.parentProj;
                            if (parentProj == null)
                            {
                                source = this;
                            }
                            else
                            {
                                source = parentProj.OriginalMeta;
                            }
                            messages.Add(new RepeatedNameMessage(s, a, source, this));
                        }
                        else
                        {
                            names.Add(s);
                        }
                    }
                }
            }
            return messages;
        }
    }
}

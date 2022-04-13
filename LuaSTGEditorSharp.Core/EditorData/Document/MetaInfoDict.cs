using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document.Meta;

namespace LuaSTGEditorSharp.EditorData.Document
{
    public class MetaInfoDict : Dictionary<TreeNodeBase, IAggregatableMeta>, IMetaInfoCollection
    {
        private readonly MetaDataEntity _parent;

        public MetaInfoDict(MetaDataEntity parent) { _parent = parent; }

        public void Add(IAggregatableMeta meta)
        {
            QuietAdd(meta);
            _parent.RaisePropertyChanged(meta.GetType().ToString());
        }

        public IAggregatableMeta FindOfName(string s)
        {
            foreach (KeyValuePair<TreeNodeBase, IAggregatableMeta> t in this)
            {
                if (t.Value.GetFullName() == s) return t.Value;
            }
            return null;
        }

        public ObservableCollection<MetaModel> GetAllSimpleWithDifficulty(string difficulty = "")
        {
            return new ObservableCollection<MetaModel>(
                from KeyValuePair<TreeNodeBase, IAggregatableMeta> ia
                in this
                where difficulty == ia.Value.GetDifficulty() || string.IsNullOrEmpty(ia.Value.GetDifficulty()) 
                || ia.Value.GetDifficulty() == "All"
                select ia.Value.GetSimpleMetaModel());
        }

        public ObservableCollection<MetaModel> GetAllFullWithDifficulty(string difficulty = "All")
        {
            return new ObservableCollection<MetaModel>(
                from KeyValuePair<TreeNodeBase, IAggregatableMeta> ia
                in this
                where difficulty == ia.Value.GetDifficulty() || 
                (string.IsNullOrEmpty(ia.Value.GetDifficulty()) && difficulty == "All")
                || difficulty == ""
                select ia.Value.GetFullMetaModel());
        }

        public MetaInfoCollection GetVirtualized()
        {
            return new MetaInfoCollection(from KeyValuePair<TreeNodeBase, IAggregatableMeta> iam
                                          in this
                                          select iam.Value.GetSimpleMetaModel());
        }

        public bool Remove(IAggregatableMeta node)
        {
            bool result= base.Remove((node as MetaInfo).target);
            _parent.RaisePropertyChanged(node.GetType().ToString());
            return result;
        }

        IEnumerator<IAggregatableMeta> IMetaInfoCollection.GetEnumerator()
        {
            return base.Values.GetEnumerator();
        }

        public void QuietAdd(IAggregatableMeta meta)
        {
            if (!base.ContainsKey((meta as MetaInfo).target)) base.Add((meta as MetaInfo).target, meta);
        }
    }
}

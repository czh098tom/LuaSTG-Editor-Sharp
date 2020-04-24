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
    [Serializable]
    public class MetaInfoCollection : List<IAggregatableMeta>, IMetaInfoCollection
    {
        public MetaInfoCollection() : base() { }
        public MetaInfoCollection(int capacity) : base(capacity) { }
        public MetaInfoCollection(IEnumerable<IAggregatableMeta> metas) : base(metas) { }

        public IAggregatableMeta FindOfName(string s)
        {
            foreach (IAggregatableMeta t in this)
            {
                if (t.GetFullName() == s) return t;
            }
            return null;
        }

        public ObservableCollection<MetaModel> GetAllSimpleWithDifficulty(string difficulty = "")
        {
            return new ObservableCollection<MetaModel>(
                from IAggregatableMeta ia
                in this
                where difficulty == ia.GetDifficulty() || string.IsNullOrEmpty(ia.GetDifficulty()) || ia.GetDifficulty() == "All"
                select ia.GetSimpleMetaModel());
        }

        public ObservableCollection<MetaModel> GetAllFullWithDifficulty(string difficulty = "All")
        {
            return new ObservableCollection<MetaModel>(
                from IAggregatableMeta ia
                in this
                where difficulty == ia.GetDifficulty() || (string.IsNullOrEmpty(ia.GetDifficulty()) && difficulty == "All")
                || difficulty == ""
                select ia.GetFullMetaModel());
        }

        public MetaInfoCollection GetVirtualized()
        {
            return new MetaInfoCollection(from IAggregatableMeta iam
                                          in this
                                          select iam.GetSimpleMetaModel());
        }

        public bool Remove(TreeNode node)
        {
            throw new NotImplementedException();
        }

        IEnumerator<IAggregatableMeta> IMetaInfoCollection.GetEnumerator()
        {
            return base.GetEnumerator();
        }

        public void QuietAdd(IAggregatableMeta meta)
        {
            Add(meta);
        }
    }
}

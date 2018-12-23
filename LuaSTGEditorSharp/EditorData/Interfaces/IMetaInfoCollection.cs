using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;

namespace LuaSTGEditorSharp.EditorData.Interfaces
{
    public interface IMetaInfoCollection
    {
        void Add(IAggregatableMeta meta);
        void QuietAdd(IAggregatableMeta meta);
        bool Remove(IAggregatableMeta meta);
        IAggregatableMeta FindOfName(string s);
        ObservableCollection<MetaModel> GetAllSimpleWithDifficulty(string difficulty = "");
        ObservableCollection<MetaModel> GetAllFullWithDifficulty(string difficulty = "");
        MetaInfoCollection GetVirtualized();
        IEnumerator<IAggregatableMeta> GetEnumerator();
    }
}

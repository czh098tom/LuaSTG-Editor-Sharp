using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document.Meta;

namespace LuaSTGEditorSharp.EditorData.Interfaces
{
    public interface IAggregatableMeta
    {
        string GetFullName();
        string GetParam();
        string GetExInfo();
        string GetDifficulty();
        MetaModel GetSimpleMetaModel();
        MetaModel GetFullMetaModel();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;

namespace LuaSTGEditorSharp.EditorData.Interfaces
{
    public interface IDocumentWithMeta
    {
        string DocPath { get; set; }
        AbstractMetaData UndecidedMeta { get; }
    }
}

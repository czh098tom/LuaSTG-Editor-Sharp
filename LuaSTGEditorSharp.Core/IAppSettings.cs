using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp
{
    public interface IAppSettings
    {
        string ZipExecutablePath { get; }
        string LuaSTGExecutablePath { get; }

        bool IsEXEPathSet { get; }

        string TempPath { get; }
        string SLDir { get; set; }
        bool SaveResMeta { get; }
        bool PackProj { get; }
        string PackerType { get; }

        bool SpaceIndentation { get; }
        int IndentationSpaceLength { get;}
    }
}

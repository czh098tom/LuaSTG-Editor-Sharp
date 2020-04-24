using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp
{
    public interface IAppDebugSettings
    {
        bool DebugWindowed { get; }
        int DebugResolutionX { get; }
        int DebugResolutionY { get; }
        bool DebugCheat { get; }
        bool DebugUpdateLib { get; }

        bool DynamicDebugReporting { get; }
    }
}

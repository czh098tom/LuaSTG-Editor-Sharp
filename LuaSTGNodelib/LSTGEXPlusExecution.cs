using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Execution;

namespace LuaSTGEditorSharp
{
    public class LSTGEXPlusExecution : LSTGExecution
    {
        public override void BeforeRun(ExecutionConfig config)
        {
            App currentApp = App.Current as App;
            Parameter= "\"" 
                + "start_game=true is_debug=true setting.nosplash=true setting.windowed="
                + currentApp.DebugWindowed.ToString().ToLower() + " setting.resx=" + currentApp.DebugResolutionX
                + " setting.resy=" + currentApp.DebugResolutionY + " cheat=" + currentApp.DebugCheat.ToString().ToLower()
                + " updatelib=" + currentApp.DebugUpdateLib.ToString().ToLower() + " setting.mod=\'"
                + config.ModName + "\'\"";
            UseShellExecute = false;
            CreateNoWindow = true;
            RedirectStandardError = true;
            RedirectStandardOutput = true;
        }

        protected override string LogFileName => "log.txt";
        public override string ExecutableName => "LuaSTGPlus.dev.exe";

        //protected override string LogFileName => "lstg_log.txt";
        //public override string ExecutableName => "LuaSTG-x.exe";
    }
}

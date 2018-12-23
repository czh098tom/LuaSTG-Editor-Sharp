using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document;

namespace LuaSTGEditorSharp.EditorData.Message
{
    public class EXEPathNotSetMessage : MessageBase
    {
        public string ArgSourceName { get; set; }
        public string ArgTargetName { get; set; }

        public override string SourceName { get => "Settings"; }
        public override DocumentData SourceDoc { get => null; }

        public EXEPathNotSetMessage(string argSourceName, string argTargetName, int level, IMessageThrowable source) : base(level, source)
        {
            ArgSourceName = argSourceName;
            ArgTargetName = argTargetName;
        }

        public override string ToString()
        {
            return "Path \"" + ArgSourceName + "\" for " + ArgTargetName + " is not valid.";
        }

        public override object Clone()
        {
            return new EXEPathNotSetMessage(ArgSourceName, ArgTargetName, WarningLevel, Source);
        }

        public override void Invoke()
        {
            new Windows.SettingsWindow().ShowDialog();
        }
    }
}

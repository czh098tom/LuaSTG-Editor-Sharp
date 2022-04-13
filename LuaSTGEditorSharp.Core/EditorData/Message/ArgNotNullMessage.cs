using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Interfaces;

namespace LuaSTGEditorSharp.EditorData.Message
{
    public class ArgNotNullMessage : MessageBase
    {
        public string ArgName { get; set; }

        public override string SourceName { get => SourceDoc.RawDocName; }
        public override DocumentData SourceDoc { get => (Source as TreeNodeBase).parentWorkSpace; }

        public ArgNotNullMessage(string argName, int level, IMessageThrowable source) : base(level, source)
        {
            ArgName = argName;
        }

        public override string ToString()
        {
            return "Attribute \"" + ArgName + "\" can not be blank.";
        }

        public override object Clone()
        {
            return new ArgNotNullMessage(ArgName, WarningLevel, Source);
        }

        public override void Invoke()
        {
            (Application.Current.MainWindow as IMainWindow).Reveal(Source as TreeNodeBase);
        }
    }
}

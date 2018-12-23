using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document;

namespace LuaSTGEditorSharp.EditorData.Message
{
    public class ArgCountInfo : MessageBase
    {
        public string ArgName { get; set; }
        public bool ArgOverFlow { get; set; }

        public override DocumentData SourceDoc { get => (Source as TreeNode).parentWorkSpace; }

        public ArgCountInfo(string argName, bool argOverflow, IMessageThrowable source) : base(6, source)
        {
            ArgName = argName;
            ArgOverFlow = argOverflow;
        }

        public override string ToString()
        {
            return "Attribute \"" + ArgName + "\" " + (ArgOverFlow ? "overflows." : "is zero.");
        }

        public override object Clone()
        {
            return new ArgCountInfo(ArgName, ArgOverFlow, Source);
        }

        public override void Invoke()
        {
            (App.Current.MainWindow as MainWindow).Reveal(Source as TreeNode);
        }
    }
}

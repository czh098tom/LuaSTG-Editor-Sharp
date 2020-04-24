using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document;

namespace LuaSTGEditorSharp.EditorData.Message
{
    public class VarNameInvalidMessage : MessageBase
    {
        public string ArgName { get; set; }

        public override DocumentData SourceDoc { get => (Source as TreeNode).parentWorkSpace; }

        public VarNameInvalidMessage(string argName, IMessageThrowable source) : base(0, source)
        {
            ArgName = argName;
        }

        public override string ToString()
        {
            return "Varible name in \"" + ArgName + "\" must be a string of letters, digits,\nand underscores, not beginning with a digit";
        }

        public override object Clone()
        {
            return new VarNameInvalidMessage(ArgName, Source);
        }

        public override void Invoke()
        {
            (Application.Current.MainWindow as IMainWindow).Reveal(Source as TreeNode);
        }
    }
}

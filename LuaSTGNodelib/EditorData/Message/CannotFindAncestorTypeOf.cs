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
    public class CannotFindAncestorTypeOf : MessageBase
    {
        public string TypeName { get; set; }

        public override DocumentData SourceDoc { get => (Source as TreeNode).parentWorkSpace; }

        public CannotFindAncestorTypeOf(string typeName, IMessageThrowable source) : base(0, source)
        {
            TypeName = typeName;
        }

        public override string ToString()
        {
            return "No parent type of " + TypeName + " was found in ancestor.";
        }

        public override object Clone()
        {
            return new CannotFindAncestorTypeOf(TypeName, Source);
        }

        public override void Invoke()
        {
            (Application.Current.MainWindow as IMainWindow).Reveal(Source as TreeNode);
        }
    }
}

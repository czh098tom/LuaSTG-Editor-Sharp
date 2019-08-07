using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document;

namespace LuaSTGEditorSharp.EditorData.Message
{
    public class CannotFindAttributeInParent : MessageBase
    {
        public int Count { get; set; }

        public override DocumentData SourceDoc { get => (Source as TreeNode).parentWorkSpace; }

        public CannotFindAttributeInParent(int count, IMessageThrowable source) : base(0, source)
        {
            Count = count;
        }

        public override string ToString()
        {
            return "Attribute count in parent is not sufficient, target is " + Count + ".";
        }

        public override object Clone()
        {
            return new CannotFindAttributeInParent(Count, Source);
        }

        public override void Invoke()
        {
            (App.Current.MainWindow as MainWindow).Reveal(Source as TreeNode);
        }
    }
}

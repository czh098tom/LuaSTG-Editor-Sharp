using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LuaSTGEditorSharp.EditorData.Message
{
    class InvalidArchiveSpaceMessage : MessageBase
    {
        public override DocumentData SourceDoc { get => (Source as TreeNode).parentWorkSpace; }

    public InvalidArchiveSpaceMessage(TreeNode source) : base(0, source) { }

        public override string ToString()
        {
            return @"Invalid archive space name, must be end with '\' or '/'.";
        }

        public override object Clone()
        {
            return new InvalidArchiveSpaceMessage((TreeNode)Source);
        }

        public override void Invoke()
        {
            (Application.Current.MainWindow as IMainWindow).Reveal(Source as TreeNode);
        }
    }
}

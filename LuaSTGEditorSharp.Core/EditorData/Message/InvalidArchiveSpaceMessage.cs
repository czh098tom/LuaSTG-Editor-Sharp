﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LuaSTGEditorSharp.EditorData.Message
{
    class InvalidArchiveSpaceMessage : MessageBase
    {
        public override DocumentData SourceDoc { get => (Source as TreeNodeBase).parentWorkSpace; }

    public InvalidArchiveSpaceMessage(TreeNodeBase source) : base(0, source) { }

        public override string ToString()
        {
            return @"Invalid archive space name, must be end with '\' or '/'.";
        }

        public override object Clone()
        {
            return new InvalidArchiveSpaceMessage((TreeNodeBase)Source);
        }

        public override void Invoke()
        {
            (Application.Current.MainWindow as IMainWindow).Reveal(Source as TreeNodeBase);
        }
    }
}

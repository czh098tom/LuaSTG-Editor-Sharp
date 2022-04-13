using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document;

namespace LuaSTGEditorSharp.EditorData.Message
{
    class AttributeMismatchMessage : MessageBase
    {
        public string AttributeName { get; set; }
        public override DocumentData SourceDoc { get => (Source as TreeNodeBase).parentWorkSpace; }

        public string MismatchLevel
        {
            get
            {
                if (WarningLevel < 1 || WarningLevel > 4)
                {
                    return "";
                }
                else
                {
                    return " by its editing window";
                }
            }
        }

        public AttributeMismatchMessage(string attributeName, int level, IMessageThrowable source) : base(level, source)
        {
            AttributeName = attributeName;
        }

        public override string ToString()
        {
            return "Attribute \"" + AttributeName + "\" mismatch" + MismatchLevel + ".";
        }

        public override object Clone()
        {
            return new AttributeMismatchMessage(AttributeName, WarningLevel, Source);
        }

        public override void Invoke()
        {
            //
        }
    }
}

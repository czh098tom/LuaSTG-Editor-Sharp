using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document;

namespace LuaSTGEditorSharp.EditorData
{
    public abstract class MessageBase : ICloneable
    {
        public IMessageThrowable Source { get; }
        public IMessageThrowable Manager { get; }
        public string ManagerName => Manager.ToString() + ":" + ((Manager as MetaDataEntity)?.Parent.RawDocName ?? "");
        public virtual string SourceName => SourceDoc.RawDocName;
        public abstract DocumentData SourceDoc { get; }
        public MessageBase This { get => this; }
        public string Message { get => ToString(); }
        public int WarningLevel { get; }
        public string Icon
        {
            get
            {
                if (WarningLevel < 1) 
                {
                    return "/LuaSTGNodeLib;component/images/error.png";
                }
                else if (WarningLevel > 5)
                {
                    return "/LuaSTGNodeLib;component/images/info.png";
                }
                else
                {
                    return "/LuaSTGNodeLib;component/images/warning.png";
                }
            }
        }

        public MessageBase(int warningLevel, IMessageThrowable source, IMessageThrowable manager)
        {
            WarningLevel = warningLevel;
            Source = source;
            Manager = manager;
        }

        public MessageBase(int warningLevel, IMessageThrowable source) : this(warningLevel, source, source) { }

        public abstract object Clone();

        public abstract void Invoke();
    }
}

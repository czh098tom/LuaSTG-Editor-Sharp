using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;

namespace LuaSTGEditorSharp.EditorData
{
    public static class MessageContainer
    {
        public static ObservableCollection<MessageBase> Messages { get; set; } = new ObservableCollection<MessageBase>();

        public static void UpdateMessage(IMessageThrowable messageThrower)
        {
            var s = from MessageBase mb
                    in Messages
                    where mb.Manager == messageThrower || mb.Source == messageThrower
                    select mb;
            //DO NOT REMOVE LINE BELOW! LINQ CREATE INDEXES INSTEAD OF STATIC LIST
            List<MessageBase> lst = new List<MessageBase>(s);
            foreach (MessageBase mb in lst)
            {
                Messages.Remove(mb);
            }
            foreach (MessageBase mb in messageThrower.Messages)
            {
                Messages.Add(mb);
            }
        }

        public static bool IsNoError()
        {
            foreach(MessageBase mb in Messages)
            {
                if (mb.WarningLevel < 1) return false;
            }
            return true;
        }
    }
}

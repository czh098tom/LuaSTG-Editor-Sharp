using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Document
{
    public class DocumentCollection : ObservableCollection<DocumentData>
    {
        public int MaxHash { get; private set; } = 0;

        public void AddAndAllocHash(DocumentData a)
        {
            base.Add(a);
            a.parent = this;
            MaxHash += 1;
        }
    }
}

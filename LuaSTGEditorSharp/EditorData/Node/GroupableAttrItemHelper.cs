using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public abstract class GroupableAttrItemHelper
    {
        public abstract void GetFromList(ICollection<AttrItem> c, int i);
    }
}

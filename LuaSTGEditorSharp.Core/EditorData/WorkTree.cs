using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData
{
    [Serializable]
    public class WorkTree : ObservableCollection<TreeNodeBase>
    {
        public WorkTree() : base() { }
        public WorkTree(IEnumerable<TreeNodeBase> treeNodes) : base(treeNodes) { }
        public WorkTree(List<TreeNodeBase> treeNodes) : base(treeNodes) { }
    }
}

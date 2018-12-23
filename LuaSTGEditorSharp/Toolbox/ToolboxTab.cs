using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Toolbox
{
    public class ToolboxTab
    {
        public string Header { get; set; }
        public ObservableCollection<ToolboxItemData> Data { get; set; } = new ObservableCollection<ToolboxItemData>();
    }
}

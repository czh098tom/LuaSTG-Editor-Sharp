using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Toolbox
{
    public class ToolboxItemData
    {
        public bool IsSeperator { get; set; }
        public string Tag { get; set; }
        public string Image { get; set; }
        public string ToolTip { get; set; }

        public ToolboxItemData(bool seperator)
        {
            this.IsSeperator = seperator;
            this.Tag = "";
            this.Image = "";
            this.ToolTip = "";
        }

        public ToolboxItemData(string Tag, string Image, string ToolTip)
        {
            this.IsSeperator = false;
            this.Tag = Tag;
            this.Image = Image;
            this.ToolTip = ToolTip;
        }
    }
}

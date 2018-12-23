using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData
{
    [XmlRoot("dependencyAttribute", IsNullable = false)]
    public class DependencyAttrItem : AttrItem
    {
        [JsonConstructor]
        private DependencyAttrItem() { }

        public DependencyAttrItem(string capital, TreeNode parent) 
            : base(capital, parent) { }

        public DependencyAttrItem(string capital, TreeNode parent, string editWindow)
            : base(capital, parent, editWindow) { }

        public DependencyAttrItem(string capital, string input, TreeNode parent) 
            : base(capital, input, parent) { }

        public DependencyAttrItem(string capital, string input, TreeNode parent, string editWindow)
            : base(capital, input, parent, editWindow) { }

        [JsonIgnore, XmlIgnore]
        public override string AttrInput
        {
            get => attrInput;
            set
            {
                string s = attrInput;
                attrInput = value;
                _parent.ReflectAttr(this, s);
                RaiseProertyChanged("AttrInput");
                _parent?.RaiseProertyChanged("ScreenString");
                _parent?.parentWorkSpace?.OriginalMeta.RaisePropertyChanged(_parent.GetType().ToString());
            }
        }

        public override object Clone()
        {
            return new DependencyAttrItem(attrCap, attrInput, _parent, EditWindow);
        }
    }
}

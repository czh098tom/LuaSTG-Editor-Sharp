using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LuaSTGEditorSharp.EditorData.Commands;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData
{
    [Serializable]
    [XmlRoot("attribute", IsNullable =false)]
    public class AttrItem : INotifyPropertyChanged, ICloneable
    {
        [JsonProperty, XmlAttribute("name")]
        protected string attrCap;
        [JsonProperty, XmlAttribute("input")]
        protected string attrInput;
        [JsonIgnore, XmlIgnore]
        protected TreeNode _parent;
        [JsonProperty, XmlAttribute("edit")]
        public string EditWindow { get; set; } = "";

        [JsonIgnore, XmlIgnore]
        public TreeNode Parent { get => _parent; set => _parent = value; }

        [JsonIgnore, XmlIgnore]
        public AttrItem This { get => this; }

        [JsonConstructor]
        protected AttrItem() { }
        
        public AttrItem(string capital, TreeNode parent)
        {
            _parent = parent;
            attrCap = capital;
            attrInput = "";
        }

        public AttrItem(string capital, TreeNode parent, string editWindow) : this(capital, parent)
        {
            EditWindow = editWindow;
        }

        public AttrItem(string capital, string input, TreeNode parent) : this(capital, parent)
        {
            attrInput = input;
        }

        public AttrItem(string capital, string input, TreeNode parent, string editWindow) : this(capital, input, parent)
        {
            EditWindow = editWindow;
        }

        [JsonIgnore]
        public string AttrInput_InvokeCommand
        {
            get => attrInput;
            set
            {
                _parent.parentWorkSpace.AddAndExecuteCommand(new EditAttrCommand(this, attrInput, value));
            }
        }

        [JsonIgnore]
        public virtual string AttrInput
        {
            get => attrInput;
            set
            {
                attrInput = value;
                RaiseProertyChanged("AttrInput");
                _parent?.RaiseProertyChanged("ScreenString");
                _parent?.parentWorkSpace?.OriginalMeta.RaisePropertyChanged(_parent.GetType().ToString());
            }
        }

        [JsonIgnore]
        public string AttrCap { get { return attrCap; } }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual object Clone()
        {
            return new AttrItem(AttrCap, _parent) { attrInput = this.attrInput, EditWindow = this.EditWindow };
        }

        protected void RaiseProertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public virtual AttrItem UpdateThis(AttrItem Standard)
        {
            if(Standard is AttrItem)
            {
                attrCap = Standard.attrCap;
                EditWindow = Standard.EditWindow;
                return null;
            }
            else
            {
                AttrItem item = Standard.Clone() as AttrItem;
                item._parent = _parent;
                item.AttrInput = AttrInput;
                return item;
            }
        }
    }
}

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
    /// <summary>
    /// This class stores attributes in a <see cref="TreeNode"/>.
    /// </summary>
    [Serializable]
    [XmlRoot("attribute", IsNullable = false)]
    public class AttrItem : INotifyPropertyChanged, ICloneable
    {
        /// <summary>
        /// Stores capital <see cref="string"/>.
        /// </summary>
        [JsonProperty, XmlAttribute("name")]
        protected string attrCap;
        /// <summary>
        /// Store input <see cref="string"/>.
        /// </summary>
        [JsonProperty, XmlAttribute("input")]
        public string attrInput;
        /// <summary>
        /// Store parent <see cref="TreeNode"/> of this attribute.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        protected TreeNode _parent;
        /// <summary>
        /// Store edit window args.
        /// </summary>
        private string editWindow = "";
        /// <summary>
        /// Store edit window args.
        /// </summary>
        [JsonProperty, XmlAttribute("edit")]
        public string EditWindow
        {
            get => editWindow;
            set
            {
                editWindow = value;
                RaiseProertyChanged("EditWindow");
            }
        }

        /// <summary>
        /// Store parent <see cref="TreeNode"/> of this attribute.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public TreeNode Parent { get => _parent; set => _parent = value; }

        /// <summary>
        /// Used to help event get current <see cref="TreeNode"/>.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public AttrItem This { get => this; }

        /// <summary>
        /// Constructor when serializing.
        /// </summary>
        [JsonConstructor]
        protected AttrItem() { }
        
        /// <summary>
        /// Initializes <see cref="AttrItem"/> by capital and parent.
        /// </summary>
        /// <param name="capital">The capital.</param>
        /// <param name="parent">The parent.</param>
        public AttrItem(string capital, TreeNode parent)
        {
            _parent = parent;
            attrCap = capital;
            attrInput = "";
        }

        /// <summary>
        /// Initializes <see cref="AttrItem"/> by capital, parent and edit window.
        /// </summary>
        /// <param name="capital">The capital.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="editWindow">The edit window args.</param>
        public AttrItem(string capital, TreeNode parent, string editWindow) : this(capital, parent)
        {
            EditWindow = editWindow;
        }

        /// <summary>
        /// Initializes <see cref="AttrItem"/> by capital, input and parent.
        /// </summary>
        /// <param name="capital">The capital.</param>
        /// <param name="input">The input.</param>
        /// <param name="parent">The parent.</param>
        public AttrItem(string capital, string input, TreeNode parent) : this(capital, parent)
        {
            attrInput = input;
        }

        /// <summary>
        /// Initializes <see cref="AttrItem"/> by capital, input, parent and edit window.
        /// </summary>
        /// <param name="capital">The capital.</param>
        /// <param name="input">The input.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="editWindow">The edit window args.</param>
        public AttrItem(string capital, string input, TreeNode parent, string editWindow) : this(capital, input, parent)
        {
            EditWindow = editWindow;
        }

        /// <summary>
        /// Initializes <see cref="AttrItem"/> by capital, input and edit window.
        /// </summary>
        /// <param name="capital">The capital.</param>
        /// <param name="input">The input.</param>
        /// <param name="editWindow">The edit window args.</param>
        public AttrItem(string capital, string input = "", string editWindow = "") : this()
        {
            this.attrCap = capital;
            this.attrInput = input;
            this.editWindow = editWindow;
        }

        /// <summary>
        /// UI triggers for <see cref="attrInput"/>. Set method is executing <see cref="Command"/>.
        /// </summary>
        [JsonIgnore]
        public string AttrInput_InvokeCommand
        {
            get => attrInput;
            set
            {
                _parent.parentWorkSpace.AddAndExecuteCommand(new EditAttrCommand(this, attrInput, value));
            }
        }

        /// <summary>
        /// Non-UI triggers for <see cref="attrInput"/>. Set value and raise events.
        /// </summary>
        [JsonIgnore]
        public virtual string AttrInput
        {
            get => attrInput;
            set
            {
                attrInput = value;
                RaiseProertyChanged("AttrInput");
                RaiseProertyChanged("AttrInput_InvokeCommand");
                _parent?.RaiseProertyChanged("ScreenString");
                _parent?.parentWorkSpace?.OriginalMeta.RaisePropertyChanged(_parent.GetType().ToString());
            }
        }

        /// <summary>
        /// Property for <see cref="attrCap"/>. Read only.
        /// </summary>
        [JsonIgnore]
        public string AttrCap { get { return attrCap; } }

        /// <summary>
        /// The event of property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Get a deep copy of this attribute.
        /// </summary>
        /// <returns>A deep copy of this attribute.</returns>
        public virtual object Clone()
        {
            return new AttrItem(AttrCap, _parent) { attrInput = this.attrInput, EditWindow = this.EditWindow };
        }

        /// <summary>
        /// The method that raise property changed.
        /// </summary>
        /// <param name="propName">The parameter of event.</param>
        protected void RaiseProertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// USELESS
        /// </summary>
        /// <param name="Standard"></param>
        /// <returns></returns>
        [Obsolete]
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

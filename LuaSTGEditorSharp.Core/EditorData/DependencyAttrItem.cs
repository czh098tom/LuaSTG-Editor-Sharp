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
        /// <summary>
        /// Constructor when serializing.
        /// </summary>
        [JsonConstructor]
        private DependencyAttrItem() { }

        /// <summary>
        /// Initializes <see cref="AttrItem"/> by capital and parent.
        /// </summary>
        /// <param name="capital">The capital.</param>
        /// <param name="parent">The parent.</param>
        public DependencyAttrItem(string capital, TreeNode parent) 
            : base(capital, parent) { }

        /// <summary>
        /// Initializes <see cref="AttrItem"/> by capital, parent and edit window.
        /// </summary>
        /// <param name="capital">The capital.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="editWindow">The edit window args.</param>
        public DependencyAttrItem(string capital, TreeNode parent, string editWindow)
            : base(capital, parent, editWindow) { }

        /// <summary>
        /// Initializes <see cref="AttrItem"/> by capital, input and parent.
        /// </summary>
        /// <param name="capital">The capital.</param>
        /// <param name="input">The input.</param>
        /// <param name="parent">The parent.</param>
        public DependencyAttrItem(string capital, string input, TreeNode parent) 
            : base(capital, input, parent) { }

        /// <summary>
        /// Initializes <see cref="AttrItem"/> by capital, input, parent and edit window.
        /// </summary>
        /// <param name="capital">The capital.</param>
        /// <param name="input">The input.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="editWindow">The edit window args.</param>
        public DependencyAttrItem(string capital, string input, TreeNode parent, string editWindow)
            : base(capital, input, parent, editWindow) { }

        /// <summary>
        /// Initializes <see cref="AttrItem"/> by capital, input and edit window.
        /// </summary>
        /// <param name="capital">The capital.</param>
        /// <param name="input">The input.</param>
        /// <param name="editWindow">The edit window args.</param>
        public DependencyAttrItem(string capital, string input = "", string editWindow = "") 
            : base(capital, input, editWindow) { }

        /// <summary>
        /// Non-UI triggers for <see cref="attrInput"/>. Set value and raise events.
        /// Also tell parent <see cref="TreeNode"/> to update its <see cref="AttrItem"/>.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public override string AttrInput
        {
            get => attrInput;
            set
            {
                string s = attrInput;
                attrInput = value;
                _parent.RaiseDependencyPropertyChanged(this
                    , new DependencyAttributeChangedEventArgs() { originalValue = s });
                RaiseProertyChanged("AttrInput");
                RaiseProertyChanged("AttrInput_InvokeCommand");
                _parent?.RaiseProertyChanged("ScreenString");
                _parent?.parentWorkSpace?.OriginalMeta.RaisePropertyChanged(_parent.GetType().ToString());
            }
        }

        /// <summary>
        /// Get a deep copy of this attribute.
        /// </summary>
        /// <returns>A deep copy of this attribute.</returns>
        public override object Clone()
        {
            return new DependencyAttrItem(attrCap, attrInput, _parent, EditWindow);
        }
    }
}

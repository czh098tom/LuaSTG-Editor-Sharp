using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData.Commands;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.EditorData.Node.Advanced;
using LuaSTGEditorSharp.EditorData.Node.General;
using LuaSTGEditorSharp.EditorData.Node.Project;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData
{
    /// <summary>
    /// This class is the base class for all tree nodes.
    /// </summary>
    /// <remarks>
    /// Remember to invoke base.ToLua() Method if you need to get Lua code of its child, 
    /// otherwise it will NOT CONSIDER SCPRACTICE and STAGEPRACTICE.
    /// The Standard Nodes can be identified by <code>_parent==null</code>
    /// </remarks>
    [Serializable]
    public abstract class TreeNode : INotifyPropertyChanged, ICloneable, IMessageThrowable
    {
        /// <summary>
        /// Store attributes in <see cref="TreeNode"/>.
        /// </summary>
        //[XmlArray("attributes")]
        [JsonIgnore, XmlIgnore]
        public ObservableCollection<AttrItem> attributes = new ObservableCollection<AttrItem>();

        /// <summary>
        /// Get the <see cref="Attribute"/> of this <see cref="TreeNode"/>.
        /// If is setted to null, create a new <see cref="ObservableCollection{T}"/>,
        /// and register the <see cref="ObservableCollection{T}.CollectionChanged"/> event.
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<AttrItem> Attributes
        {
            get => attributes;
            set
            {
                if (value == null)
                {
                    attributes = new ObservableCollection<AttrItem>();
                    attributes.CollectionChanged += new NotifyCollectionChangedEventHandler(this.AttributesChanged);
                }
                else
                {
                    throw new InvalidOperationException();
                    //attributes = value;
                }
            }
        }
        /// <summary>
        /// Store whether a <see cref="TreeNode"/> is expanded in view. Use this only when you do not want to refresh the view.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool isExpanded;
        /// <summary>
        /// Store whether a <see cref="TreeNode"/> is selected in view. Use this only when you do not want to refresh the view.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool isSelected = false;
        /// <summary>
        /// Store whether a <see cref="TreeNode"/> is banned. Use this only when you do not want to refresh the view.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        private bool isBanned = false;
        /// <summary>
        /// Store whether a <see cref="TreeNode"/> is expanded in view. Using this will refresh the view.
        /// </summary>
        [JsonProperty, DefaultValue(true)]
        [XmlAttribute("expanded")]
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                RaiseProertyChanged("IsExpanded");
            }
        }
        /// <summary>
        /// Store whether a <see cref="TreeNode"/> is selected in view. Using this will refresh the view.
        /// </summary>
        [JsonProperty, DefaultValue(false)]
        [XmlAttribute("selected")]
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                RaiseProertyChanged("IsSelected");
            }
        }
        /// <summary>
        /// Store whether a <see cref="TreeNode"/> is banned. 
        /// Using setter of this will create a new <see cref="Command"/> and execute it.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool IsBanned_InvokeCommand
        {
            get => isBanned;
            set
            {
                parentWorkSpace.AddAndExecuteCommand(new SwitchBanCommand(this, value));
                if (isBanned)
                {
                    RaiseVirtuallyRemove(new OnRemoveEventArgs() { parent = _parent });
                }
                else
                {
                    RaiseVirtuallyCreate(new OnCreateEventArgs() { parent = _parent });
                }
            }
        }
        /// <summary>
        /// Store whether a <see cref="TreeNode"/> is banned. 
        /// Using this will refresh the view, <see cref="MetaInfo"/> and <see cref="MessageBase"/>.
        /// </summary>
        [JsonProperty, DefaultValue(false)]
        [XmlAttribute("banned")]
        public bool IsBanned
        {
            get => isBanned;
            set
            {
                if (CanBeBanned)
                {
                    isBanned = value;
                }
                else
                {
                    isBanned = false;
                }
                RaiseProertyChanged("IsBanned");
            }
        }
        /// <summary>
        /// Use this to get the icon of the <see cref="TreeNode"/>. 
        /// This property is always synced with <see cref="Node.NodeAttributes.NodeIconAttribute"/>.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public string Icon { get => PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].icon; }
        /// <summary>
        /// Use this to get whether the <see cref="TreeNode"/> can be deleted. 
        /// This property is always synced with <see cref="Node.NodeAttributes.CannotDeleteAttribute"/> (reverted).
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool CanDelete { get => PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].canDelete; }
        /// <summary>
        /// Use this to get whether the <see cref="TreeNode"/> cannot be banned. 
        /// This property is always synced with <see cref="Node.NodeAttributes.CannotBanAttribute"/> (reverted).
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool CanBeBanned { get => PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].canBeBanned; }
        /// <summary>
        /// Use this to get whether the <see cref="TreeNode"/> ignores validation. 
        /// This property is always synced with <see cref="Node.NodeAttributes.IgnoreValidationAttribute"/>.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool IgnoreValidation { get => PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].ignoreValidation; }

        [JsonIgnore, XmlIgnore]
        private TreeNode _parent = null;
        /// <summary>
        /// Store the <see cref="DocumentData"/> containing it. 
        /// Only change it when it is created from file or be moved to other documents.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public DocumentData parentWorkSpace = null;

        /// <summary>
        /// Store the child <see cref="TreeNode"/> of this <see cref="TreeNode"/>.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        private ObservableCollection<TreeNode> children;
        /// <summary>
        /// Get the child <see cref="TreeNode"/> of this <see cref="TreeNode"/>.
        /// If is setted to null, create a new <see cref="ObservableCollection{T}"/>,
        /// and register the <see cref="ObservableCollection{T}.CollectionChanged"/> event.
        /// </summary>
        [JsonIgnore, XmlElement("Node")]
        public ObservableCollection<TreeNode> Children
        {
            get => children;
            set
            {
                if (value == null)
                {
                    children = new ObservableCollection<TreeNode>();
                    children.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ChildrenChanged);
                }
                else
                {
                    throw new InvalidOperationException();
                    //children = value;
                }
            }
        }
        /// <summary>
        /// Store the child <see cref="TreeNode"/> of this <see cref="TreeNode"/>.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public ObservableCollection<MessageBase> Messages { get; } = new ObservableCollection<MessageBase>();

        /// <summary>
        /// Store the parent <see cref="TreeNode"/> of this node. Read only.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public TreeNode Parent { get { return _parent; } }

        /// <summary>
        /// Store the next term of double linked list organized tree.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        private TreeNode dblLinkedNext = null;

        /// <summary>
        /// Store the previous term of double linked list organized tree.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        private TreeNode dblLinkedPrev = null;

        /// <summary>
        /// Use this property to get the <see cref="string"/> displayed on screen. 
        /// This property is synced with result of <see cref="ToString"/> method.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public string ScreenString { get { return this.ToString(); } }

        /// <summary>
        /// Use this property to get the attribute count of this node.
        /// </summary>
        public int AttributeCount { get => attributes.Count; }

        /// <summary>
        /// Identify whether ignore parity check for this node.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        protected virtual bool EnableParityCheck { get => true; }

        /// <summary>
        /// Identify whether a <see cref="TreeNode"/> is belong to a proper document. 
        /// This property will determine whether event <see cref="OnCreate"/> and <see cref="OnRemove"/> are invoked.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        protected bool activated = false;

        /// <summary>
        /// Indicates used attributes when checking.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        private HashSet<AttrItem> usedAttributes = null;

        /// <summary>
        /// Event when node is created.
        /// </summary>
        private event OnCreateNodeHandler OnCreate;
        /// <summary>
        /// Event when node switched banned off.
        /// </summary>
        private event OnCreateNodeHandler OnVirtuallyCreate;
        /// <summary>
        /// Event when node is removed.
        /// </summary>
        private event OnRemoveNodeHandler OnRemove;
        /// <summary>
        /// Event when node switched banned on.
        /// </summary>
        private event OnRemoveNodeHandler OnVirtuallyRemove;
        /// <summary>
        /// Event when node have an <see cref="DependencyAttrItem"/> changed.
        /// </summary>
        private event OnDependencyAttributeChangedHandler OnDependencyAttributeItemChanged;

        /// <summary>
        /// Raise <see cref="OnCreate"/> event.
        /// </summary>
        /// <param name="e">Arguments for this event.</param>
        public void RaiseCreate(OnCreateEventArgs e)
        {
            if (e.parent == null || e.parent.activated)
            {
                if (!isBanned)
                {
                    OnVirtuallyCreate?.Invoke(e);
                }
                OnCreate?.Invoke(e);
                OnCreateEventArgs args = new OnCreateEventArgs() { parent = this };
                foreach (TreeNode t in Children)
                {
                    t.RaiseCreate(args);
                }
            }
        }

        /// <summary>
        /// Raise <see cref="OnVirtuallyCreate"/> event.
        /// </summary>
        /// <param name="e">Arguments for this event.</param>
        public void RaiseVirtuallyCreate(OnCreateEventArgs e)
        {
            if (activated && !isBanned)
            {
                OnVirtuallyCreate?.Invoke(e);
                foreach (TreeNode t in Children)
                {
                    t.RaiseVirtuallyCreate(e);
                }
            }
        }

        /// <summary>
        /// Raise <see cref="OnRemove"/> event.
        /// </summary>
        /// <param name="e">Arguments for this event.</param>
        public void RaiseRemove(OnRemoveEventArgs e)
        {
            if (e.parent == null || e.parent.activated)
            {
                if (!isBanned)
                {
                    OnVirtuallyRemove?.Invoke(e);
                }
                OnRemove?.Invoke(e);
                OnCreateEventArgs args = new OnCreateEventArgs() { parent = this };
                foreach (TreeNode t in Children)
                {
                    t.RaiseRemove(e);
                }
            }
        }

        /// <summary>
        /// Raise <see cref="OnVirtuallyRemove"/> event.
        /// </summary>
        /// <param name="e">Arguments for this event.</param>
        public void RaiseVirtuallyRemove(OnRemoveEventArgs e)
        {
            //System.Windows.MessageBox.Show(activated.ToString());
            if (activated && isBanned)
            {
                OnVirtuallyRemove?.Invoke(e);
                foreach (TreeNode t in Children)
                {
                    t.RaiseVirtuallyRemove(e);
                }
            }
        }

        /// <summary>
        /// Raise <see cref="OnDependencyAttributeItemChanged"/> event.
        /// </summary>
        /// <param name="o">Object that trigger this event.</param>
        /// <param name="e">Arguments for this event.</param>
        public void RaiseDependencyPropertyChanged(DependencyAttrItem o, DependencyAttributeChangedEventArgs e)
        {
            OnDependencyAttributeItemChanged?.Invoke(o, e);
        }

        /// <summary>
        /// Event handler for <see cref="ObservableCollection{T}.CollectionChanged"/> in <see cref="Children"/>.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ChildrenChanged(object o, NotifyCollectionChangedEventArgs e)
        {
            TreeNode t, tPrev = null;
            if (e.OldItems != null)
            {
                for (int index = 0; index < e.OldItems.Count; index++)
                {
                    t = (TreeNode)e.OldItems[index];
                    t.RaiseRemove(new OnRemoveEventArgs() { parent = this });
                    if (index + e.OldStartingIndex != 0)
                    {
                        if (tPrev == null)
                        {
                            tPrev = Children[index + e.OldStartingIndex - 1];
                        }
                        tPrev.dblLinkedNext = t.dblLinkedNext;
                        if (t.dblLinkedNext != null) t.dblLinkedNext.dblLinkedPrev = tPrev;
                    }
                    else
                    {
                        this.dblLinkedNext = t.dblLinkedNext;
                        if (this.dblLinkedNext != null) t.dblLinkedNext.dblLinkedPrev = this;
                        tPrev = this;
                    }
                }
            }
            if (e.NewItems != null)
            {
                for (int index = 0; index < e.NewItems.Count; index++)
                {
                    t = (TreeNode)e.NewItems[index];
                    t.RaiseCreate(new OnCreateEventArgs() { parent = this });
                    t._parent = this;
                    //Manage double linked list
                    if (index + e.NewStartingIndex != 0)
                    {
                        if (tPrev == null)
                        {
                            tPrev = Children[index + e.NewStartingIndex - 1];
                        }
                        t.dblLinkedPrev = tPrev;
                        t.dblLinkedNext = tPrev.dblLinkedNext;
                        if (tPrev.dblLinkedNext != null) tPrev.dblLinkedNext.dblLinkedPrev = t;
                        tPrev.dblLinkedNext = t;
                    }
                    else
                    {
                        t.dblLinkedPrev = this;
                        t.dblLinkedNext = this.dblLinkedNext;
                        if (this.dblLinkedNext != null) this.dblLinkedNext.dblLinkedPrev = t;
                        this.dblLinkedNext = t;
                    }
                    tPrev = t;
                }
            }
        }

        private void AttributesChanged(object o, NotifyCollectionChangedEventArgs e)
        {
            AttrItem ai;
            if (e.NewItems != null)
            {
                foreach (var i in e.NewItems)
                {
                    ai = (AttrItem)i;
                    if (ai != null)
                    {
                        ai.Parent = this;
                    }
                }
            }
        }

        /// <summary>
        /// Rearrange all attributes by a given source.
        /// </summary>
        /// <param name="source">Source <see cref="TreeNode"/>.</param>
        public void DeepCopyFrom(TreeNode source)
        {
            var attrs = from AttrItem a in source.attributes select (AttrItem)a.Clone();
            var childrens = from TreeNode t in source.Children select (TreeNode)t.Clone();
            Attributes = null;
            foreach (AttrItem ai in attrs)
            {
                attributes.Add(ai);
            }
            Children = null;
            foreach (TreeNode treeNode in childrens)
            {
                Children.Add(treeNode);
            }
            _parent = source._parent;
            isExpanded = source.isExpanded;
            isBanned = source.isBanned;
        }

        /// <summary>
        /// The constructor used by Serializer. Classes who inherit it must override this.
        /// </summary>
        protected TreeNode()
        {
            PropertyChanged += new PropertyChangedEventHandler(CheckMessage);
            OnDependencyAttributeItemChanged += new OnDependencyAttributeChangedHandler(ReflectAttr);
            OnVirtuallyCreate += new OnCreateNodeHandler(CreateMeta);
            OnCreate += new OnCreateNodeHandler(CreatedActivation);
            OnVirtuallyRemove += new OnRemoveNodeHandler(RemoveMeta);
            OnRemove += new OnRemoveNodeHandler(RemovedDeactivation);
            Children = null;
            Attributes = null;
            isExpanded = true;
        }

        /// <summary>
        /// The constructor initialize the <see cref="parentWorkSpace"/> property.
        /// </summary>
        /// <param name="workSpaceData"> the given <see cref="DocumentData"/> </param>
        public TreeNode(DocumentData workSpaceData) : this()
        {
            parentWorkSpace = workSpaceData;
        }

        /// <summary>
        /// This method gets the <see cref="string"/> displayed on screen.
        /// </summary>
        /// <returns> a <see cref="string"/> displayed on screen </returns>
        public override string ToString()
        {
            return "";
        }

        /// <summary>
        /// This method gets the lua code of current node and its childs. 
        /// It is a Compile Method.
        /// </summary>
        /// <param name="spacing"> The spacing before each line of lua code. must be unsigned. </param>
        /// <returns> The <see cref="string"/> of lua code. </returns>
        public virtual IEnumerable<string> ToLua(int spacing)
        {
            return ToLua(spacing, children);
        }

        /// <summary>
        /// This method gets the lua code of current node and its childs. 
        /// It is a Compile Method.
        /// </summary>
        /// <param name="spacing"> The spacing before each line of lua code. must be unsigned. </param>
        /// <param name="children"> The children enumerable of child of this node. </param>
        /// <returns> The <see cref="string"/> of lua code. </returns>
        protected IEnumerable<string> ToLua(int spacing, IEnumerable<TreeNode> children)
        {
            bool childof = false;
            TreeNode temp = GlobalCompileData.StageDebugger;
            if (GlobalCompileData.StageDebugger != null && PluginHandler.Plugin.MatchStageNodeTypes(Parent?.GetType()))
            {
                while (temp._parent != null)
                {
                    if (temp._parent == this)
                    {
                        childof = true;
                        break;
                    }
                    temp = temp._parent;
                }
            }

            bool firstC = false;
            bool folderFound = false;
            bool equalFound = false;
            foreach (TreeNode t in children)
            {
                if (!t.isBanned)
                {
                    if (GlobalCompileData.SCDebugger == t)
                    {
                        foreach (var a in t.ToLua(spacing))
                        {
                            yield return a;
                        }
                        string difficultyS = (NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1));
                        string fullName = "\"" + NonMacrolize(0) + difficultyS + "\"";
                        yield return "_boss_class_name=" + fullName;
                        yield return " _editor_class[" + fullName + "].cards={boss.move.New(0,144,60,MOVE_NORMAL),_tmp_sc} ";
                    }
                    else if (GlobalCompileData.StageDebugger != null)
                    {
                        if (childof)
                        {
                            if(!firstC && folderFound && !equalFound)
                            {
                                firstC = true;
                                yield return "if false then ";
                            }
                            if (!folderFound && t is Folder)
                            {
                                folderFound = true;
                            }
                            if (GlobalCompileData.StageDebugger == t)
                            {
                                equalFound = true;
                                if (firstC)
                                {
                                    yield return "end ";
                                }
                            }
                        }
                        foreach (var a in t.ToLua(spacing))
                        {
                            yield return a;
                        }
                    }
                    else
                    {
                        foreach (var a in t.ToLua(spacing))
                        {
                            yield return a;
                        }
                    }
                    t.AddCompileSettings();
                }
            }
        }

        /// <summary>
        /// Get all <see cref="MessageBase"/> that generated by this node.
        /// </summary>
        /// <returns>
        /// A List of <see cref="MessageBase"/> that generated by this node.
        /// </returns>
        public virtual List<MessageBase> GetMessage()
        {
            return new List<MessageBase>();
        }

        /// <summary>
        /// Get <see cref="MessageBase"/> that indicates attribute mismatch to standard node.
        /// </summary>
        /// <returns>
        /// A List of <see cref="MessageBase"/> that generated by this node.
        /// </returns>
        private List<MessageBase> GetMismatchedAttributeMessage()
        {
            var a = new List<MessageBase>();
            if (PluginHandler.Plugin.NodeTypeCache.StandardNode.ContainsKey(GetType()))
            {
                if (parentWorkSpace == null) return a;
                TreeNode t = PluginHandler.Plugin.NodeTypeCache.StandardNode[GetType()].Clone() as TreeNode;
                if (t.GetType() != GetType()) return a;
                for (int i = 0; i < 2; i++)
                {
                    List<Tuple<int, int>> relatedMatchPairs = new List<Tuple<int, int>>();
                    var thisRelatedAttr = new ObservableCollection<AttrItem>(from AttrItem ai
                                                                             in attributes
                                                                             where ai is DependencyAttrItem
                                                                             select ai);
                    var standardRelatedAttr = new ObservableCollection<AttrItem>(from AttrItem ai
                                                                                 in t.attributes
                                                                                 where ai is DependencyAttrItem
                                                                                 select ai);
                    GetMismatchByAttributes(a, relatedMatchPairs, thisRelatedAttr, standardRelatedAttr);
                    foreach (Tuple<int, int> tup in relatedMatchPairs)
                    {
                        standardRelatedAttr[tup.Item2].AttrInput = thisRelatedAttr[tup.Item1].AttrInput;
                        thisRelatedAttr[tup.Item1].EditWindow = standardRelatedAttr[tup.Item2].EditWindow;
                    }
                }
                List<Tuple<int, int>> matchPairs = new List<Tuple<int, int>>();
                GetMismatchByAttributes(a, matchPairs, attributes, t.attributes);
                foreach (Tuple<int, int> tup in matchPairs)
                {
                    attributes[tup.Item1].EditWindow = t.attributes[tup.Item2].EditWindow;
                }
            }
            return a;
        }

        /// <summary>
        /// Given two list of <see cref="AttrItem"/>, compare them and add mismatch attributes to a
        /// <see cref="MessageBase"/> list.
        /// </summary>
        /// <param name="a"> The <see cref="MessageBase"/>list taking <see cref="MessageBase"/>.</param>
        /// <param name="matchPairs"> The <see cref="int"/>-<see cref="int"/> map between same attributes.</param>
        /// <param name="thisAttribute"><see cref="AttrItem"/> list to be tested.</param>
        /// <param name="StandardAttribute"><see cref="AttrItem"/> list regarded as standard.</param>
        private void GetMismatchByAttributes(List<MessageBase> a, List<Tuple<int, int>> matchPairs,
            Collection<AttrItem> thisAttribute, Collection<AttrItem> StandardAttribute)
        {
            bool found;
            //Get map between attributes of this and standard, alongwith getting all in this but not in standard.
            for (int i = 0; i < thisAttribute.Count; i++)
            {
                found = false;
                for (int j = 0; j < StandardAttribute.Count; j++)
                {
                    if (thisAttribute[i].AttrCap == StandardAttribute[j].AttrCap)
                    {
                        //repeat reference by attribute in this
                        if (found) a.Add(new AttributeMismatchMessage(thisAttribute[i].AttrCap, 0, this));
                        found = true;
                        matchPairs.Add(new Tuple<int, int>(i, j));
                    }
                }
                if (!found)
                {
                    a.Add(new AttributeMismatchMessage(thisAttribute[i].AttrCap, 0, this));
                }
            }
            matchPairs.Sort((Tuple<int, int> t1, Tuple<int, int> t2) => t1.Item2.CompareTo(t2.Item2));
            int t1l = -1, t2l = -1;
            bool isDisordered = false;
            for (int i = 0; i < matchPairs.Count; i++)
            {
                if (matchPairs[i].Item2 == t2l)
                {
                    //repeat reference by attribute in standard
                    a.Add(new AttributeMismatchMessage(thisAttribute[matchPairs[i].Item1].AttrCap, 0, this));
                }
                else
                {
                    //getting all in standard but not in this before last match.
                    for (int j = t2l + 1; j < matchPairs[i].Item2; j++)
                    {
                        a.Add(new AttributeMismatchMessage(StandardAttribute[j].AttrCap, 0, this));
                    }
                }
                //getting first disordered
                if (!isDisordered && matchPairs[i].Item1 < t1l)
                {
                    a.Add(new AttributeMismatchMessage(thisAttribute[matchPairs[i].Item1].AttrCap, 0, this));
                    isDisordered = true;
                }
                else
                {
                    t1l = matchPairs[i].Item1;
                }
                t2l = matchPairs[i].Item2;
            }
            //getting all in standard but not in this after last match.
            for (int i = t2l + 1; i < StandardAttribute.Count; i++)
            {
                a.Add(new AttributeMismatchMessage(StandardAttribute[i].AttrCap, 0, this));
            }
        }

        /// <summary>
        /// Event handler that check and updates messages.
        /// </summary>
        /// <param name="sender">Sender of the event. Useless.</param>
        /// <param name="e">Arguments. <code>"Selected"</code> or <code>"Expanded"</code> are ignored.</param>
        public void CheckMessage(object sender, PropertyChangedEventArgs e)
        {
            if (e?.PropertyName != "Selected" && e?.PropertyName != "Expanded"
                && parentWorkSpace != null && !parentWorkSpace.SupressMessage
                && (GetType() == typeof(RootFolder) || GetType() == typeof(ProjectRoot) || _parent != null))
            {
                List<MessageBase> a = new List<MessageBase>();
                if (!isBanned)
                {
                    a = GetMessage();
                }
                if (EnableParityCheck) a.AddRange(GetMismatchedAttributeMessage());
                Messages.Clear();
                foreach (MessageBase mb in a)
                {
                    Messages.Add(mb);
                }
                MessageContainer.UpdateMessage(this);
            }
            if (parentWorkSpace != null && !parentWorkSpace.SupressMessage)
            {
                parentWorkSpace?.OriginalMeta.CheckMessage(null, new PropertyChangedEventArgs(""));
            }
        }

        /// <summary>
        /// This method modify the current node when a <see cref="DependencyAttrItem"/> changed.
        /// It can only modify items after those who call it, and cannot modify <see cref="AttrItem"/> hierarchically.
        /// </summary>
        /// <param name="relatedAttrItem"> The <see cref="DependencyAttrItem"/> who call this. </param>
        /// <param name="originalvalue"> The original <see cref="DependencyAttrItem.AttrInput"/> value before it was changed. </param>
        public virtual void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs e) { }

        protected virtual void AddCompileSettings() { }

        /// <summary>
        /// This method gets the <see cref="MetaInfo"/> generated by this node.
        /// </summary>
        /// <returns></returns>
        public virtual MetaInfo GetMeta()
        {
            return null;
        }

        /// <summary>
        /// This item gets lua line information of <see cref="TreeNode"/>. Must be synced with <see cref="ToLua(int)"/>.
        /// </summary>
        /// <returns>
        /// the <see cref="Tuple"/> of <see cref="int"/> and <see cref="TreeNode"/> 
        /// which stores the information of line - TreeNode relations.
        /// </returns>
        public abstract IEnumerable<Tuple<int, TreeNode>> GetLines();

        /// <summary>
        /// This method is used to invoke when getting lua line information of child <see cref="TreeNode"/>s.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<Tuple<int, TreeNode>> GetChildLines()
        {
            foreach (TreeNode t in Children)
            {
                if (!t.isBanned)
                {
                    foreach (Tuple<int, TreeNode> ti in t.GetLines())
                    {
                        yield return ti;
                    }
                }
            }
        }

        /// <summary>
        /// Get difficulty of the current node. recursive.
        /// </summary>
        /// <returns> A string of difficulty. <code>null</code> if parent is <code>null</code></returns>
        public virtual string GetDifficulty()
        {
            return _parent?.GetDifficulty();
        }

        /// <summary>
        /// Validate whether the given node can be the child of this node.
        /// </summary>
        /// <param name="toV">
        /// The given node.
        /// </param>
        /// <returns>
        /// A boolean, true for can.
        /// </returns>
        public bool ValidateChild(TreeNode toV)
        {
            return ValidateChild(toV, this);
        }

        /// <summary>
        /// Validate whether the given node can be the child of this node.
        /// </summary>
        /// <param name="toV">
        /// The given node.
        /// </param>
        /// <param name="toV">
        /// The orginal parent for searching.
        /// </param>
        /// <returns>
        /// A boolean, true for can.
        /// </returns>
        private bool ValidateChild(TreeNode toV, TreeNode originalParent)
        {
            if (this is Folder) return GetLogicalParent()?.ValidateChild(toV, originalParent) ?? true;
            if (toV is Folder)
            {
                foreach (TreeNode t in toV.GetLogicalChildren())
                {
                    if (!ValidateChild(t, originalParent)) return false;
                }
                return true;
            }
            if (PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].leaf) return false;
            var e = this != originalParent 
                ? this.GetLogicalChildren().Concat(originalParent.GetLogicalChildren()).Distinct()
                : GetLogicalChildren();
            if (PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[toV.GetType()].uniqueness)
            {
                e = e.Concat(new TreeNode[] { toV }.AsEnumerable());
            }
            if (!MatchUniqueness(e)) return false;
            if (!toV.MatchParents(this)) return false;
            Stack<TreeNode> stack = new Stack<TreeNode>();
            stack.Push(toV);
            TreeNode cur;
            while (stack.Count != 0)
            {
                cur = stack.Pop();
                if (!(cur.FindAncestorIn(cur, toV.Parent, this, null))) return false;
                if (PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[cur.GetType()].classNode)
                {
                    if (!(MatchClassNode(cur, toV.Parent) || MatchClassNode(this, null))) return false;
                }
                foreach (TreeNode t in cur.Children)
                {
                    stack.Push(t);
                }
            }
            return true;
        }

        /// <summary>
        /// --USELESS--
        /// Validate whether the given node can be the child of this node.
        /// --USELESS--
        /// </summary>
        /// <param name="toV">
        /// The given node.
        /// </param>
        /// <returns>
        /// A boolean, true for can.
        /// </returns>
        [Obsolete]
        private bool ValidateChild_Obs(TreeNode toV)
        {
            if (MatchType(toV.GetType(), new Type[] { typeof(IfThen), typeof(IfElse) })) return false;
            foreach (TreeNode t in toV.Children)
            {
                if (Parent != null) if (!Parent.ValidateChild(t)) return false;
            }
            return true;
        }

        /// <summary>
        /// This method is used to write <see cref="TreeNode"/> information to file.
        /// </summary>
        /// <param name="level"> the depth of current node </param>
        /// <returns> the <see cref="string"/> to write into file </returns>
        [Obsolete]
        public string Serialize(int level)
        {
            string s = "" + level + "," + EditorSerializer.SerializeTreeNode(this) + "\n";
            foreach (TreeNode t in Children)
            {
                s += t.Serialize(level + 1);
            }
            return s;
        }

        /// <summary>
        /// This method is used to write <see cref="TreeNode"/> information to file.
        /// </summary>
        /// <param name="fs"> The <see cref="FileStream"/> object of file </param>
        /// <param name="level"> the depth of current node </param>
        public void SerializeFile(StreamWriter fs, int level)
        {
            fs.WriteLine("" + level + "," + EditorSerializer.SerializeTreeNode(this));
            foreach (TreeNode t in Children)
            {
                t.SerializeFile(fs, level + 1);
            }
        }

        #region tree

        /// <summary>
        /// Add a child to this node. also update meta and messages.
        /// </summary>
        /// <param name="n"><see cref="TreeNode"/> to add.</param>
        public void AddChild(TreeNode n)
        {
            Children.Add(n);
            //n._parent = this;
            //n.RaiseCreate(new OnCreateEventArgs() { parent = this });
        }

        /// <summary>
        /// Insert a child to this node. also update meta and messages.
        /// </summary>
        /// <param name="n"><see cref="TreeNode"/> to insert.</param>
        /// <param name="index">ID to insert</param>
        public void InsertChild(TreeNode n, int index)
        {
            Children.Insert(index, n);
            //n._parent = this;
            //n.RaiseCreate(new OnCreateEventArgs() { parent = this });
        }

        /// <summary>
        /// Remove a child from tree. also update meta and messages.
        /// </summary>
        /// <param name="t"><see cref="TreeNode"/> to remove.</param>
        public void RemoveChild(TreeNode t)
        {
            Children.Remove(t);
            //t.RaiseRemove(new OnRemoveEventArgs() { parent = this });
        }

        /// <summary>
        /// Activation after created.
        /// </summary>
        private void CreatedActivation(OnCreateEventArgs e)
        {
            activated = true;
        }

        /// <summary>
        /// Deactivation after removed.
        /// </summary>
        private void RemovedDeactivation(OnRemoveEventArgs e)
        {
            activated = false;
        }

        /// <summary>
        /// Get an <see cref="AttrItem"/> by id.
        /// </summary>
        /// <param name="n">ID of a <see cref="AttrItem"/>.</param>
        /// <returns>The targeted <see cref="AttrItem"/> if in bound, otherwise null.</returns>
        private AttrItem GetAttr(int n)
        {
            if (attributes.Count > n)
            {
                return attributes[n];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get an <see cref="AttrItem"/> by name.
        /// </summary>
        /// <param name="name">Name of a <see cref="AttrItem"/>.</param>
        /// <returns>The targeted <see cref="AttrItem"/> if found, otherwise null.</returns>
        private AttrItem GetAttr(string name)
        {
            var attrs = from AttrItem ai in attributes
                        where ai != null && !string.IsNullOrEmpty(ai.AttrCap) && ai.AttrCap == name
                        select ai;
            if (attrs != null && attrs.Count() > 0) return attrs.First();
            return null;
        }

        /// <summary>
        /// Insert <see cref="AttrItem"/> in a desired place. If place is out of bounds, create null entries.
        /// </summary>
        /// <param name="id">The desired ID.</param>
        /// <param name="target">The <see cref="AttrItem"/></param>
        private void InsertAttrAt(int id, AttrItem target)
        {
            if (id >= attributes.Count)
            {
                for (int i = attributes.Count; i < id; i++)
                {
                    attributes.Add(null);
                }
                attributes.Insert(id, target);
            }
            else
            {
                target.attrInput = attributes[id]?.attrInput ?? "";
                attributes[id] = target;
            }
        }

        /// <summary>
        /// Get an <see cref="AttrItem"/> check by both id and name. If not found, create one.
        /// </summary>
        /// <param name="id">ID of a <see cref="AttrItem"/>.</param>
        /// <param name="defaultEditWindow">Indicate default editwindow property.</param>
        /// <param name="name">
        /// Name of a <see cref="AttrItem"/>. 
        /// It will set to caller's name if no <see cref="string"/> is assigned to it.
        /// </param>
        /// <param name="isDependency">Indicate whether a default <see cref="AttrItem"/> 
        /// is <see cref="DependencyAttrItem"/>.</param>
        /// <returns>The targeted <see cref="AttrItem"/> if found, otherwise a default <see cref="AttrItem"/>.</returns>
        public AttrItem DoubleCheckAttr(int id, string defaultEditWindow = ""
            , [CallerMemberName] string name = "", bool isDependency = false)
        {
            AttrItem ai = GetAttr(id);
            if (ai == null || string.IsNullOrEmpty(ai.AttrCap) || ai.AttrCap != name)
            {
                ai = GetAttr(name);
                if (ai != null)
                {
                    SwapAttr(id, ai);
                }
                else
                {
                    if (isDependency)
                    {
                        ai = new DependencyAttrItem(name, "", defaultEditWindow);
                    }
                    else
                    {
                        ai = new AttrItem(name, "", defaultEditWindow);
                    }
                    InsertAttrAt(id, ai);
                }
            }
            ai.EditWindow = defaultEditWindow;
            usedAttributes?.Remove(ai);
            return ai;
        }

        /// <summary>
        /// Place an <see cref="AttrItem"/> to a place of a given id, then put the <see cref="AttrItem"/> replaced to the place 
        /// that <see cref="AttrItem"/> located before.
        /// </summary>
        /// <param name="id">The ID of the target place.</param>
        /// <param name="ai">The <see cref="AttrItem"/> to place.</param>
        private void SwapAttr(int id, AttrItem ai)
        {
            if (id >= attributes.Count)
            {
                for (int i = attributes.Count; i <= id; i++)
                {
                    attributes.Add(null);
                }
            }
            int id2 = attributes.IndexOf(ai);
            AttrItem ai2 = attributes[id];
            attributes[id2] = ai2;
            attributes[id] = ai;
        }

        #endregion

        /// <summary>
        /// Create related <see cref="MetaInfo"/> and <see cref="MessageBase"/>.
        /// </summary>
        /// <param name="e">Argument for event.</param>
        private void CreateMeta(OnCreateEventArgs e)
        {
            MetaInfo meta = GetMeta();
            RaiseProertyChanged("m");
            meta?.Create(meta, parentWorkSpace.OriginalMeta);
        }

        /// <summary>
        /// Remove related <see cref="MetaInfo"/> and <see cref="MessageBase"/>.
        /// </summary>
        /// <param name="e">Argument for event.</param>
        private void RemoveMeta(OnRemoveEventArgs e)
        {
            MetaInfo meta = this.GetMeta();
            if (meta != null) meta.Remove(meta, parentWorkSpace.OriginalMeta);
            var s = from MessageBase mb
                    in MessageContainer.Messages
                    where mb.Source == this
                    select mb;
            //DO NOT REMOVE LINE BELOW! LINQ CREATE INDEXES INSTEAD OF STATIC LIST
            List<MessageBase> lst = new List<MessageBase>(s);
            foreach (MessageBase mb in lst)
            {
                MessageContainer.Messages.Remove(mb);
            }
        }

        /// <summary>
        /// Get <see cref="AttrItem"/> that to be edited when create this node.
        /// </summary>
        /// <returns>The<see cref="AttrItem"/> that to be edited when create this node.</returns>
        public AttrItem GetCreateInvoke()
        {
            int? idt = PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].createInvokeID;
            return idt.HasValue ? attributes[idt.Value] : null;
        }

        /// <summary>
        /// Get <see cref="AttrItem"/> that to be edited when right clicked.
        /// </summary>
        /// <returns>The<see cref="AttrItem"/> that to be edited when right clicked.</returns>
        public AttrItem GetRCInvoke()
        {
            int? idt = PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].rightClickInvokeID;
            return idt.HasValue ? attributes[idt.Value] : null;
        }

        /// <summary>
        /// The event of property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The method that raise property changed.
        /// </summary>
        /// <param name="propName">The parameter of event.</param>
        public void RaiseProertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// The method that returns a deep copy of this <see cref="TreeNode"/>. 
        /// Only specific types of <see cref="TreeNode"/> may return distinct typed object.
        /// This behaviour in plugin is prohibited.
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        /// <summary>
        /// This method fixes parent in child attributes.
        /// </summary>
        public void FixAttrParent()
        {
            foreach (AttrItem ai in attributes)
            {
                ai.Parent = this;
            }
        }

        /// <summary>
        /// This method fixes parent in child nodes.
        /// </summary>
        public void FixChildrenParent()
        {
            foreach (TreeNode t in Children)
            {
                t._parent = this;
            }
        }

        /// <summary>
        /// This method fixes banned situation.
        /// </summary>
        public void FixBan()
        {
            if (isBanned)
            {
                RemoveMeta(new OnRemoveEventArgs());
            }
            else
            {
                CreateMeta(new OnCreateEventArgs());
            }
            foreach (TreeNode t in Children)
            {
                t.FixBan();
            }
        }

        /// <summary>
        /// This method fixes parent <see cref="DocumentData"/> in both this and child nodes.
        /// </summary>
        public void FixParentDoc(DocumentData document)
        {
            parentWorkSpace = document;
            foreach (TreeNode t in Children)
            {
                t.FixParentDoc(document);
            }
        }

        /// <summary>
        /// This method tests whether the given <see cref="Type"/> is in a given <see cref="Type"/> array.
        /// </summary>
        /// <param name="a">The given <see cref="Type"/>.</param>
        /// <param name="types">The given <see cref="Type"/> array.</param>
        protected static bool MatchType(Type a, Type[] types)
        {
            bool found = false;
            foreach (Type t in types)
            {
                found = found || a.Equals(t);
            }
            return found;
        }

        /// <summary>
        /// This method tests whether a group of nodes <see cref="TreeNode"/> can be unique one if marked as uniqueness.
        /// </summary>
        /// <param name="logicalChildren">The group of nodes.</param>
        private static bool MatchUniqueness(IEnumerable<TreeNode> logicalChildren)
        {
            if (logicalChildren == null) return false;
            var info = PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo;
            HashSet<Type> foundTypes = new HashSet<Type>();
            foreach (TreeNode t in logicalChildren)
            {
                if (info[t.GetType()].uniqueness && foundTypes.Contains(t.GetType())) return false;
                foundTypes.Add(t.GetType());
            }
            return true;
        }

        /// <summary>
        /// This method tests whether this <see cref="TreeNode"/> have a direct parent of any type discribed in
        /// <see cref="TypeCacheData"/>.
        /// </summary>
        /// <param name="toMatch">The given <see cref="TreeNode"/> as parent.</param>
        private bool MatchParents(TreeNode toMatch)
        {
            Type[] ts = PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].requireParent;
            if (toMatch == null) return false;
            if (ts == null) return true;
            if (toMatch.IgnoreValidation) return true;
            foreach (Type t in ts)
            {
                if (toMatch.GetType().Equals(t)) return true;
            }
            return false;
        }

        /// <summary>
        /// This method tests whether nodes in the given range are folders.
        /// </summary>
        /// <param name="beg">The node with lowest depth.</param>
        /// <param name="end">The parent of node with highest depth.</param>
        private static bool MatchClassNode(TreeNode beg, TreeNode end)
        {
            while (beg != end)
            {
                if (beg.IgnoreValidation) return true;
                if (beg.GetType() != typeof(Folder) && beg.GetType() != typeof(RootFolder) && beg.GetType() != typeof(ProjectRoot)) return false;
                beg = beg.Parent;
            }
            return true;
        }

        /// <summary>
        /// This method tests whether this nodes satisfy ancestor condition in two given ranges.
        /// </summary>
        /// <param name="Beg1">The node with lowest depth in range 1.</param>
        /// <param name="End1">The parent of node with highest depth in range 1.</param>
        /// <param name="Beg2">The node with lowest depth in range 2.</param>
        /// <param name="End2">The parent of node with highest depth in range 2.</param>
        private bool FindAncestorIn(TreeNode Beg1, TreeNode End1, TreeNode Beg2, TreeNode End2)
        {
            Type[][] ts = PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].requireAncestor;
            if (ts == null) return true;
            List<Type[]> toSatisfiedGroups = ts.ToList();
            List<Type> Satisfied = new List<Type>();
            List<Type[]> toRemove = new List<Type[]>();
            while (Beg1 != End1)
            {
                if (Beg1.IgnoreValidation) return true;
                foreach (Type[] t1 in ts)
                {
                    foreach (Type t2 in t1)
                    {
                        if (Beg1.GetType().Equals(t2)) Satisfied.Add(t2);
                    }
                }
                foreach (Type[] t1 in toSatisfiedGroups)
                {
                    foreach (Type t2 in t1)
                    {
                        foreach (Type t3 in Satisfied)
                        {
                            if (t2 == t3 && !toRemove.Contains(t1)) toRemove.Add(t1);
                        }
                    }
                }
                foreach (Type[] t1 in toRemove)
                {
                    toSatisfiedGroups.Remove(t1);
                }
                if (toSatisfiedGroups.Count == 0) return true;
                Satisfied.Clear();
                toRemove.Clear();
                Beg1 = Beg1.Parent;
            }
            while (Beg2 != End2)
            {
                if (Beg2.IgnoreValidation) return true;
                foreach (Type[] t1 in ts)
                {
                    foreach (Type t2 in t1)
                    {
                        if (Beg2.GetType().Equals(t2)) Satisfied.Add(t2);
                    }
                }
                foreach (Type[] t1 in toSatisfiedGroups)
                {
                    foreach (Type t2 in t1)
                    {
                        foreach (Type t3 in Satisfied)
                        {
                            if (t2 == t3 && !toRemove.Contains(t1)) toRemove.Add(t1);
                        }
                    }
                }
                foreach (Type[] t1 in toRemove)
                {
                    toSatisfiedGroups.Remove(t1);
                }
                if (toSatisfiedGroups.Count == 0) return true;
                Satisfied.Clear();
                toRemove.Clear();
                Beg2 = Beg2.Parent;
            }
            return false;
        }

        /// <summary>
        /// This method execute a macro to a string by given<see cref="Compile.DefineMarcoSettings"/>.
        /// </summary>
        /// <param name="marco">The <see cref="Compile.DefineMarcoSettings"/> contains macro information.</param>
        /// <param name="original">The original string.</param>
        /// <returns>A string after applying macro+.</returns>
        public static string ExecuteMarco(Compile.DefineMarcoSettings marco, string original)
        {
            //Old one considering only "" pairs.
            //Regex regex = new Regex("\\b" + marco.ToBeReplaced + "\\b" + @"(?<=^([^""]*(""[^""]*"")+)*[^""]*.)");
            //Considering both "" and '' pairs, also with \" handled.
            //(?<=^([^"]*((?<!\\)"([^"]|(\\"))*(?<!\\)")+)*[^"]*.)(?<=^([^']*((?<!\\)'([^']|(\\'))*(?<!\\)')+)*[^']*.)
            //final version below also considers \\" conditions.
            //(?<=^([^"]*((?<!(^|[^\\])(\\\\)*\\)"([^"]|((?<=(^|[^\\])(\\\\)*\\)"))*(?<!(^|[^\\])(\\\\)*\\)")+)*[^"]*.)
            //(?<=^([^']*((?<!(^|[^\\])(\\\\)*\\)'([^']|((?<=(^|[^\\])(\\\\)*\\)'))*(?<!(^|[^\\])(\\\\)*\\)')+)*[^']*.)
            Regex regex = new Regex("\\b" + marco.ToBeReplaced + "\\b"
                + @"(?<=^([^""]*((?<!(^|[^\\])(\\\\)*\\)""([^""]|((?<=(^|[^\\])(\\\\)*\\)""))*(?<!(^|[^\\])(\\\\)*\\)"")+)*[^""]*.)"
                + @"(?<=^([^']*((?<!(^|[^\\])(\\\\)*\\)'([^']|((?<=(^|[^\\])(\\\\)*\\)'))*(?<!(^|[^\\])(\\\\)*\\)')+)*[^']*.)");
            return regex.Replace(original, marco.New);
        }

        /// <summary>
        /// This method executes all macros currently in the <see cref="CompileProcess"/> to an <see cref="AttrItem"/>.
        /// Can only be called in Compile Methods.
        /// </summary>
        /// <param name="attrItem">The target <see cref="AttrItem"/></param>
        /// <returns>The <see cref="string"/> after applying macros.</returns>
        protected string Macrolize(AttrItem attrItem)
        {
            string s = attrItem.AttrInput;
            foreach (Compile.DefineMarcoSettings m in parentWorkSpace.CompileProcess.marcoDefinition)
            {
                s = ExecuteMarco(m, s);
            }
            return s;
        }

        /// <summary>
        /// This method executes all macros currently in the <see cref="CompileProcess"/> to an<see cref="AttrItem"/>.
        /// Can only be called in Compile Methods.
        /// </summary>
        /// <param name="i">
        /// The id of target <see cref="AttrItem"/>.
        /// Case of <see cref="UnidentifiedNode"/> has taken into account.
        /// </param>
        /// <returns>The <see cref="string"/> after applying macros.</returns>
        protected string Macrolize(int i)
        {
            if (GetType() != typeof(UnidentifiedNode))
            {
                return Macrolize(attributes[i]);
            }
            else
            {
                return Macrolize(attributes[i + 1]);
            }
        }

        /// <summary>
        /// This method gets the directly contents of inputs in a <see cref="AttrItem"/>.
        /// </summary>
        /// <param name="attrItem">The target <see cref="AttrItem"/></param>
        /// <returns>The <see cref="string"/> of inputs.</return
        protected string NonMacrolize(AttrItem attrItem)
        {
            return attrItem.AttrInput;
        }

        /// <summary>
        /// This method gets the directly contents of inputs in a <see cref="AttrItem"/>.
        /// </summary>
        /// <param name="i">
        /// The id of target <see cref="AttrItem"/>.
        /// Case of <see cref="UnidentifiedNode"/> has taken into account.
        /// </param>
        /// <returns>The <see cref="string"/> of inputs.</return
        public string NonMacrolize(int i)
        {
            if (GetType() != typeof(UnidentifiedNode))
            {
                return NonMacrolize(attributes[i]);
            }
            else
            {
                return NonMacrolize(attributes[i + 1]);
            }
        }

        /// <summary>
        /// Get the path string of a given attribute.
        /// </summary>
        /// <param name="pathAttrID">The id of attribute</param>
        /// <returns><see cref="string"/> after applying archive space and lua escape sequences.</returns>
        protected string GetPath(int pathAttrID)
        {
            if ((App.Current as App).BatchPacking) return Path.GetFileName(NonMacrolize(pathAttrID));
            string s = parentWorkSpace.CompileProcess.archiveSpace;
            if (s != "")
            {
                if (!s.EndsWith("\\") && !s.EndsWith("/"))
                {
                    s = "";
                }
                else
                {
                    char[] cs = Path.GetInvalidPathChars();
                    foreach (char c in cs)
                    {
                        if (s.Contains(c))
                        {
                            s = "";
                            break;
                        }
                    }
                }
            }
            return Lua.StringParser.ParseLua(s + Path.GetFileName(NonMacrolize(pathAttrID)));
        }

        /// <summary>
        /// UNFINISHED
        /// </summary>
        [Obsolete]
        public void FixAttr()
        {
            int i = 0, j = 0;
            TreeNode Standard = PluginHandler.Plugin.NodeTypeCache.StandardNode[GetType()];
            while (j <= Standard.attributes.Count)
            {
                if (attributes[i].AttrCap == Standard.attributes[j].AttrCap)
                {
                    attributes[i].EditWindow = Standard.attributes[i].EditWindow;
                    i++;
                }
                else
                {
                    attributes.Insert(i, Standard.attributes[j].Clone() as AttrItem);
                }
                j++;
            }
        }

        /// <summary>
        /// This method checks name of varible name contents
        /// </summary>
        /// <param name="s">The target string</param>
        /// <returns>a bool.</returns>
        public static bool CheckVarName(string s)
        {
            Regex regex = new Regex("^[a-zA-Z_][\\w\\d_]*$");
            return regex.IsMatch(s);
        }

        /// <summary>
        /// This method try to simulate parent and child relation for <see cref="Commands.Factory.ParentFac"/> 
        /// by its parent property (change to ones like <see cref="Commands.Factory.ChildFac"/>) but not actually insert them. 
        /// Only for validation.
        /// </summary>
        /// <param name="parent">The assumed parent.</param>
        /// <param name="child">The assumed child.</param>
        /// <returns>parent of original <paramref name="parent"/></returns>
        public static TreeNode TryLink(TreeNode parent, TreeNode child)
        {
            TreeNode toInsP = parent._parent;
            parent._parent = child._parent;
            child._parent = parent;
            return toInsP;
        }

        /// <summary>
        /// This method revert what <see cref="TryLink(TreeNode, TreeNode)"/> did. Only for validation.
        /// </summary>
        /// <param name="parent">The assumed parent.</param>
        /// <param name="child">The assumed child.</param>
        /// <param name="originalpp">parent of original <paramref name="parent"/></param>
        public static void TryUnlink(TreeNode parent, TreeNode child, TreeNode originalpp)
        {
            child._parent = parent._parent;
            parent._parent = originalpp;
        }

        /// <summary>
        /// This method expands all child nodes in this branch.
        /// </summary>
        public void ExpandTree()
        {
            this.IsExpanded = true;
            foreach (TreeNode t in children)
            {
                t.ExpandTree();
            }
        }

        /// <summary>
        /// This method folds all child nodes in this branch.
        /// </summary>
        public void FoldTree()
        {
            this.IsExpanded = false;
            foreach (TreeNode t in children)
            {
                t.FoldTree();
            }
        }

        /// <summary>
        /// Get the enumerator fowardly iterate the rest of the tree.
        /// </summary>
        /// <returns>An enumerator of the tree.</returns>
        public IEnumerator<TreeNode> GetFowardEnumerator()
        {
            TreeNode t = this;
            while (t.dblLinkedNext != null)
            {
                yield return t;
                t = t.dblLinkedNext;
            }
        }

        /// <summary>
        /// Get the enumerator backwardly iterate the rest of the tree.
        /// </summary>
        /// <returns>An enumerator of the tree.</returns>
        public IEnumerator<TreeNode> GetBackwardEnumerator()
        {
            TreeNode t = this;
            while (t.dblLinkedPrev != null)
            {
                yield return t;
                t = t.dblLinkedPrev;
            }
        }

        /// <summary>
        /// Get a default <see cref="string"/> if source is null or empty, otherwise return itself.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="def">The default <see cref="string"/>.</param>
        /// <returns>A <see cref="string"/> based on source itself.</returns>
        public static string NullOrDefault(string source, string def = "")
        {
            return string.IsNullOrEmpty(source) ? def : source;
        }

        /// <summary>
        /// Get the indirect parent of the current node, that is, folder means piercing.
        /// </summary>
        /// <returns>A <see cref="TreeNode"/>, null if not found.</returns>
        public TreeNode GetLogicalParent()
        {
            TreeNode p = _parent;
            while ((p is Folder) && p != null)
            {
                p = p._parent;
            }
            return p;
        }

        /// <summary>
        /// Get the indirect children of the current node, that is, folder means piercing.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{TreeNode}"/> that enumerates its logical children.</returns>
        public IEnumerable<TreeNode> GetLogicalChildren()
        {
            foreach (TreeNode n in children)
            {
                //Ensure not in TryLink
                if (n._parent == this)
                {
                    if (n is Folder)
                    {
                        foreach (TreeNode t in n.GetLogicalChildren())
                        {
                            yield return t;
                        }
                    }
                    else
                    {
                        yield return n;
                    }
                }
            }
        }

        /// <summary>
        /// Get whether a node can be removed logically.
        /// </summary>
        /// <returns>A bool value, true for can.</returns>
        public bool CanLogicallyDelete()
        {
            if(this is Folder)
            {
                foreach(TreeNode t in GetLogicalChildren())
                {
                    if (!t.CanDelete) return false;
                }
                return true;
            }
            else
            {
                return CanDelete;
            }
        }

        /// <summary>
        /// Get whether a node can be banned logically.
        /// </summary>
        /// <returns>A bool value, true for can.</returns>
        public bool CanLogicallyBeBanned()
        {
            if (this is Folder)
            {
                foreach (TreeNode t in GetLogicalChildren())
                {
                    if (!t.CanBeBanned) return false;
                }
                return true;
            }
            else
            {
                return CanBeBanned;
            }
        }

        /// <summary>
        /// Clear isSelected in all children.
        /// </summary>
        public void ClearChildSelection()
        {
            this.isSelected = false;
            foreach(TreeNode t in children)
            {
                t.ClearChildSelection();
            }
        }

        /// <summary>
        /// Check the current attributes in this <see cref="TreeNode"/> with definition, rearrange it to definition.
        /// </summary>
        public void FixAttributesList()
        {
            bool ignore = GetType()
                .GetCustomAttributes(false)
                .Count((j) => j is Node.NodeAttributes.IgnoreAttributesParityCheckAttribute) > 0;
            if (!ignore)
            {
                usedAttributes = new HashSet<AttrItem>(attributes);
                var infos = GetType()
                    .GetProperties()
                    .Where(
                        (i) => i
                            .GetCustomAttributes(false)
                            .Count((j) => j is Node.NodeAttributes.NodeAttributeAttribute) > 0
                    );
                foreach(var i in infos)
                {
                    i.GetGetMethod().Invoke(this, null);
                }
                foreach(var i in usedAttributes)
                {
                    attributes.Remove(i);
                }
                usedAttributes = null;
            }
        }

        /// <summary>
        /// Get the nearest node in editing sequence.
        /// </summary>
        /// <returns></returns>
        public TreeNode GetNearestEdited()
        {
            TreeNode ne = _parent;
            if (ne != null)
            {
                int id = ne.children.IndexOf(this) - 1;
                if (id >= 0)
                {
                    ne = ne.children[id];
                }
                return ne;
            }
            else
            {
                return this;
            }
        }

        public static string Indent(int count)
        {
            return Lua.IndentationGenerator.Current.CreateIndentation(count);
        }
    }
}

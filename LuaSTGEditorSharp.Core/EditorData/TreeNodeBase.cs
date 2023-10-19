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
    public abstract class TreeNodeBase : INotifyPropertyChanged, ICloneable, IMessageThrowable
    {
        /// <summary>
        /// Store whether a <see cref="TreeNodeBase"/> is expanded in view. Use this only when you do not want to refresh the view.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool isExpanded;
        /// <summary>
        /// Store whether a <see cref="TreeNodeBase"/> is selected in view. Use this only when you do not want to refresh the view.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool isSelected = false;
        /// <summary>
        /// Store whether a <see cref="TreeNodeBase"/> is banned. Use this only when you do not want to refresh the view.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        protected bool isBanned = false;
        /// <summary>
        /// Store whether a <see cref="TreeNodeBase"/> is locked. Use this only when you do not want to refresh the view.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        protected bool isLocked = false;
        /// <summary>
        /// Store whether a <see cref="TreeNodeBase"/> is expanded in view. Using this will refresh the view.
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
        /// Store whether a <see cref="TreeNodeBase"/> is selected in view. Using this will refresh the view.
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
        /// Store whether a <see cref="TreeNodeBase"/> is locked in view. Using this will refresh the view.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool IsLocked_InvokeCommand
        {
            get => isLocked;
            set
            {
                parentWorkSpace.AddAndExecuteCommand(new SwitchLockCommand(this, value));
                RaiseProertyChanged(nameof(IsLocked_InvokeCommand));
            }
        }
        /// <summary>
        /// Store whether a <see cref="TreeNodeBase"/> is locked in view. Using this will refresh the view.
        /// </summary>
        [JsonProperty, DefaultValue(false)]
        [XmlAttribute("locked")]
        public bool IsLocked
        {
            get => isLocked;
            set
            {
                isLocked = value;
                RaiseProertyChanged("IsLocked");
            }
        }
        /// <summary>
        /// Store whether a <see cref="TreeNodeBase"/> is banned. 
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
                RaiseProertyChanged(nameof(IsBanned_InvokeCommand));
            }
        }
        /// <summary>
        /// Store whether a <see cref="TreeNodeBase"/> is banned. 
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
                parentWorkSpace?.OriginalMeta.RaisePropertyChanged(GetType().ToString());
            }
        }
        /// <summary>
        /// Use this to get the icon of the <see cref="TreeNodeBase"/>. 
        /// This property is always synced with <see cref="Node.NodeAttributes.NodeIconAttribute"/>.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public string Icon { get => PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].icon; }
        /// <summary>
        /// Use this to get whether the <see cref="TreeNodeBase"/> can be deleted. 
        /// This property is always synced with <see cref="Node.NodeAttributes.CannotDeleteAttribute"/> (reverted).
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool CanDelete { get => PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].canDelete; }
        /// <summary>
        /// Use this to get whether the <see cref="TreeNodeBase"/> cannot be banned. 
        /// This property is always synced with <see cref="Node.NodeAttributes.CannotBanAttribute"/> (reverted).
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool CanBeBanned { get => PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].canBeBanned; }
        /// <summary>
        /// Use this to get whether the <see cref="TreeNodeBase"/> ignores validation. 
        /// This property is always synced with <see cref="Node.NodeAttributes.IgnoreValidationAttribute"/>.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public bool IgnoreValidation { get => PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[GetType()].ignoreValidation; }

        [JsonIgnore, XmlIgnore]
        protected TreeNodeBase _parent = null;
        /// <summary>
        /// Store the <see cref="DocumentData"/> containing it. 
        /// Only change it when it is created from file or be moved to other documents.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public DocumentData parentWorkSpace = null;

        /// <summary>
        /// Store the child <see cref="TreeNodeBase"/> of this <see cref="TreeNodeBase"/>.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        private ObservableCollection<TreeNodeBase> children;
        /// <summary>
        /// Get the child <see cref="TreeNodeBase"/> of this <see cref="TreeNodeBase"/>.
        /// If is setted to null, create a new <see cref="ObservableCollection{T}"/>,
        /// and register the <see cref="ObservableCollection{T}.CollectionChanged"/> event.
        /// </summary>
        [JsonIgnore, XmlElement("Node")]
        public ObservableCollection<TreeNodeBase> Children
        {
            get => children;
            set
            {
                if (value == null)
                {
                    children = new ObservableCollection<TreeNodeBase>();
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
        /// Store the child <see cref="TreeNodeBase"/> of this <see cref="TreeNodeBase"/>.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public ObservableCollection<MessageBase> Messages { get; } = new ObservableCollection<MessageBase>();

        /// <summary>
        /// Store the parent <see cref="TreeNodeBase"/> of this node. Read only.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public TreeNodeBase Parent { get { return _parent; } }

        /// <summary>
        /// Store the next term of double linked list organized tree.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        private TreeNodeBase dblLinkedNext = null;

        /// <summary>
        /// Store the previous term of double linked list organized tree.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        private TreeNodeBase dblLinkedPrev = null;

        /// <summary>
        /// Use this property to get the <see cref="string"/> displayed on screen. 
        /// This property is synced with result of <see cref="ToString"/> method.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public string ScreenString { get { return this.ToString(); } }

        /// <summary>
        /// Identify whether ignore parity check for this node.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        protected virtual bool EnableParityCheck { get => true; }

        /// <summary>
        /// Identify whether a <see cref="TreeNodeBase"/> is belong to a proper document. 
        /// This property will determine whether event <see cref="OnCreate"/> and <see cref="OnRemove"/> are invoked.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        protected bool activated = false;

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
                foreach (TreeNodeBase t in Children)
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
                foreach (TreeNodeBase t in Children)
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
                foreach (TreeNodeBase t in Children)
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
                foreach (TreeNodeBase t in Children)
                {
                    t.RaiseVirtuallyRemove(e);
                }
            }
        }

        /// <summary>
        /// Event handler for <see cref="ObservableCollection{T}.CollectionChanged"/> in <see cref="Children"/>.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ChildrenChanged(object o, NotifyCollectionChangedEventArgs e)
        {
            TreeNodeBase t, tPrev = null;
            if (e.OldItems != null)
            {
                for (int index = 0; index < e.OldItems.Count; index++)
                {
                    t = (TreeNodeBase)e.OldItems[index];
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
                    t = (TreeNodeBase)e.NewItems[index];
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

        public abstract void DeepCopyFrom(TreeNodeBase source);

        /// <summary>
        /// The constructor used by Serializer. Classes who inherit it must override this.
        /// </summary>
        protected TreeNodeBase()
        {
            PropertyChanged += new PropertyChangedEventHandler(CheckMessage);
            OnVirtuallyCreate += new OnCreateNodeHandler(CreateMeta);
            OnCreate += new OnCreateNodeHandler(CreatedActivation);
            OnVirtuallyRemove += new OnRemoveNodeHandler(RemoveMeta);
            OnRemove += new OnRemoveNodeHandler(RemovedDeactivation);
            Children = null;
            isExpanded = true;
        }

        /// <summary>
        /// The constructor initialize the <see cref="parentWorkSpace"/> property.
        /// </summary>
        /// <param name="workSpaceData"> the given <see cref="DocumentData"/> </param>
        public TreeNodeBase(DocumentData workSpaceData) : this()
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
        protected IEnumerable<string> ToLua(int spacing, IEnumerable<TreeNodeBase> children)
        {
            bool childof = false;
            TreeNodeBase temp = GlobalCompileData.StageDebugger;
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

            if (GlobalCompileData.SCDebugger != null)
            {
                foreach (TreeNodeBase t in GetLogicalChildren())
                {
                    if (!t.isBanned)
                    {
                        foreach (var a in t.ToLua(spacing))
                        {
                            yield return a;
                        }
                        if (GlobalCompileData.SCDebugger == t)
                        {
                            foreach (string s in CompileBossForSCPrac())
                            {
                                yield return s;
                            }
                        }
                        t.AddCompileSettings();
                    }
                }
            }
            else if (GlobalCompileData.StageDebugger != null)
            {
                foreach (TreeNodeBase t in children)
                {
                    if (!t.isBanned)
                    {
                        if (childof)
                        {
                            if (!firstC && folderFound && !equalFound)
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
                        t.AddCompileSettings();
                    }
                }
            }
            else
            {
                foreach (TreeNodeBase t in children)
                {
                    if (!t.isBanned)
                    {
                        foreach (var a in t.ToLua(spacing))
                        {
                            yield return a;
                        }
                        t.AddCompileSettings();
                    }
                }
            }
        }

        protected abstract IEnumerable<string> CompileBossForSCPrac();

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
                a.AddRange(GetMismatchedAttributeMessage());
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

        protected abstract IEnumerable<MessageBase> GetMismatchedAttributeMessage();

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
        /// This method gets the <see cref="MetaInfo"> referred by this node.
        /// </summary>
        /// <returns></returns>
        public virtual MetaInfo GetReferredMeta()
        {
            return null;
        }

        /// <summary>
        /// This method gets the <see cref="TreeNodeBase"> referred by this node.
        /// </summary>
        /// <returns></returns>
        public TreeNodeBase GetReferredTreeNode()
        {
            return GetReferredMeta()?.target;
        }

        /// <summary>
        /// This item gets lua line information of <see cref="TreeNodeBase"/>. Must be synced with <see cref="ToLua(int)"/>.
        /// </summary>
        /// <returns>
        /// the <see cref="Tuple"/> of <see cref="int"/> and <see cref="TreeNodeBase"/> 
        /// which stores the information of line - TreeNode relations.
        /// </returns>
        public abstract IEnumerable<Tuple<int, TreeNodeBase>> GetLines();

        /// <summary>
        /// This method is used to invoke when getting lua line information of child <see cref="TreeNodeBase"/>s.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<Tuple<int, TreeNodeBase>> GetChildLines()
        {
            foreach (TreeNodeBase t in Children)
            {
                if (!t.isBanned)
                {
                    foreach (Tuple<int, TreeNodeBase> ti in t.GetLines())
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
        public bool ValidateChild(TreeNodeBase toV)
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
        private bool ValidateChild(TreeNodeBase toV, TreeNodeBase originalParent)
        {
            if (this is Folder) return GetLogicalParent()?.ValidateChild(toV, originalParent) ?? true;
            if (toV is Folder)
            {
                foreach (TreeNodeBase t in toV.GetLogicalChildren())
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
                e = e.Concat(new TreeNodeBase[] { toV }.AsEnumerable());
            }
            if (!MatchUniqueness(e)) return false;
            if (!toV.MatchParents(this)) return false;
            Stack<TreeNodeBase> stack = new Stack<TreeNodeBase>();
            stack.Push(toV);
            TreeNodeBase cur;
            while (stack.Count != 0)
            {
                cur = stack.Pop();
                if (!(cur.FindAncestorIn(cur, toV.Parent, this, null))) return false;
                if (PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo[cur.GetType()].classNode)
                {
                    if (!(MatchClassNode(cur, toV.Parent) || MatchClassNode(this, null))) return false;
                }
                foreach (TreeNodeBase t in cur.Children)
                {
                    stack.Push(t);
                }
            }
            return true;
        }

        /// <summary>
        /// This method is used to write <see cref="TreeNodeBase"/> information to file.
        /// </summary>
        /// <param name="fs"> The <see cref="FileStream"/> object of file </param>
        /// <param name="level"> the depth of current node </param>
        public void SerializeFile(StreamWriter fs, int level)
        {
            fs.WriteLine("" + level + "," + EditorSerializer.SerializeTreeNode(this));
            foreach (TreeNodeBase t in Children)
            {
                t.SerializeFile(fs, level + 1);
            }
        }

        #region tree

        /// <summary>
        /// Add a child to this node. also update meta and messages.
        /// </summary>
        /// <param name="n"><see cref="TreeNodeBase"/> to add.</param>
        public void AddChild(TreeNodeBase n)
        {
            Children.Add(n);
            //n._parent = this;
            //n.RaiseCreate(new OnCreateEventArgs() { parent = this });
        }

        /// <summary>
        /// Insert a child to this node. also update meta and messages.
        /// </summary>
        /// <param name="n"><see cref="TreeNodeBase"/> to insert.</param>
        /// <param name="index">ID to insert</param>
        public void InsertChild(TreeNodeBase n, int index)
        {
            Children.Insert(index, n);
            //n._parent = this;
            //n.RaiseCreate(new OnCreateEventArgs() { parent = this });
        }

        /// <summary>
        /// Remove a child from tree. also update meta and messages.
        /// </summary>
        /// <param name="t"><see cref="TreeNodeBase"/> to remove.</param>
        public void RemoveChild(TreeNodeBase t)
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
        /// The method that returns a deep copy of this <see cref="TreeNodeBase"/>. 
        /// Only specific types of <see cref="TreeNodeBase"/> may return distinct typed object.
        /// This behaviour in plugin is prohibited.
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        /// <summary>
        /// This method fixes parent in child nodes.
        /// </summary>
        public void FixChildrenParent()
        {
            foreach (TreeNodeBase t in Children)
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
            foreach (TreeNodeBase t in Children)
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
            foreach (TreeNodeBase t in Children)
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
        /// This method tests whether a group of nodes <see cref="TreeNodeBase"/> can be unique one if marked as uniqueness.
        /// </summary>
        /// <param name="logicalChildren">The group of nodes.</param>
        private static bool MatchUniqueness(IEnumerable<TreeNodeBase> logicalChildren)
        {
            if (logicalChildren == null) return false;
            var info = PluginHandler.Plugin.NodeTypeCache.NodeTypeInfo;
            HashSet<Type> foundTypes = new HashSet<Type>();
            foreach (TreeNodeBase t in logicalChildren)
            {
                if (info[t.GetType()].uniqueness && foundTypes.Contains(t.GetType())) return false;
                foundTypes.Add(t.GetType());
            }
            return true;
        }

        /// <summary>
        /// This method tests whether this <see cref="TreeNodeBase"/> have a direct parent of any type discribed in
        /// <see cref="TypeCacheData"/>.
        /// </summary>
        /// <param name="toMatch">The given <see cref="TreeNodeBase"/> as parent.</param>
        private bool MatchParents(TreeNodeBase toMatch)
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
        private static bool MatchClassNode(TreeNodeBase beg, TreeNodeBase end)
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
        private bool FindAncestorIn(TreeNodeBase Beg1, TreeNodeBase End1, TreeNodeBase Beg2, TreeNodeBase End2)
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
        public static TreeNodeBase TryLink(TreeNodeBase parent, TreeNodeBase child)
        {
            TreeNodeBase toInsP = parent._parent;
            parent._parent = child._parent;
            child._parent = parent;
            return toInsP;
        }

        /// <summary>
        /// This method revert what <see cref="TryLink(TreeNodeBase, TreeNodeBase)"/> did. Only for validation.
        /// </summary>
        /// <param name="parent">The assumed parent.</param>
        /// <param name="child">The assumed child.</param>
        /// <param name="originalpp">parent of original <paramref name="parent"/></param>
        public static void TryUnlink(TreeNodeBase parent, TreeNodeBase child, TreeNodeBase originalpp)
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
            foreach (TreeNodeBase t in children)
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
            foreach (TreeNodeBase t in children)
            {
                t.FoldTree();
            }
        }

        /// <summary>
        /// Get the enumerator fowardly iterate the rest of the tree.
        /// </summary>
        /// <returns>An enumerator of the tree.</returns>
        public IEnumerator<TreeNodeBase> GetFowardEnumerator()
        {
            TreeNodeBase t = this;
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
        public IEnumerator<TreeNodeBase> GetBackwardEnumerator()
        {
            TreeNodeBase t = this;
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
        /// <returns>A <see cref="TreeNodeBase"/>, null if not found.</returns>
        public TreeNodeBase GetLogicalParent()
        {
            TreeNodeBase p = _parent;
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
        public IEnumerable<TreeNodeBase> GetLogicalChildren()
        {
            foreach (TreeNodeBase n in children)
            {
                //Ensure not in TryLink
                if (n._parent == this)
                {
                    if (n is Folder)
                    {
                        foreach (TreeNodeBase t in n.GetLogicalChildren())
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
            if (this is Folder)
            {
                foreach (TreeNodeBase t in GetLogicalChildren())
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
                foreach (TreeNodeBase t in GetLogicalChildren())
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
            foreach (TreeNodeBase t in children)
            {
                t.ClearChildSelection();
            }
        }

        /// <summary>
        /// Get the nearest node in editing sequence.
        /// </summary>
        /// <returns></returns>
        public TreeNodeBase GetNearestEdited()
        {
            TreeNodeBase ne = _parent;
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

        public abstract string PreferredMacrolize(int id, string name);
        public abstract string PreferredNonMacrolize(int id, string name);

        public bool HasPreferredProperty(int id, string name)
        {
            return !string.IsNullOrEmpty(PreferredNonMacrolize(id, name));
        }

        /// <summary>
        /// Create Indentation.
        /// </summary>
        /// <param name="count">Indentation level. Must be nonegative.</param>
        /// <returns></returns>
        public static string Indent(int count)
        {
            return Lua.IndentationGenerator.Current.CreateIndentation(count);
        }
    }
}

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
    public abstract class FixedAttributeTreeNode : TreeNodeBase
    {
        /// <summary>
        /// Store attributes in <see cref="TreeNodeBase"/>.
        /// </summary>
        //[XmlArray("attributes")]
        [JsonIgnore, XmlIgnore]
        public ObservableCollection<AttrItem> attributes = new ObservableCollection<AttrItem>();

        /// <summary>
        /// Get the <see cref="Attribute"/> of this <see cref="TreeNodeBase"/>.
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

        [JsonIgnore, XmlIgnore]
        private Dictionary<string, int> attrName2ID = new Dictionary<string, int>();

        /// <summary>
        /// Use this property to get the attribute count of this node.
        /// </summary>
        public int AttributeCount { get => attributes.Count; }

        /// <summary>
        /// Indicates used attributes when checking.
        /// </summary>
        [JsonIgnore, XmlIgnore]
        private HashSet<AttrItem> usedAttributes = null;

        /// <summary>
        /// Event when node have an <see cref="DependencyAttrItem"/> changed.
        /// </summary>
        private event OnDependencyAttributeChangedHandler OnDependencyAttributeItemChanged;

        /// <summary>
        /// Raise <see cref="OnDependencyAttributeItemChanged"/> event.
        /// </summary>
        /// <param name="o">Object that trigger this event.</param>
        /// <param name="e">Arguments for this event.</param>
        public void RaiseDependencyPropertyChanged(DependencyAttrItem o, DependencyAttributeChangedEventArgs e)
        {
            OnDependencyAttributeItemChanged?.Invoke(o, e);
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
            if (e.OldItems != null || e.NewItems != null)
            {
                attrName2ID.Clear();
                for (int i = 0; i < attributes.Count; i++)
                {
                    if (attributes[i] != null) attrName2ID.Add(attributes[i].AttrCap, i);
                }
            }
        }

        /// <summary>
        /// Rearrange all attributes by a given source.
        /// </summary>
        /// <param name="source">Source <see cref="TreeNodeBase"/>.</param>
        public override void DeepCopyFrom(TreeNodeBase source)
        {
            DeepCopyFrom((FixedAttributeTreeNode)source);
        }

        public void DeepCopyFrom(FixedAttributeTreeNode source)
        {
            var attrs = from AttrItem a in source.attributes select (AttrItem)a.Clone();
            var childrens = from TreeNodeBase t in source.Children select (TreeNodeBase)t.Clone();
            Attributes = null;
            foreach (AttrItem ai in attrs)
            {
                attributes.Add(ai);
            }
            Children = null;
            foreach (TreeNodeBase treeNode in childrens)
            {
                Children.Add(treeNode);
            }
            _parent = source._parent;
            isExpanded = source.isExpanded;
            isBanned = source.isBanned;
        }

        protected FixedAttributeTreeNode() : base()
        {
            Attributes = null;
            OnDependencyAttributeItemChanged += new OnDependencyAttributeChangedHandler(ReflectAttr);
        }

        public FixedAttributeTreeNode(DocumentData workSpaceData) : this()
        {
            parentWorkSpace = workSpaceData;
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
        /// Get <see cref="MessageBase"/> that indicates attribute mismatch to standard node.
        /// </summary>
        /// <returns>
        /// A List of <see cref="MessageBase"/> that generated by this node.
        /// </returns>
        protected override IEnumerable<MessageBase> GetMismatchedAttributeMessage()
        {
            var a = new List<MessageBase>();
            if (!EnableParityCheck) return a;
            if (PluginHandler.Plugin.NodeTypeCache.StandardNode.ContainsKey(GetType()))
            {
                if (parentWorkSpace == null) return a;
                FixedAttributeTreeNode t = PluginHandler.Plugin.NodeTypeCache.StandardNode[GetType()].Clone() as FixedAttributeTreeNode;
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
                        ai = new DependencyAttrItem(name, (string)null, defaultEditWindow);
                    }
                    else
                    {
                        ai = new AttrItem(name, (string)null, defaultEditWindow);
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
        /// This method modify the current node when a <see cref="DependencyAttrItem"/> changed.
        /// It can only modify items after those who call it, and cannot modify <see cref="AttrItem"/> hierarchically.
        /// </summary>
        /// <param name="relatedAttrItem"> The <see cref="DependencyAttrItem"/> who call this. </param>
        /// <param name="originalvalue"> The original <see cref="DependencyAttrItem.AttrInput"/> value before it was changed. </param>
        public virtual void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs e) { }

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
        /// Get the path string of a given attribute.
        /// </summary>
        /// <param name="pathAttrID">The id of attribute</param>
        /// <returns><see cref="string"/> after applying archive space and lua escape sequences.</returns>
        protected string GetPath(int pathAttrID)
        {
            if (!parentWorkSpace.CompileProcess.Packer.SupportFolderInArchive) return Path.GetFileName(NonMacrolize(pathAttrID));
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
        /// This method executes all macros currently in the <see cref="CompileProcess"/> to a <see cref="string"/>.
        /// Can only be called in Compile Methods.
        /// </summary>
        /// <param name="s">The target <see cref="string"/></param>
        /// <returns>The <see cref="string"/> after applying macros.</returns>
        protected string ApplyMacro(string s)
        {
            foreach (Compile.DefineMarcoSettings m in parentWorkSpace.CompileProcess.marcoDefinition)
            {
                s = ExecuteMarco(m, s);
            }

            return s;
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
            s = ApplyMacro(s);
            return s;
        }

        protected string Macrolize(string name)
        {
            if (!attrName2ID.ContainsKey(name)) return "";
            return Macrolize(attributes[attrName2ID[name]]);
        }

        /// <summary>
        /// This method executes all macros currently in the <see cref="CompileProcess"/> to an<see cref="AttrItem"/>.
        /// Can only be called in Compile Methods.
        /// </summary>
        /// <param name="id">
        /// The id of target <see cref="AttrItem"/>.
        /// Case of <see cref="UnidentifiedNode"/> has taken into account.
        /// </param>
        /// <returns>The <see cref="string"/> after applying macros.</returns>
        protected string Macrolize(int id)
        {
            if (GetType() != typeof(UnidentifiedNode))
            {
                if (id < attributes.Count) return Macrolize(attributes[id]);
                return "";
            }
            else
            {
                if (id + 1 < attributes.Count) return Macrolize(attributes[id + 1]);
                return "";
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

        public string NonMacrolize(string name)
        {
            if (!attrName2ID.ContainsKey(name)) return "";
            return attributes[attrName2ID[name]].AttrInput;
        }

        /// <summary>
        /// This method gets the directly contents of inputs in a <see cref="AttrItem"/>.
        /// </summary>
        /// <param name="id">
        /// The id of target <see cref="AttrItem"/>.
        /// Case of <see cref="UnidentifiedNode"/> has taken into account.
        /// </param>
        /// <returns>The <see cref="string"/> of inputs.</return
        public string NonMacrolize(int id)
        {
            if (GetType() != typeof(UnidentifiedNode))
            {
                if (id < attributes.Count) return NonMacrolize(attributes[id]);
                return "";
            }
            else
            {
                if (id + 1 < attributes.Count) return NonMacrolize(attributes[id + 1]);
                return "";
            }
        }

        public override string PreferredMacrolize(int id, string name)
        {
            return Macrolize(id);
        }

        public override string PreferredNonMacrolize(int id, string name)
        {
            return NonMacrolize(id);
        }

        /// <summary>
        /// Check the current attributes in this <see cref="TreeNodeBase"/> with definition, rearrange it to definition.
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
                foreach (var i in infos)
                {
                    var attr = i.GetCustomAttributes(false).First(o => o is Node.NodeAttributes.NodeAttributeAttribute)
                        as Node.NodeAttributes.NodeAttributeAttribute;
                    string s = i.GetGetMethod().Invoke(this, null) as string;
                    i.SetValue(this, string.IsNullOrEmpty(s) ? attr.Default : s, null);
                }
                foreach (var i in usedAttributes)
                {
                    attributes.Remove(i);
                }
                usedAttributes = null;
            }
        }

        protected override IEnumerable<string> CompileBossForSCPrac()
        {
            string difficultyS = (NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1));
            string fullName = "\"" + NonMacrolize(0) + difficultyS + "\"";
            yield return "_boss_class_name=" + fullName;
            yield return " _editor_class[" + fullName + "].cards={boss.move.New(0,144,60,MOVE_NORMAL),_tmp_sc} ";
        }
    }
}

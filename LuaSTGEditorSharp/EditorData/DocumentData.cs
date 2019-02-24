using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.Plugin;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData
{
    /// <summary>
    /// Base class for all single documents.
    /// </summary>
    public abstract class DocumentData : INotifyPropertyChanged
    {
        /// <summary>
        /// Stores the <see cref="EditorData.CompileProcess"/> of the current document.
        /// </summary>
        public CompileProcess CompileProcess { get; set; }
        /// <summary>
        /// Store the absolute path to the target document.
        /// </summary>
        public string DocPath { get; set; } = "";
        /// <summary>
        /// Name for binding. Get method will add "*" if unsaved, 
        /// set will directly set <see cref="RawDocName"/> and notify UI to update.
        /// </summary>
        public string DocName
        {
            get => RawDocName + (IsUnsaved ? " *" : "");
            set
            {
                RawDocName = value;
                RaiseProertyChanged("DocName");
            }
        }

        /// <summary>
        /// Store whether the document is selected.
        /// </summary>
        private bool isSelected;
        /// <summary>
        /// Selected property for binding.
        /// </summary>
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                RaiseProertyChanged("IsSelected");
            }
        }

        public virtual string ExtensionInfo
        {
            get
            {
                return "LuaSTG Sharp File (*.lstges)|*.lstges";
            }
        }

        /// <summary>
        /// Store name of the target document.
        /// </summary>
        public string RawDocName { get; set; }
        /// <summary>
        /// Store unique document ID.
        /// </summary>
        public int DocHash { get; } = 0;

        /// <summary>
        /// Get the overall meta data that this file can find.
        /// </summary>
        public abstract AbstractMetaData Meta { get; }
        /// <summary>
        /// Get the meta data directly stored by this one.
        /// </summary>
        public abstract MetaDataEntity OriginalMeta { get; }

        /// <summary>
        /// Store the <see cref="Command"/> that have done.
        /// </summary>
        public Stack<Command> commandFlow = new Stack<Command>();
        /// <summary>
        /// Store the <see cref="Command"/> that is after undo if something is undone.
        /// </summary>
        public Stack<Command> undoFlow = new Stack<Command>();

        /// <summary>
        /// Store the reference of currently saved command.
        /// </summary>
        private Command savedCommand = null;

        /// <summary>
        /// Store the reference of parent <see cref="DocumentCollection"/>
        /// </summary>
        public DocumentCollection parent = null;

        /// <summary>
        /// Store whether the message is blocked.
        /// </summary>
        public bool SupressMessage { get; }

        /// <summary>
        /// Get the state that whether the current document is unsaved.
        /// </summary>
        public bool IsUnsaved
        {
            get
            {
                try
                {
                    return commandFlow.Peek() != savedCommand;
                }
                catch (InvalidOperationException)
                {
                    return savedCommand != null;
                }
            }
        }
        /// <summary>
        /// Store the <see cref="TreeNode"/> in the document.
        /// </summary>
        public WorkTree TreeNodes { get; set; } = new WorkTree();

        /// <summary>
        /// Initializes document by ID
        /// </summary>
        /// <param name="hash">ID of document. Unique.</param>
        public DocumentData(int hash, bool supressMessage = false)
        {
            DocHash = hash;
            SupressMessage = supressMessage;
        }

        /// <summary>
        /// Undo the command.
        /// </summary>
        public void Undo()
        {
            commandFlow.Peek().Undo();
            undoFlow.Push(commandFlow.Pop());
            RaiseProertyChanged("DocName");
            //OnEditing(parent);
        }

        /// <summary>
        /// Redo the command.
        /// </summary>
        public void Redo()
        {
            undoFlow.Peek().Execute();
            commandFlow.Push(undoFlow.Pop());
            RaiseProertyChanged("DocName");
            //OnEditing(parent);
        }

        /// <summary>
        /// Call on saving, get saved command.
        /// </summary>
        public void PushSavedCommand()
        {
            try
            {
                savedCommand = commandFlow.Peek();
                RaiseProertyChanged("DocName");
            }
            catch (InvalidOperationException) { }
        }

        /// <summary>
        /// If command is not null, add a <see cref="Command"/> to <see cref="commandFlow"/> and execute it.
        /// </summary>
        /// <param name="command">The <see cref="Command"/> to add, can be null.</param>
        /// <returns>Whether <paramref name="command"/> is null.</returns>
        public bool AddAndExecuteCommand(Command command)
        {
            if (command != null)
            {
                commandFlow.Push(command);
                commandFlow.Peek().Execute();
                undoFlow = new Stack<Command>();
                RaiseProertyChanged("DocName");
                //OnEditing(parent);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get a new document based on extension.
        /// </summary>
        /// <param name="extension">Extension of document.</param>
        /// <param name="maxHash">Max ID of <see cref="DocumentCollection"/>.</param>
        /// <param name="name">Name of document.</param>
        /// <param name="path">Path of document.</param>
        /// <returns></returns>
        public static DocumentData GetNewByExtension(string extension, int maxHash, string name, string path
            , bool supressMessage = false)
        {
            if (extension == ".lstges")
            {
                return new PlainDocumentData(maxHash, supressMessage)
                {
                    DocName = name,
                    DocPath = path,
                    IsSelected = true
                };
            }
            else
            {
                return new ProjectData(maxHash, supressMessage)
                {
                    DocName = name,
                    DocPath = path,
                    IsSelected = true
                };
            }
        }

        /// <summary>
        /// Save the document.
        /// </summary>
        /// <param name="saveAs">Whether force to save to another place.</param>
        /// <returns>Whether it is not cancelled.</returns>
        public bool Save(bool saveAs = false)
        {
            string path = "";
            if (string.IsNullOrEmpty(DocPath) || saveAs)
            {
                var saveFileDialog = new System.Windows.Forms.SaveFileDialog()
                {
                    Filter = ExtensionInfo,
                    FileName = saveAs ? "" : RawDocName
                };
                do
                {
                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return false;
                }
                while (string.IsNullOrEmpty(saveFileDialog.FileName));
                path = saveFileDialog.FileName;
                DocPath = path;
                DocName = path.Substring(path.LastIndexOf("\\") + 1);
            }
            else path = DocPath;
            PushSavedCommand();
            var s = new FileStream(path, FileMode.Create, FileAccess.Write);
            var sw = new StreamWriter(s, Encoding.UTF8);
            TreeNodes[0].SerializeFile(sw, 0);
            sw.Close();
            s.Close();
            return true;
        }


        /// <summary>
        /// Generate <see cref="TreeNode"/> from file. Asynchronous.
        /// </summary>
        /// <param name="fileName">The name of file.</param>
        /// <returns>The generated root.</returns>
        public static async Task<TreeNode> CreateNodeFromFileAsync(string fileName, DocumentData target)
        {
            TreeNode root = null;
            TreeNode prev = null;
            TreeNode tempN = null;
            int prevLevel = -1;
            int i;
            int levelgrad;
            char[] temp;
            string des;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(fileName, Encoding.UTF8);
                while (!sr.EndOfStream)
                {
                    temp = (await sr.ReadLineAsync()).ToCharArray();
                    i = 0;
                    while (temp[i] != ',')
                    {
                        i++;
                    }
                    des = new string(temp, i + 1, temp.Length - i - 1);
                    if (prevLevel != -1)
                    {
                        levelgrad = Convert.ToInt32(new string(temp, 0, i)) - prevLevel;
                        if (levelgrad <= 0)
                        {
                            for (int j = 0; j >= levelgrad; j--)
                            {
                                prev = prev.Parent;
                            }
                        }
                        tempN = (TreeNode)EditorSerializer.DeserializeTreeNode(des);
                        tempN.parentWorkSpace = target;
                        tempN.FixAttrParent();
                        prev.AddChild(tempN);
                        prev = tempN;
                        prevLevel += levelgrad;
                    }
                    else
                    {
                        root = (TreeNode)EditorSerializer.DeserializeTreeNode(des);
                        root.FixAttrParent();
                        root.parentWorkSpace = target;
                        prev = root;
                        prevLevel = 0;
                    }
                }
            }
            finally
            {
                if (sr != null) sr.Close();
            }
            return root;
        }

        /// <summary>
        /// Generate <see cref="TreeNode"/> from file.
        /// </summary>
        /// <param name="fileName">The name of file.</param>
        /// <returns>The generated root.</returns>
        public TreeNode CreateNodeFromFile(string fileName)
        {
            TreeNode root = null;
            TreeNode prev = null;
            TreeNode tempN = null;
            int prevLevel = -1;
            int i;
            int levelgrad;
            char[] temp;
            string des;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(fileName, Encoding.UTF8);
                while (!sr.EndOfStream)
                {
                    temp = (sr.ReadLine()).ToCharArray();
                    i = 0;
                    while (temp[i] != ',')
                    {
                        i++;
                    }
                    des = new string(temp, i + 1, temp.Length - i - 1);
                    if (prevLevel != -1)
                    {
                        levelgrad = Convert.ToInt32(new string(temp, 0, i)) - prevLevel;
                        if (levelgrad <= 0)
                        {
                            for (int j = 0; j >= levelgrad; j--)
                            {
                                prev = prev.Parent;
                            }
                        }
                        tempN = (TreeNode)EditorSerializer.DeserializeTreeNode(des);
                        tempN.parentWorkSpace = this;
                        tempN.FixAttrParent();
                        prev.AddChild(tempN);
                        prev = tempN;
                        prevLevel += levelgrad;
                    }
                    else
                    {
                        root = (TreeNode)EditorSerializer.DeserializeTreeNode(des);
                        root.FixAttrParent();
                        root.parentWorkSpace = this;
                        prev = root;
                        prevLevel = 0;
                    }
                }
            }
            finally
            {
                if (sr != null) sr.Close();
            }
            return root;
        }

        /// <summary>
        /// The method that call after initializing and creating nodes from file.
        /// </summary>
        public virtual void OnOpening()
        {

        }

        /// <summary>
        /// The method that call after meta changes.
        /// </summary>
        /// <param name="sender">A <see cref="MetaDataEntity"/> that calls it.</param>
        /// <param name="args">Argument of event. Always be <see cref="string"/> of type of metas.</param>
        public virtual void OnEditing(object sender, PropertyChangedEventArgs args)
        {

        }

        /// <summary>
        /// The method that call after saving on close.
        /// </summary>
        public virtual void OnClosing()
        {
            RevertUntilSaved();
            var toRemove = new List<MessageBase>();
            foreach(MessageBase mb in MessageContainer.Messages)
            {
                if (mb.SourceDoc == this) 
                {
                    toRemove.Add(mb);
                }
            }
            foreach (MessageBase mb in toRemove)
            {
                MessageContainer.Messages.Remove(mb);
            }
        }

        /// <summary>
        /// Get the <see cref="EditorData.CompileProcess"/> of this document.
        /// </summary>
        /// <param name="mainAppWithInfo">Main <see cref="App"/></param>
        internal abstract void GatherCompileInfo(App mainAppWithInfo);

        internal void SaveCode(string path)
        {
            FileStream s = null;
            StreamWriter sw = null;
            try
            {
                s = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(s, Encoding.UTF8);
                foreach (var a in TreeNodes[0].ToLua(0))
                {
                    sw.Write(a);
                }
            }
            finally
            {
                if (sw != null) sw.Close();
                if (s != null) s.Close();
            }
        }

        /// <summary>
        /// Save code for spell card debug.
        /// </summary>
        /// <param name="path">Target path.</param>
        internal void SaveSCDebugCode(string path)
        {
            FileStream s = null;
            StreamWriter sw = null;
            try
            {
                s = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(s, Encoding.UTF8);
                foreach (var a in TreeNodes[0].ToLua(0))
                {
                    sw.Write(a);
                }
                sw.Write("Include \'THlib\\\\UI\\\\scdebugger.lua\'");
            }
            finally
            {
                if (sw != null) sw.Close();
                if (s != null) s.Close();
            }
        }

        /// <summary>
        /// Save code for stage debug.
        /// </summary>
        /// <param name="path">Target path.</param>
        internal void SaveStageDebugCode(string path)
        {
            FileStream s = null;
            StreamWriter sw = null;
            try
            {
                s = new FileStream(path, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(s, Encoding.UTF8);
                foreach (var a in TreeNodes[0].ToLua(0))
                {
                    sw.Write(a);
                }
                TreeNode stage = GlobalCompileData.StageDebugger;
                while (!PluginHandler.Plugin.MatchStageNodeTypes(stage?.GetType()))
                {
                    stage = stage.Parent;
                }
                string parentStageGroupName = stage.Parent.attributes[0].AttrInput;
                sw.Write("_debug_stage_name=\'" + stage.attributes[0].AttrInput + "@" + parentStageGroupName + "\'");
                sw.Write("Include \'THlib\\\\UI\\\\debugger.lua\'");
            }
            finally
            {
                if (sw != null) sw.Close();
                if (s != null) s.Close();
            }
        }

        /// <summary>
        /// WARNING: POTENTIAL BUG ON META AND MESSAGES
        /// </summary>
        private void RevertUntilSaved()
        {
            if (savedCommand == null || commandFlow.Contains(savedCommand)) 
            {
                while (commandFlow.Count != 0 && commandFlow.Peek() != savedCommand)
                {
                    Undo();
                }
            }
            else
            {
                while (undoFlow.Count != 0 && undoFlow.Peek() != savedCommand)
                {
                    Redo();
                }
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
    }
}

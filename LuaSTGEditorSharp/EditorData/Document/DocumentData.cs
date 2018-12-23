using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Plugin;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Document
{
    public abstract class DocumentData : INotifyPropertyChanged
    {
        public CompileProcess CompileProcess { get; set; }
        public string DocPath { get; set; } = "";
        private string docName = "";
        public string DocName
        {
            get => docName + (IsUnsaved ? " *" : "");
            set
            {
                docName = value;
                RaiseProertyChanged("DocName");
            }
        }

        private bool isSelected;
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

        public string RawDocName { get => docName; }
        public int DocHash { get; } = 0;

        public abstract AbstractMetaData Meta { get; }
        public abstract MetaDataEntity OriginalMeta { get; }

        public Stack<Command> commandFlow = new Stack<Command>();
        public Stack<Command> undoFlow = new Stack<Command>();

        private Command savedCommand = null;

        public DocumentCollection parent = null;

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
        public WorkTree TreeNodes { get; set; } = new WorkTree();

        public DocumentData(int hash)
        {
            DocHash = hash;
        }

        public void Undo()
        {
            commandFlow.Peek().Undo();
            undoFlow.Push(commandFlow.Pop());
            RaiseProertyChanged("DocName");
            //OnEditing(parent);
        }

        public void Redo()
        {
            undoFlow.Peek().Execute();
            commandFlow.Push(undoFlow.Pop());
            RaiseProertyChanged("DocName");
            //OnEditing(parent);
        }

        public void PushSavedCommand()
        {
            try
            {
                savedCommand = commandFlow.Peek();
                RaiseProertyChanged("DocName");
            }
            catch (InvalidOperationException) { }
        }

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

        public static DocumentData GetNewByExtension(string extension, int maxHash, string name, string path)
        {
            if (extension == ".lstges")
            {
                return new PlainDocumentData(maxHash)
                {
                    DocName = name,
                    DocPath = path,
                    IsSelected = true
                };
            }
            else
            {
                return new ProjectData(maxHash)
                {
                    DocName = name,
                    DocPath = path,
                    IsSelected = true
                };
            }
        }

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

        public async Task<TreeNode> CreateNodeFromFileAsync(string fileName)
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

        public virtual void OnOpening()
        {

        }

        public virtual void OnEditing(object sender, PropertyChangedEventArgs args)
        {

        }

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

        public void CheckGrammar()
        {

        }

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

        public void HotFixEditWindow()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaiseProertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}

using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.Toolbox;
using LuaSTGEditorSharp.Windows;
using LuaSTGEditorSharp.Windows.Input;
using LuaSTGEditorSharp.EditorData.Exception;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.EditorData.Node.General;
using LuaSTGEditorSharp.EditorData.Node.Advanced;
using LuaSTGEditorSharp.EditorData.Commands;
using LuaSTGEditorSharp.EditorData.Commands.Factory;
using Newtonsoft.Json;

using Path = System.IO.Path;

namespace LuaSTGEditorSharp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        IInputWindowSelector InputWindowSelector { get; set; }
        DocumentCollection Documents { get; } = new DocumentCollection();

        private string debugString = "";

        public ObservableCollection<ToolboxTab> toolboxData;
        public ObservableCollection<ToolboxTab> ToolboxData { get => toolboxData; }

        private CommandTypeFac insertState = new AfterFac();
        
        public bool IsBeforeState { get => insertState.GetType() == typeof(BeforeFac); }
        public bool IsAfterState { get => insertState.GetType() == typeof(AfterFac); }
        public bool IsChildState { get => insertState.GetType() == typeof(ChildFac); }
        public bool IsParentState { get => insertState.GetType() == typeof(ParentFac); }

        public ObservableCollection<MessageBase> Messages { get => MessageContainer.Messages; }

        public string DebugString
        {
            get => debugString;
            set
            {
                debugString = value;
                RaiseProertyChanged("DebugString");
            }
        }

        public TreeNode clipBoard = null;

        public TreeNode selectedNode = null;

        public DocumentData ActivatedWorkSpaceData
        {
            get
            {
                return (DocumentData)docTabs?.SelectedItem;
            }
        }

        public TreeView workSpace;

        public MainWindow()
        {
            toolbox = PluginHandler.Plugin.GetToolbox(this);
            toolboxData = toolbox.ToolboxTabs;
            InitDict();
            InitializeComponent();
            InputWindowSelector = PluginHandler.Plugin.GetInputWindowSelector();
            comboDict.ItemsSource = toolbox.nodeNameList;
            this.docTabs.ItemsSource = Documents;
            EditorConsole.ItemsSource = Messages;
        }

        private bool CloseFile(DocumentData DocumentToRemove)
        {
            if (DocumentToRemove.IsUnsaved)
            {
                switch (MessageBox.Show("Do you want to save \"" + DocumentToRemove.RawDocName 
                    + "\"? ", "LuaSTG Editor Sharp", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                {
                    case MessageBoxResult.Yes:
                        if(SaveDoc(DocumentToRemove))
                        {
                            Documents.Remove(DocumentToRemove);
                            DocumentToRemove.OnClosing();
                            if (propData.ItemsSource is ObservableCollection<AttrItem> ai)
                            {
                                if (ai.Count > 0 && ai[0].Parent.parentWorkSpace == DocumentToRemove)
                                {
                                    propData.ItemsSource = null;
                                }
                            }
                        }
                        return true;
                    case MessageBoxResult.No:
                        break;
                    default:
                        return false;
                }
            }
            Documents.Remove(DocumentToRemove);
            DocumentToRemove.OnClosing();
            if (propData.ItemsSource is ObservableCollection<AttrItem> oai)
            {
                if (oai.Count > 0 && oai[0].Parent.parentWorkSpace == DocumentToRemove)
                {
                    propData.ItemsSource = null;
                }
            }
            return true;
        }

        private void RaiseInsertStateChanged()
        {
            RaiseProertyChanged("IsBeforeState");
            RaiseProertyChanged("IsAfterState");
            RaiseProertyChanged("IsParentState");
            RaiseProertyChanged("IsChildState");
        }

        #region execution

        private void NewDoc()
        {
            NewWindow nw = new NewWindow();
            if (nw.ShowDialog() == true)
            {
                string fullPathClone = Path.GetFullPath(nw.SelectedPath);
                CloneDocFromPath(nw.FileName, fullPathClone,
                    new ProjSettings(ActivatedWorkSpaceData, "", nw.Author
                    , nw.AllowPR.ToString().ToLower(), nw.AllowSCPR.ToString().ToLower()));
            }
        }

        private void OpenDoc()
        {
            var loadFileDialog = new System.Windows.Forms.OpenFileDialog()
            {
                Filter = "LuaSTG Sharp Editor File (*.lstges, *.lstgproj)|*.lstges;*.lstgproj",
                Multiselect = true
            };
            if (loadFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
            for (int i = 0; i < loadFileDialog.FileNames.Length; i++)
            {
                if (!IsOpened(loadFileDialog.FileNames[i])) OpenDocFromPath(loadFileDialog.SafeFileNames[i], loadFileDialog.FileNames[i]);
            }
        }

        private bool IsOpened(string path)
        {
            foreach (DocumentData doc in Documents)
            {
                if (doc.DocPath == path)
                {
                    return true;
                }
            }
            return false;
        }

        public async void OpenDocFromPath(string name, string path)
        {
            try
            {
                DocumentData newDoc = DocumentData.GetNewByExtension(Path.GetExtension(path), Documents.MaxHash, name, path);
                Documents.AddAndAllocHash(newDoc);
                TreeNode t = await newDoc.CreateNodeFromFileAsync(path);
                newDoc.TreeNodes.Add(t);
                newDoc.OnOpening();
                newDoc.OriginalMeta.PropertyChanged += newDoc.OnEditing;
            }
            catch (JsonException)
            {
                MessageBox.Show("Failed to open document. Please check whether the targeted file is in current version"
                    , "LuaSTG Editor Sharp", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void CloneDocFromPath(string name, string path, ProjSettings settings)
        {
            try
            {
                DocumentData newDoc = DocumentData.GetNewByExtension(Path.GetExtension(path), Documents.MaxHash, name, path);
                newDoc.DocPath = "";
                Documents.AddAndAllocHash(newDoc);
                TreeNode t = await newDoc.CreateNodeFromFileAsync(path);
                newDoc.TreeNodes.Add(t);
                foreach (TreeNode node in t.Children)
                {
                    if (node is ProjSettings)
                    {
                        for (int i = 0; i < node.attributes.Count; i++)
                        {
                            node.attributes[i].AttrInput = settings.attributes[i].AttrInput;
                        }
                    }
                    else if (node is EditorVersion)
                    {
                        node.attributes[0].AttrInput = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    }
                }
                newDoc.OnOpening();
                newDoc.OriginalMeta.PropertyChanged += newDoc.OnEditing;
            }
            catch (JsonException)
            {
                MessageBox.Show("Failed to open document. Please check whether the targeted file is in current version"
                    , "LuaSTG Editor Sharp", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool SaveActiveFile()
        {
            return SaveDoc(ActivatedWorkSpaceData);
        }

        private bool SaveDoc(DocumentData doc)
        {
            propData.CommitEdit();
            return doc.Save();
        }

        private void SaveActiveFileAs()
        {
            propData.CommitEdit();
            ActivatedWorkSpaceData.Save(true);
        }

        private void CutNode()
        {
            clipBoard = (TreeNode)selectedNode.Clone();
            ActivatedWorkSpaceData.AddAndExecuteCommand(new DeleteCommand(selectedNode));
        }

        private void CopyNode()
        {
            clipBoard = (TreeNode)selectedNode.Clone();
        }

        private void PasteNode()
        {
            TreeNode node = (TreeNode)clipBoard.Clone();
            node.FixParentDoc(ActivatedWorkSpaceData);
            ActivatedWorkSpaceData.AddAndExecuteCommand(
                insertState.ValidateAndNewInsert(
                    selectedNode, node));
        }

        private void Undo()
        {
            ActivatedWorkSpaceData.Undo();
        }

        private void Redo()
        {
            ActivatedWorkSpaceData.Redo();
        }

        private void DeleteNode()
        {
            ActivatedWorkSpaceData.AddAndExecuteCommand(new DeleteCommand(selectedNode));
        }

        private void GotoLine(int line)
        {
            int c = 0;
            ActivatedWorkSpaceData.GatherCompileInfo(App.Current as App);
            foreach(Tuple<int,TreeNode> tuple in ActivatedWorkSpaceData.TreeNodes[0].GetLines())
            {
                c += tuple.Item1;
                if (c >= line)
                {
                    Reveal(tuple.Item2);
                    break;
                }
            }
        }

        public void Reveal(TreeNode node)
        {
            if (node == null) return;
            TreeNode temp = node;
            node.parentWorkSpace.IsSelected = true;
            Stack<TreeNode> sta = new Stack<TreeNode>();
            while (temp != null)
            {
                sta.Push(temp);
                temp = temp.Parent;
            }
            
            while (sta.Count > 0)
            {
                sta.Pop().IsExpanded = true;
            }
            
            node.IsSelected = true;
        }

        private void ViewCode()
        {
            try
            {
                propData.CommitEdit();
                ActivatedWorkSpaceData.GatherCompileInfo(App.Current as App);
                var w = new CodePreviewWindow(string.Concat(selectedNode.ToLua(0)));
                w.ShowDialog();
            }
            catch { }
        }

        private void ExportCode()
        {
            try
            {
                propData.CommitEdit();
                var saveFileDialog = new System.Windows.Forms.SaveFileDialog()
                {
                    Filter = "Lua Code|*.lua"
                };
                saveFileDialog.ShowDialog();
                if (string.IsNullOrEmpty(saveFileDialog.FileName)) return;
                ActivatedWorkSpaceData.GatherCompileInfo(App.Current as App);
                ActivatedWorkSpaceData.SaveCode(saveFileDialog.FileName);
            }
            catch { }
        }

        private void ExportZip(bool run, TreeNode SCDebugger = null, TreeNode StageDebugger = null)
        {
            try
            {
                bool saveMeta=false;
                propData.CommitEdit();
                GlobalCompileData.SCDebugger = SCDebugger;
                GlobalCompileData.StageDebugger = StageDebugger;

                App currentApp = Application.Current as App;

                if (!run)
                {
                    saveMeta = currentApp.SaveResMeta;
                    currentApp.SaveResMeta = false;
                }

                if (currentApp.DebugSaveProj) if (!SaveActiveFile()) return;

                CompileProcess process = null;
                if(!(ActivatedWorkSpaceData is PlainDocumentData pdd && pdd.parentProj != null))
                {
                    ActivatedWorkSpaceData.GatherCompileInfo(currentApp);
                    ActivatedWorkSpaceData.CompileProcess.ExecuteProcess(SCDebugger != null, StageDebugger != null);
                    process = ActivatedWorkSpaceData.CompileProcess;
                }
                else
                {
                    pdd.parentProj.GatherCompileInfo(currentApp);
                    pdd.parentProj.CompileProcess.ExecuteProcess(SCDebugger != null, StageDebugger != null);
                    process = pdd.parentProj.CompileProcess;
                }

                if (run)
                {
                    TxtLine.Text = "";
                    RunLuaSTG(currentApp, process);
                }
                else
                {
                    currentApp.SaveResMeta = saveMeta;
                }
            }
            catch (EXEPathNotSetException)
            {
                
            }
            catch (InvalidRelativeResPathException)
            {
                
            }
            //catch { }
        }

        private void RunLuaSTG(App currentApp, CompileProcess process)
        {
            string LuaSTGparam = "\"" +
                                "start_game=true is_debug=true setting.nosplash=true setting.windowed="
                                + currentApp.DebugWindowed.ToString().ToLower() + " setting.resx=" + currentApp.DebugResolutionX +
                                " setting.resy=" + currentApp.DebugResolutionY + " cheat=" + currentApp.DebugCheat.ToString().ToLower() +
                                " updatelib=" + currentApp.DebugUpdateLib.ToString().ToLower() + " setting.mod=\'"
                                + process.projName + "\'\"";
            try
            {
                Process lstg = new Process
                {
                    StartInfo = new ProcessStartInfo(process.luaSTGExePath, LuaSTGparam)
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = process.luaSTGFolder,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true
                    }
                };
                lstg.Start();
                DebugString = "LuaSTG is Running.\n\n";
                tabOutput.IsSelected = true;
                /* 
                 * what it should be like:
                 * 
                lstg.OutputDataReceived += (s, e) => DebugString += e.Data;
                lstg.ErrorDataReceived += (s, e) => DebugString += e.Data;
                 *
                 * what it actually is:
                 */
                lstg.Exited += (s, e) => {
                    FileStream fs = null;
                    StreamReader sr = null;
                    try
                    {
                        fs = new FileStream(Path.GetFullPath(Path.Combine(
                            Path.GetDirectoryName(process.luaSTGExePath), "log.txt")), FileMode.Open);
                        sr = new StreamReader(fs);
                        DebugString += sr.ReadToEnd();
                    }
                    finally
                    {
                        if (fs != null) fs.Close();
                        if (sr != null) sr.Close();
                    }
                    DebugString += "\nExited with code " + lstg.ExitCode + ".";
                };
                lstg.EnableRaisingEvents = true;
                lstg.BeginOutputReadLine();
                lstg.BeginErrorReadLine();
                //lstg.WaitForExit();
            }
            catch (Win32Exception)
            {
                throw new EXEPathNotSetException();
            }
        }
        private void FoldRegion()
        {
            Region beg = selectedNode as Region;
            Region end = null;
            ObservableCollection<TreeNode> toFold = new ObservableCollection<TreeNode>();
            TreeNode p = beg.Parent;
            bool inSel = false;
            for (int i = 0; i < p.Children.Count; i++)
            {
                if (p.Children[i] != beg && p.Children[i] is Region)
                {
                    inSel = false;
                    end = p.Children[i] as Region;
                }
                if (inSel)
                {
                    toFold.Add(p.Children[i]);
                }
                if (p.Children[i] == beg)
                {
                    inSel = true;
                }
            }
            ActivatedWorkSpaceData.AddAndExecuteCommand(new FoldRegionCommand(toFold, beg, end));
        }

        private void UnfoldAsRegion()
        {
            ActivatedWorkSpaceData.AddAndExecuteCommand(new UnfoldAsRegionCommand(selectedNode));
        }

        private void CreateInvoke(TreeNode newNode)
        {
            propData.CommitEdit();
            AttrItem ai = newNode.GetCreateInvoke();
            if (ai != null)
            {
                InputWindow iw = InputWindowSelector.SelectInputWindow(ai, ai.EditWindow, ai.AttrInput, this);
                if (iw.ShowDialog() == true)
                {
                    ActivatedWorkSpaceData.AddAndExecuteCommand(new EditAttrCommand(ai, ai.AttrInput, iw.Result));
                    var a = propData.ItemsSource;
                    propData.ItemsSource = null;
                    propData.ItemsSource = a;
                }
            }
        }

        #endregion
        #region events

        private void ButtonUP_Click(object sender, RoutedEventArgs e)
        {
            insertState = new BeforeFac();
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            insertState = new AfterFac();
        }

        private void ButtonChild_Click(object sender, RoutedEventArgs e)
        {
            insertState = new ChildFac();
        }

        private void ButtonParent_Click(object sender, RoutedEventArgs e)
        {
            insertState = new ParentFac();
        }

        private void WorkSpaceSelectedChanged(object sender,RoutedEventArgs e)
        {
            workSpace = sender as TreeView;
            selectedNode = ((TreeNode)(workSpace.SelectedItem));
            if (selectedNode != null) this.propData.ItemsSource = selectedNode.attributes;
            //EditorConsole.Text = selectedNode.ToLua(0);
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            foreach(string s in InputWindowSelector.SelectComboBox(comboBox.Tag.ToString()))
            {
                ComboBoxItem item = new ComboBoxItem() { Content = s };
                comboBox.Items.Add(item);
            }
            comboBox.Focus();
        }

        protected void UpdateLog_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Update Log.txt"));
            if (File.Exists(path))
            {
                Process log = new Process
                {
                    StartInfo = new ProcessStartInfo(path)
                };
                log.Start();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            propData.CommitEdit();
            while (Documents.Count > 0)
            {
                if (!CloseFile(Documents[0]))
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void ButtonCloseFile_Click(object sender, RoutedEventArgs e)
        {
            propData.CommitEdit();
            Button btn = sender as Button;
            int btnHash = Convert.ToInt32(btn.Tag.ToString());
            var toRemove = new List<DocumentData>();
            foreach (DocumentData wsd in Documents)
            {
                if (wsd.DocHash == btnHash) toRemove.Add(wsd);
            }

            DocumentData DocumentToRemove = toRemove[0];
            CloseFile(DocumentToRemove);
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) is TreeViewItem treeViewItem)
            {
                treeViewItem.Focus();
                //e.Handled = true;
            }
        }

        private void TreeViewItem_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem
                && (sender as TreeViewItem).IsSelected)
            {
                propData.CommitEdit();
                AttrItem ai = selectedNode.GetRCInvoke();
                if (ai != null)
                {
                    InputWindow iw = InputWindowSelector.SelectInputWindow(ai, ai.EditWindow, ai.AttrInput, this);
                    if (iw.ShowDialog() == true)
                    {
                        ActivatedWorkSpaceData.AddAndExecuteCommand(new EditAttrCommand(ai, ai.AttrInput, iw.Result));
                        var a = propData.ItemsSource;
                        propData.ItemsSource = null;
                        propData.ItemsSource = a;
                    }
                }
                //e.Handled = true;
            }
        }
        #endregion
        #region commands

        private void NewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            NewDoc();
        }

        private void OpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenDoc();
        }

        private void SaveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveDoc(ActivatedWorkSpaceData);
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ActivatedWorkSpaceData != null)
            {
                e.CanExecute = ActivatedWorkSpaceData.IsUnsaved;
            }
            else e.CanExecute = false;
        }

        private void SaveAsCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveActiveFileAs();
        }

        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ActivatedWorkSpaceData != null;
        }

        private void CloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CloseFile(ActivatedWorkSpaceData);
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ActivatedWorkSpaceData != null;
        }

        private void UndoCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Undo();
            var a = propData.ItemsSource;
            propData.ItemsSource = null;
            propData.ItemsSource = a;
        }

        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ActivatedWorkSpaceData != null)
            {
                e.CanExecute = ActivatedWorkSpaceData.commandFlow.Count > 0;
            }
            else e.CanExecute = false;
        }

        private void RedoCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Redo();
            var a = propData.ItemsSource;
            propData.ItemsSource = null;
            propData.ItemsSource = a;
        }

        private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ActivatedWorkSpaceData != null)
            {
                e.CanExecute = ActivatedWorkSpaceData.undoFlow.Count > 0;
            }
            else e.CanExecute = false;
        }

        private void CutCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CutNode();
        }

        private void CutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (selectedNode != null)
            {
                e.CanExecute = selectedNode.CanDelete;
            }
            else e.CanExecute = false;
        }

        private void CopyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CopyNode();
        }

        private void CopyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = selectedNode != null;
        }

        private void PasteCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            PasteNode();
        }

        private void PasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = selectedNode != null && clipBoard != null;
        }

        private void DeleteCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteNode();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (selectedNode != null)
            {
                e.CanExecute = selectedNode.CanDelete;
            }
            else e.CanExecute = false;
        }

        private void FoldRegionCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            FoldRegion();
        }

        private void FoldRegionCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = selectedNode as Region != null && selectedNode.Parent.ValidateChildType(new Folder(ActivatedWorkSpaceData));
        }

        private void UnfoldAsRegionCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            UnfoldAsRegion();
        }

        private void UnfoldAsRegionCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (selectedNode == null) 
            {
                e.CanExecute = false;
                return;
            }
            if(!selectedNode.CanDelete)
            {
                e.CanExecute = false;
                return;
            }
            bool canE = true;
            foreach(TreeNode t in selectedNode.Children)
            {
                canE &= selectedNode.Parent.ValidateChildType(t);
            }
            e.CanExecute = canE;
        }

        private void GoToLineXCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            int.TryParse(e.Parameter.ToString(), out int line);
            GotoLine(line);
        }

        private void GoToLineXCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ActivatedWorkSpaceData != null && int.TryParse(e.Parameter.ToString(), out int i);
        }

        private void ViewCodeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewCode();
        }

        private void ViewCodeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = selectedNode != null;
        }

        private void ExportCodeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ExportCode();
        }

        private void ExportCodeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ActivatedWorkSpaceData != null;
        }

        private void ExportZipCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ExportZip(false);
        }

        private void ExportZipCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ActivatedWorkSpaceData != null;
        }

        private void RunProjectCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ExportZip(true);
        }

        private void RunProjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ActivatedWorkSpaceData != null;
        }

        private void SCDebugCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            TreeNode t = selectedNode;
            while (t != null)
            {
                if (PluginHandler.Plugin.MatchBossSCNodeTypes(t.GetType()))
                {
                    break;
                }
                t = t.Parent;
            }
            ExportZip(true, t, null);
        }

        private void SCDebugCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (selectedNode == null) return;
            TreeNode t = selectedNode;
            while (t != null) 
            {
                if(PluginHandler.Plugin.MatchBossSCNodeTypes(t.GetType()))
                {
                    e.CanExecute = true;
                }
                t = t.Parent;
            }
        }

        private void StageDebugCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ExportZip(true, null, selectedNode);
        }

        private void StageDebugCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (selectedNode == null) return;
            TreeNode t = selectedNode;
            while (t != null && t?.Parent is Folder)
            {
                t = t.Parent;
            }
            e.CanExecute = PluginHandler.Plugin.MatchStageNodeTypes(t?.Parent?.Parent?.GetType());
        }

        private void SwitchBeforeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            insertState = new BeforeFac();
            RaiseInsertStateChanged();
        }

        private void SwitchAfterCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            insertState = new AfterFac();
            RaiseInsertStateChanged();
        }

        private void SwitchChildCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            insertState = new ChildFac();
            RaiseInsertStateChanged();
        }

        private void SwitchParentCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            insertState = new ParentFac();
            RaiseInsertStateChanged();
        }
        
        private void ViewFileFolderCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Process folder = new Process
            {
                StartInfo = new ProcessStartInfo(Path.GetDirectoryName(ActivatedWorkSpaceData.DocPath))
                {
                    UseShellExecute = true,
                    CreateNoWindow = false
                }
            };
            folder.Start();
        }
        
        private void ViewFileFolderCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ActivatedWorkSpaceData != null;
        }
        
        private void ViewDefinitionCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            propData.CommitEdit();
            PluginHandler.Plugin.GetViewDefinitionWindow(ActivatedWorkSpaceData).ShowDialog();
        }
        
        private void ViewDefinitionCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ActivatedWorkSpaceData != null;
        }

        private void SettingsCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new SettingsWindow().ShowDialog();
        }

        private void AboutNodeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new AboutNodeWindow().ShowDialog();
        }

        private void AdjustPropCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            propData.CommitEdit();
            var button = sender as Button;
            //AttrItem ai = button.GetBindingExpression(TagProperty).ResolvedSource as AttrItem;
            AttrItem ai = button.Tag as AttrItem;
            //try
            {
                InputWindow iw = InputWindowSelector.SelectInputWindow(ai, e.Parameter.ToString(), ai.AttrInput, this);
                if (iw.ShowDialog() == true)
                {
                    ActivatedWorkSpaceData.AddAndExecuteCommand(new EditAttrCommand(ai, ai.AttrInput, iw.Result));
                    var a = propData.ItemsSource;
                    propData.ItemsSource = null;
                    propData.ItemsSource = a;
                }
            }
            //catch { }
        }

        public static string GetMD5HashFromFile(string fileName)
        {
            FileStream file = null;
            try
            {
                file = new FileStream(fileName, FileMode.Open);
                var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                var bytes = md5.ComputeHash(file);
                file.Close();
                var sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
            finally
            {
                if (file != null) file.Close();
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaiseProertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource is DataGridCell dgc)
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                if(!dgc.IsReadOnly)grd.BeginEdit(e);
            }
        }

        private void EditorConsoleRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBase mb = (sender as DataGridRow).Tag as MessageBase;
            mb.Invoke();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            propData.CommitEdit();
            AttrItem ai = selectedNode?.GetRCInvoke();
            if (ai != null)
            {
                InputWindow iw = InputWindowSelector.SelectInputWindow(ai, ai.EditWindow, ai.AttrInput, this);
                if (iw.ShowDialog() == true)
                {
                    ActivatedWorkSpaceData.AddAndExecuteCommand(new EditAttrCommand(ai, ai.AttrInput, iw.Result));
                    var a = propData.ItemsSource;
                    propData.ItemsSource = null;
                    propData.ItemsSource = a;
                }
            }
        }
    }
}

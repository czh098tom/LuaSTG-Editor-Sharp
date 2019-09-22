using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.Toolbox;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.EditorData.Node.General;
using LuaSTGEditorSharp.EditorData.Node.Advanced;
using LuaSTGEditorSharp.EditorData.Node.Project;

namespace LuaSTGEditorSharp
{
    partial class MainWindow : Window
    {
        public AbstractToolbox toolbox;

        public void InitDict()
        {
            foreach (KeyValuePair<string, BitmapImage> kvp in toolbox.GetToolBoxImageResources())
            {
                if(!Resources.Contains(kvp.Key)) Resources.Add(kvp.Key, kvp.Value);
            }
            foreach (KeyValuePair<string, BitmapImage> kvp in PluginHandler.Plugin.GetNodeImageResources())
            {
                if (!Resources.Contains(kvp.Key)) Resources.Add(kvp.Key, kvp.Value);
            }
        }

        public void Insert(TreeNode node, bool isInvoke = true)
        {
            try
            {
                bool move = (Application.Current as App).AutoMoveToNew;
                node.IsSelected = move;
                if (ActivatedWorkSpaceData.AddAndExecuteCommand(
                    insertState.ValidateAndNewInsert(
                        selectedNode, node)) && isInvoke)
                {
                    node.CheckMessage(null, new System.ComponentModel.PropertyChangedEventArgs(""));
                    CreateInvoke(node);
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(e.ToString());
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            toolbox.NFuncs[(sender as Button)?.Tag?.ToString()]();
        }

        private void ComboDict_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            comboDict.IsDropDownOpen = true;
        }

        private void ComboDict_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string s = (sender as ComboBox)?.Text;
                if (!string.IsNullOrEmpty(s))
                {
                    if (toolbox.NFuncs.ContainsKey(s))
                    {
                        toolbox.NFuncs[s]();
                        comboDict.Text = "";
                    }
                }
            }
            CollectionView itemsViewOriginal = (CollectionView)CollectionViewSource.GetDefaultView(comboDict.ItemsSource);

            itemsViewOriginal.Filter = ((o) =>
            {
                if (String.IsNullOrEmpty(comboDict.Text)) return true;
                else
                {
                    if ((((SearchModel)o).Name).Contains(comboDict.Text)) return true;
                    else return false;
                }
            });
            //comboDict.IsDropDownOpen = true;
            comboDict.SelectedItem = null;
            itemsViewOriginal.Refresh();
            comboDict.IsDropDownOpen = true;
            /*
            else if (e.Key != Key.Down && e.Key != Key.Up && e.Key != Key.Left && e.Key != Key.Right) 
            {
                List<SearchModel> mylist = new List<SearchModel>();
                mylist = toolbox.nodeNameList.FindAll(delegate (SearchModel s) { return s.Name.Contains(comboDict.Text.Trim().ToLower()); });
                comboDict.ItemsSource = mylist;
                comboDict.SelectedItem = null;
                comboDict.IsDropDownOpen = true;
            }
            */
        }

        public void GetPresets()
        {
            PresetsGetList.Clear();
            string s = Path.GetFullPath(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                , "LuaSTG Editor Sharp Presets"));
            if (!Directory.Exists(s)) Directory.CreateDirectory(s);
            GetDirInfo(new DirectoryInfo(s), PresetsGetList);
        }
        
        private void GetDirInfo(DirectoryInfo path, ObservableCollection<FileDirectoryModel> input)
        {
            FileDirectoryModel temp;
            foreach(DirectoryInfo di in path.EnumerateDirectories())
            {
                temp = new FileDirectoryModel() { Name = di.Name, FullPath = di.FullName };
                GetDirInfo(di, temp.Children);
                input.Add(temp);
            }
            foreach(FileInfo fi in path.EnumerateFiles())
            {
                temp = new FileDirectoryModel() { Name = Path.GetFileNameWithoutExtension(fi.Name), FullPath = fi.FullName };
                input.Add(temp);
            }
            temp = new FileDirectoryModel() { Name = "New folder...", FullPath = path.FullName };
            input.Add(temp);
        }
    }
}

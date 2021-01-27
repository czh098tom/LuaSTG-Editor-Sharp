using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData
{
    internal class DummyDoc
    {
        private readonly ObservableCollection<TreeNode> treeNodes = new ObservableCollection<TreeNode>();
        public TreeNode Root
        {
            get => treeNodes.Count > 0 ? (treeNodes[0].Children.Count > 0 ? treeNodes[0].Children[0] : null) : null;
        }

        private async void Create(string path)
        {
            try
            {
                TreeNode t = await DocumentData.CreateNodeFromFileAsync(path, null);
                treeNodes.Add(t);
            }
            catch (JsonException e)
            {
                MessageBox.Show("Failed to open document. Please check whether the targeted file is in current version.\n"
                    + e.ToString()
                    , "LuaSTG Editor Sharp", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Toolbox
{
    public class FileDirectoryModel
    {
        public static readonly string newFolderIndicator = "\n..";

        public string Name { get; set; }
        public string FullPath { get; set; }
        public ObservableCollection<FileDirectoryModel> Children { get; set; } = new ObservableCollection<FileDirectoryModel>();
    }
}

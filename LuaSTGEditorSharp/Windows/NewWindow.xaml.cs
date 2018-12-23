using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LuaSTGEditorSharp.Windows
{
    /// <summary>
    /// NewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewWindow : Window, INotifyPropertyChanged
    {
        private string fileName = "Untitled";
        private string author = (Application.Current as App).AuthorName;
        private bool allowpr = true;
        private bool allowscpr = true;

        public string FileName
        {
            get => fileName;
            set
            {
                fileName = value;
                RaiseProertyChanged("FileName");
            }
        }

        public string Author
        {
            get => author;
            set
            {
                author = value;
                RaiseProertyChanged("Author");
            }
        }

        public bool AllowPR
        {
            get => allowpr;
            set
            {
                allowpr = value;
                RaiseProertyChanged("AllowPR");
            }
        }

        public bool AllowSCPR
        {
            get => allowscpr;
            set
            {
                allowscpr = value;
                RaiseProertyChanged("AllowSCPR");
            }
        }

        public string SelectedPath { get; set; }

        class DefS
        {
            public string Text { get; set; }
            public string FullPath { get; set; }
            public string Icon { get; set; }
        }

        List<DefS> templates;

        public NewWindow()
        {
            string s = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\"));
            DirectoryInfo dir = new DirectoryInfo(s);
            List<FileInfo> fis = new List<FileInfo>(dir.GetFiles("*.lstges"));
            fis.AddRange(dir.GetFiles("*.lstgproj"));
            templates = new List<DefS>(
                from FileInfo fi
                in fis
                select new DefS {
                    Text = Path.GetFileNameWithoutExtension(fi.Name),
                    FullPath =fi.FullName,
                    Icon = "..\\images\\Icon.png" });
            InitializeComponent();
            ListTemplates.ItemsSource = templates;
            try
            {
                ListTemplates.SelectedIndex = 0;
            }
            catch { }
            TextName.Focus();
            TextName.SelectAll();
        }

        private void ListTemplates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DefS sel = ListTemplates.SelectedItem as DefS;
            try
            {
                string fullPathDesc = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                    , "Templates", sel.Text + ".txt"));
                FileStream f = new FileStream(fullPathDesc, FileMode.Open);
                StreamReader sr = new StreamReader(f);
                TextDescription.Text = sr.ReadLine();
                f.Close();
            }
            catch { }
        }

        private void ListTemplates_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            SelectedPath = (ListTemplates.SelectedItem as DefS)?.FullPath;
            this.Close();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            SelectedPath = (ListTemplates.SelectedItem as DefS)?.FullPath;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaiseProertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}

using LuaSTGEditorSharp.EditorData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;
using TextBox = System.Windows.Controls.TextBox;

namespace LuaSTGEditorSharp.Windows.Input
{
    /// <summary>
    /// MultiplePathInput.xaml 的交互逻辑
    /// </summary>
    public partial class MultiplePathInput : InputWindow
    {
        string CurrentFilePath { get; }
        string Extension { get; }

        class FileItem : INotifyPropertyChanged
        {
            private readonly MultiplePathInput parent;

            private string value;

            public FileItem(MultiplePathInput p) { parent = p; }

            public string Value
            {
                get => value;
                set
                {
                    this.value = value;
                    RaiseProertyChanged("Value");
                }
            }

            public string Value_Invoke
            {
                get => value;
                set
                {
                    this.value = value;
                    RaiseProertyChanged("Value");
                    parent.CombineParams();
                    parent.RaisePropertyChanged("ResultTXT");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void RaiseProertyChanged(string propName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }

        private ObservableCollection<FileItem> Items { get; set; } = new ObservableCollection<FileItem>();

        public MultiplePathInput(string s, string ext, AttrItem owner)
        {
            InitializeComponent();
            Result = s;
            codeText.Text = Result;
            Extension = ext;
            try
            {
                CurrentFilePath = Path.GetDirectoryName(owner?.Parent?.parentWorkSpace?.DocPath);
            }
            catch (ArgumentException)
            {
                CurrentFilePath = "";
            }
            sumBox.ItemsSource = Items;
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            var chooseFile = new OpenFileDialog()
            {
                InitialDirectory = (Application.Current as IAppSettings).SLDir,
                Multiselect = true,
                Filter = Extension
            };
            if (chooseFile.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                List<string> files = new List<string>();
                foreach (var file in ResultTXT.Split("|", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Distinct())
                {
                    files.Add(file);
                }
                foreach (var filename in chooseFile.FileNames)
                {
                    if (!string.IsNullOrEmpty(CurrentFilePath) && filename.Contains(CurrentFilePath))
                    {
                        files.Add(RelativePathConverter.GetRelativePath(CurrentFilePath, filename));
                    }
                    else
                    {
                        files.Add(filename);
                    }
                }
                ResultTXT = string.Join("|", files.Distinct());
                try
                {
                    (Application.Current as IAppSettings).SLDir = Path.GetDirectoryName(chooseFile.FileNames.First());
                }
                catch { }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            ResultTXT = "";
        }

        public string ResultTXT
        {
            get => Result;
            set
            {
                Result = value;
                RaisePropertyChanged("ResultTXT");
                DecomposeParams();
            }
        }

        public void DecomposeParams()
        {
            Items.Clear();
            foreach (var file in ResultTXT.Split("|", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Distinct())
            {
                Items.Add(new FileItem(this)
                {
                    Value = file
                });
            }
        }

        public void CombineParams()
        {
            ResultTXT = string.Join("|", Items.Select(x => x.Value));
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void InputWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(ButtonAddFile);
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox t = sender as TextBox;
            t.Focus();
            t.SelectAll();
        }

        private void Text_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                this.Close();
            }
        }

        private void sumBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                FileItem[] selected = new FileItem[sumBox.SelectedItems.Count];
                for (var i = 0; i < sumBox.SelectedItems.Count; i++)
                {
                    selected[i] = (FileItem)sumBox.SelectedItems[i];
                }
                foreach (FileItem file in selected)
                {
                    Items.Remove(file);
                }
                CombineParams();
            }
        }
    }
}

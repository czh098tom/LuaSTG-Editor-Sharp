using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using LuaSTGEditorSharp.EditorData;

namespace LuaSTGEditorSharp.Windows.Input
{
    /// <summary>
    /// CodeInput.xaml 的交互逻辑
    /// </summary>
    public partial class PathInput : InputWindow
    {
        string CurrentFilePath { get; }
        string Extension { get; }
        public PathInput(string s, string ext, MainWindow owner)
        {
            InitializeComponent();
            Result = s;
            codeText.Text = Result;
            Extension = ext;
            try
            {
                CurrentFilePath = System.IO.Path.GetDirectoryName(owner.ActivatedWorkSpaceData.DocPath);
            }
            catch (ArgumentException)
            {
                CurrentFilePath = "";
            }
            ButtonBrowse_Click(null, null);
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            var chooseFile = new OpenFileDialog()
            {
                InitialDirectory = (App.Current as App).SLDir,
                Filter = Extension
            };
            if (chooseFile.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                if(!string.IsNullOrEmpty(CurrentFilePath) && chooseFile.FileName.Contains(CurrentFilePath))
                {
                    Result = RelativePathConverter.GetRelativePath(CurrentFilePath, chooseFile.FileName);
                }
                else
                {
                    Result = chooseFile.FileName;
                }
                try
                {
                    (App.Current as App).SLDir = Path.GetDirectoryName(chooseFile.FileName);
                }
                catch { }
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void InputWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(codeText);
        }

        private void Text_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                this.Close();
            }
        }
    }
}

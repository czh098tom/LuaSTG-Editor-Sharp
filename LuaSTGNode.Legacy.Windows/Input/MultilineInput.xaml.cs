using System;
using System.Collections.Generic;
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
using System.IO;
using LuaSTGEditorSharp.EditorData;

namespace LuaSTGEditorSharp.Windows.Input
{
    /// <summary>
    /// MultilineInput.xaml 的交互逻辑
    /// </summary>
    public partial class MultilineInput : InputWindow
    {
        public MultilineInput(string s)
        {
            InitializeComponent();
            Result = s;
            codeText.Text = Result;
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
    }
}

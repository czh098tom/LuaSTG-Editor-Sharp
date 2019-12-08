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
using System.Windows.Shapes;

namespace LuaSTGEditorSharp.Windows.Input
{
    /// <summary>
    /// BoolInput.xaml 的交互逻辑
    /// </summary>
    public partial class Selector : InputWindow
    {
        public Selector(string s, string[] items, string title)
        {
            InitializeComponent();
            Result = s;
            codeText.Text = Result;
            this.Title = title;
            foreach (string str in items)
            {
                ComboBoxItem item = new ComboBoxItem() { Content = str };
                codeText.Items.Add(item);
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            //Result = codeText.Text;
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

        private void Text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                this.Close();
            }
        }
    }
}

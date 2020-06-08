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
    /// BulletInput.xaml 的交互逻辑
    /// </summary>
    public partial class EnemyInput : InputWindow
    {
        public EnemyInput(string s)
        {
            InitializeComponent();
            Result = s;
        }

        private void Style_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button a) Result = a.Tag?.ToString();
            DialogResult = true;
            this.Close();
        }

        private void InputWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void InputWindow_Closed(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;

using Path = System.IO.Path;

namespace LuaSTGEditorSharp.Windows.Input
{
    /// <summary>
    /// BGMInput.xaml 的交互逻辑
    /// </summary>
    public partial class BGMInput : InputWindow
    {
        ObservableCollection<MetaModel> AllBGMInfo { get; set; }
        ObservableCollection<MetaModel> FilteredBGMInfo { get; set; }

        public BGMInput(string s, AttrItem item)
        {
            AllBGMInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.BGMLoad].GetAllSimpleWithDifficulty("");
            FilteredBGMInfo = new ObservableCollection<MetaModel>(AllBGMInfo);

            InitializeComponent();

            BoxBGMData.ItemsSource = FilteredBGMInfo;

            Result = s;
            codeText.Text = Result;
        }

        private void Filter_TextChanged(object sender, RoutedEventArgs e)
        {
            FilteredBGMInfo.Clear();
            foreach (MetaModel mm in AllBGMInfo.Where(mm => MatchFilter(mm.FullName, filter.Text)))
            {
                FilteredBGMInfo.Add(mm);
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

        private void BoxBGMData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MetaModel m = (BoxBGMData.SelectedItem as MetaModel);
            if (m == null) return;
            if (!string.IsNullOrEmpty(m?.Result)) Result = m?.Result;
            try
            {
                mediaPlayer.Source = new Uri(m?.ExInfo1, UriKind.RelativeOrAbsolute);
                //MessageBox.Show(m?.ExInfo1);
                //mediaPlayer.Play();
            }
            catch {  }
            codeText.Focus();
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void Text_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                DialogResult = true;
                this.Close();
            }
        }
    }
}

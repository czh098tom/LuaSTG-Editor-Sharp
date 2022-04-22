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

namespace LuaSTGEditorSharp.Windows.Input
{
    /// <summary>
    /// SingleLineInput.xaml 的交互逻辑
    /// </summary>
    public partial class BossBGDefInput : InputWindow
    {
        ObservableCollection<MetaModel> AllBossBGInfo { get; set; }
        ObservableCollection<MetaModel> FilteredBossBGInfo { get; set; }

        public BossBGDefInput(string s, AttrItem item)
        {
            AllBossBGInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.BossBG].GetAllSimpleWithDifficulty();
            FilteredBossBGInfo = new ObservableCollection<MetaModel>(AllBossBGInfo);

            InitializeComponent();

            BoxBossBGDefinitionData.ItemsSource = FilteredBossBGInfo;

            Result = s;
            codeText.Text = Result;
        }

        private void Filter_TextChanged(object sender, RoutedEventArgs e)
        {
            FilteredBossBGInfo.Clear();
            foreach (MetaModel mm in AllBossBGInfo.Where(mm => MatchFilter(mm.FullName, filter.Text)))
            {
                FilteredBossBGInfo.Add(mm);
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

        private void BoxBossBGDefinitionData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MetaModel m = (BoxBossBGDefinitionData.SelectedItem as MetaModel);
            if (m == null) return;
            if (!string.IsNullOrEmpty(m?.Result)) Result = m?.Result;
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

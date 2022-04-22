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
    public partial class BossDefInput : InputWindow
    {
        private readonly string difficulty;

        ObservableCollection<MetaModel> AllBossInfo { get; set; }
        ObservableCollection<MetaModel> FilteredBossInfo { get; set; }

        public BossDefInput(string s, IMainWindow owner, AttrItem item)
        {
            difficulty = item.Parent.GetDifficulty();

            AllBossInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.Boss].GetAllSimpleWithDifficulty(difficulty);
            FilteredBossInfo = new ObservableCollection<MetaModel>(AllBossInfo);

            InitializeComponent();

            BoxBossDefinitionData.ItemsSource = FilteredBossInfo;

            Result = s;
            codeText.Text = Result;
        }

        private void Filter_TextChanged(object sender, RoutedEventArgs e)
        {
            FilteredBossInfo.Clear();
            foreach (MetaModel mm in AllBossInfo.Where(mm => MatchFilter(mm.FullName, filter.Text)))
            {
                FilteredBossInfo.Add(mm);
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

        private void BoxBossDefinitionData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MetaModel m = (BoxBossDefinitionData.SelectedItem as MetaModel);
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

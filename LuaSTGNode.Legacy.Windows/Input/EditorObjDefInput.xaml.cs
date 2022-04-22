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
    /// BulletDefInput.xaml 的交互逻辑
    /// </summary>
    public partial class EditorObjDefInput : InputWindow
    {
        private readonly string difficulty;

        ObservableCollection<MetaModel> AllEditorObjInfo { get; set; }
        ObservableCollection<MetaModel> FilteredEditorObjInfo { get; set; }

        public EditorObjDefInput(string s, MetaType type, AttrItem item)
        {
            difficulty = item.Parent.GetDifficulty();

            AllEditorObjInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)type].GetAllSimpleWithDifficulty(difficulty);
            FilteredEditorObjInfo = new ObservableCollection<MetaModel>(AllEditorObjInfo);

            InitializeComponent();

            Title = "Choose " + type.ToString();

            BoxEditorObjDefinitionData.ItemsSource = FilteredEditorObjInfo;

            Result = s;
            codeText.Text = Result;
        }

        private void Filter_TextChanged(object sender, RoutedEventArgs e)
        {
            FilteredEditorObjInfo.Clear();
            foreach (MetaModel mm in AllEditorObjInfo.Where(mm => MatchFilter(mm.FullName, filter.Text)))
            {
                FilteredEditorObjInfo.Add(mm);
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

        private void BoxEditorObjDefinitionData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MetaModel m = (BoxEditorObjDefinitionData.SelectedItem as MetaModel);
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

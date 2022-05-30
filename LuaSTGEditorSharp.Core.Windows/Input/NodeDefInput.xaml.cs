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
    public partial class NodeDefInput : InputWindow
    {
        private readonly string difficulty;

        ObservableCollection<MetaModel> AllNodeDefModel { get; set; }
        ObservableCollection<MetaModel> FilteredNodeDefModel { get; set; }

        public NodeDefInput(string s, AttrItem item)
        {
            difficulty = item.Parent.GetDifficulty();

            AllNodeDefModel = item.Parent.parentWorkSpace.Meta.UserDefinedData.GetAllSimpleWithDifficulty(difficulty);
            FilteredNodeDefModel = new ObservableCollection<MetaModel>(AllNodeDefModel);

            InitializeComponent();

            Title = "Choose node";

            BoxNodeDefinitionData.ItemsSource = FilteredNodeDefModel;

            Result = s;
            codeText.Text = Result;
        }

        private void Filter_TextChanged(object sender, RoutedEventArgs e)
        {
            FilteredNodeDefModel.Clear();
            foreach (MetaModel mm in AllNodeDefModel.Where(mm => MatchFilter(mm.FullName, filter.Text)))
            {
                FilteredNodeDefModel.Add(mm);
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
            MetaModel m = (BoxNodeDefinitionData.SelectedItem as MetaModel);
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
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                this.Close();
            }
        }
    }
}

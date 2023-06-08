using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LuaSTGEditorSharp.Windows.Input
{
    /// <summary>
    /// ImageInput.xaml 的交互逻辑
    /// </summary>
    public partial class SEInput : InputWindow
    {
        ObservableCollection<MetaModel> allSEInfo;
        ObservableCollection<MetaModel> allSEInfoSys;
        ObservableCollection<MetaModel> filteredSEInfo;
        ObservableCollection<MetaModel> filteredSEInfoSys;

        public ObservableCollection<MetaModel> FilteredSEInfo { get => filteredSEInfo; }
        public ObservableCollection<MetaModel> FilteredSEInfoSys { get => filteredSEInfoSys; }

        public override string Result
        {
            get => result;
            set
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }

        public SEInput(string s, AttrItem item)
        {
            allSEInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.SELoad].GetAllSimpleWithDifficulty();
            AddInternalMetas();
            filteredSEInfo = new ObservableCollection<MetaModel>(allSEInfo);
            filteredSEInfoSys = new ObservableCollection<MetaModel>(allSEInfoSys);

            InitializeComponent();

            Result = s;
            codeText.Text = Result;
        }

        private void Filter_TextChanged(object sender, RoutedEventArgs e)
        {
            filteredSEInfo.Clear();
            foreach (MetaModel mm in allSEInfo.Where(mm => MatchFilter(mm.FullName, filter.Text)))
            {
                filteredSEInfo.Add(mm);
            }
            filteredSEInfoSys.Clear();
            foreach (MetaModel mm in allSEInfoSys.Where(mm => MatchFilter(mm.FullName, filter.Text)))
            {
                filteredSEInfoSys.Add(mm);
            }
        }

        private void AddInternalMetas()
        {
            allSEInfoSys = new ObservableCollection<MetaModel>(NodesConfig.SysSE);
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

        private void BoxSEData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MetaModel m = (BoxSEData.SelectedItem as MetaModel);
            if (m == null) return;
            if (!string.IsNullOrEmpty(m?.Result)) Result = m?.Result;
            try
            {
                var uri = new Uri(m?.ExInfo1, UriKind.RelativeOrAbsolute);
                if (!string.IsNullOrEmpty(m?.ExInfo1))
                {
                    string type = "unknown";
                    if (allSEInfoSys.Any(x => x.ExInfo1 == m.ExInfo1))
                    {
                        type = Path.GetExtension(m.ExInfo1).Substring(1);
                    }
                    else
                    {
                        var fileStream = File.OpenRead(uri.AbsolutePath);
                        var binaryReader = new BinaryReader(fileStream, Encoding.Default);
                        byte[] buffer = binaryReader.ReadBytes(4);
                        binaryReader.Close();
                        fileStream.Close();
                        string header = string.Join("", buffer.Select(element => element.ToString("X2")));
                        switch (header)
                        {
                            case "49443303":
                                type = "mp3";
                                break;
                            case "52494646":
                                type = "wav";
                                break;
                            case "4F676753":
                                type = "ogg";
                                break;
                        }
                    }
                    labelSEInfo.Content = $"Audio type: {type}";
                    mediaPlayer.Source = uri;
                    //mediaPlayer.Play();
                }
                else
                {
                    labelSEInfo.Content = "Audio type: unknown";
                }
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
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

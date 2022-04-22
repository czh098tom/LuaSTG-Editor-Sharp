using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;
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
    /// ImageInput.xaml 的交互逻辑
    /// </summary>
    public partial class ImageInput : InputWindow
    {
        ObservableCollection<MetaModel> allImageInfo;
        ObservableCollection<MetaModel> allImageGroupInfo;
        ObservableCollection<MetaModel> allParticleInfo;
        ObservableCollection<MetaModel> allAnimationInfo;

        ObservableCollection<MetaModel> allImageInfoSys;
        ObservableCollection<MetaModel> allImageGroupInfoSys;
        ObservableCollection<MetaModel> allParticleInfoSys;
        ObservableCollection<MetaModel> allAnimationInfoSys;

        ObservableCollection<MetaModel> filteredImageInfo;
        ObservableCollection<MetaModel> filteredImageGroupInfo;
        ObservableCollection<MetaModel> filteredParticleInfo;
        ObservableCollection<MetaModel> filteredAnimationInfo;

        ObservableCollection<MetaModel> filteredImageInfoSys;
        ObservableCollection<MetaModel> filteredImageGroupInfoSys;
        ObservableCollection<MetaModel> filteredParticleInfoSys;
        ObservableCollection<MetaModel> filteredAnimationInfoSys;

        public ObservableCollection<MetaModel> FilteredImageInfo { get => filteredImageInfo; }
        public ObservableCollection<MetaModel> FilteredImageGroupInfo { get => filteredImageGroupInfo; }
        public ObservableCollection<MetaModel> FilteredParticleInfo { get => filteredParticleInfo; }
        public ObservableCollection<MetaModel> FilteredAnimationInfo { get => filteredAnimationInfo; }

        public ObservableCollection<MetaModel> FilteredImageInfoSys { get => filteredImageInfoSys; }
        public ObservableCollection<MetaModel> FilteredImageGroupInfoSys { get => filteredImageGroupInfoSys; }
        public ObservableCollection<MetaModel> FilteredParticleInfoSys { get => filteredParticleInfoSys; }
        public ObservableCollection<MetaModel> FilteredAnimationInfoSys { get => filteredAnimationInfoSys; }

        int cols = 1, rows = 1;

        DrawingVisual hint;

        private struct HGEParticleInformation
        {
        }

        public override string Result
        {
            get => result;
            set
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }

        private int selectedIndex = 1;
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                value = value < 1 ? 1 : value;
                value = value > cols * rows ? cols * rows : value;
                selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
                int selX = (value - 1) % cols;
                int selY = (value - 1) / cols;
                RefreshView(selX, selY);
            }
        }

        private bool mouseDown = false;

        public ImageInput(string s, AttrItem item, ImageClassType imageClassType = ImageClassType.None)
        {
            allImageInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.ImageLoad].GetAllSimpleWithDifficulty();
            allImageGroupInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.ImageGroupLoad].GetAllSimpleWithDifficulty();
            allParticleInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.ParticleLoad].GetAllSimpleWithDifficulty();
            allAnimationInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.AnimationLoad].GetAllSimpleWithDifficulty();
            AddInternalMetas();
            filteredImageInfo = new ObservableCollection<MetaModel>(allImageInfo);
            filteredImageGroupInfo = new ObservableCollection<MetaModel>(allImageGroupInfo);
            filteredParticleInfo = new ObservableCollection<MetaModel>(allParticleInfo);
            filteredAnimationInfo = new ObservableCollection<MetaModel>(allAnimationInfo);
            filteredImageInfoSys = new ObservableCollection<MetaModel>(allImageInfoSys);
            filteredImageGroupInfoSys = new ObservableCollection<MetaModel>(allImageGroupInfoSys);
            filteredParticleInfoSys = new ObservableCollection<MetaModel>(allParticleInfoSys);
            filteredAnimationInfoSys = new ObservableCollection<MetaModel>(allAnimationInfoSys);

            InitializeComponent();

            //BoxImageData.ItemsSource = ImageInfo;
            //BoxImageGroupData.ItemsSource = ImageGroupInfo;

            tabAnimation.Visibility = (imageClassType & ImageClassType.Animation) != 0 ? Visibility.Visible : Visibility.Collapsed;
            tabParticle.Visibility = (imageClassType & ImageClassType.Particle) != 0 ? Visibility.Visible : Visibility.Collapsed;

            Result = s;
            codeText.Text = Result;
        }

        private void FilterImage_TextChanged(object sender, RoutedEventArgs e)
        {
            filteredImageInfo.Clear();
            foreach (MetaModel mm in allImageInfo.Where(mm => MatchFilter(mm.FullName, filterImage.Text)))
            {
                filteredImageInfo.Add(mm);
            }
            filteredImageInfoSys.Clear();
            foreach (MetaModel mm in allImageInfoSys.Where(mm => MatchFilter(mm.FullName, filterImage.Text)))
            {
                filteredImageInfoSys.Add(mm);
            }
        }

        private void FilterImageGroup_TextChanged(object sender, RoutedEventArgs e)
        {
            filteredImageGroupInfo.Clear();
            foreach (MetaModel mm in allImageGroupInfo.Where(mm => MatchFilter(mm.FullName, filterImageGroup.Text)))
            {
                filteredImageGroupInfo.Add(mm);
            }
            filteredImageGroupInfoSys.Clear();
            foreach (MetaModel mm in allImageGroupInfoSys.Where(mm => MatchFilter(mm.FullName, filterImageGroup.Text)))
            {
                filteredImageGroupInfoSys.Add(mm);
            }
        }

        private void FilterParticle_TextChanged(object sender, RoutedEventArgs e)
        {
            filteredParticleInfo.Clear();
            foreach (MetaModel mm in allParticleInfo.Where(mm => MatchFilter(mm.FullName, filterParticle.Text)))
            {
                filteredParticleInfo.Add(mm);
            }
            filteredParticleInfoSys.Clear();
            foreach (MetaModel mm in allParticleInfoSys.Where(mm => MatchFilter(mm.FullName, filterParticle.Text)))
            {
                filteredParticleInfoSys.Add(mm);
            }
        }

        private void FilterAnimation_TextChanged(object sender, RoutedEventArgs e)
        {
            filteredAnimationInfo.Clear();
            foreach (MetaModel mm in allAnimationInfo.Where(mm => MatchFilter(mm.FullName, filterAnimation.Text)))
            {
                filteredAnimationInfo.Add(mm);
            }
            filteredAnimationInfoSys.Clear();
            foreach (MetaModel mm in allAnimationInfoSys.Where(mm => MatchFilter(mm.FullName, filterAnimation.Text)))
            {
                filteredAnimationInfoSys.Add(mm);
            }
        }

        private void AddInternalMetas()
        {
            allImageInfoSys = new ObservableCollection<MetaModel>(NodesConfig.SysImage);
            allImageGroupInfoSys = new ObservableCollection<MetaModel>(NodesConfig.SysImageGroup);
            allAnimationInfoSys = new ObservableCollection<MetaModel>();
            allParticleInfoSys = new ObservableCollection<MetaModel>();
        }

        private IEnumerable<MetaModel> EnumerateImageInfo()
        {
            foreach (MetaModel mm in allImageInfo) yield return mm;
            foreach (MetaModel mm in allImageInfoSys) yield return mm;
        }

        private IEnumerable<MetaModel> EnumerateImageGroupInfo()
        {
            foreach (MetaModel mm in allImageGroupInfo) yield return mm;
            foreach (MetaModel mm in allImageGroupInfoSys) yield return mm;
        }

        private ImageSource GetImage(string s)
        {
            char[] trimarr = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '"' };
            foreach (MetaModel mm in EnumerateImageInfo())
            {
                if (mm.Result == s) try { return new BitmapImage(new Uri(mm.ExInfo1)); } catch { }
            }
            foreach (MetaModel mm in EnumerateImageGroupInfo())
            {
                if (mm.Result.Trim('"') == s.Trim(trimarr))
                {
                    try
                    {
                        string data = s.Remove(0, mm.Result.Length - 1).Trim('"');
                        int id = int.Parse(data);
                        id--;
                        string[] colrow = mm.ExInfo2.Split(',');
                        int cols = 1, rows = 1;
                        if (colrow != null && colrow.Length > 1)
                        {
                            if (int.TryParse(colrow[0], out int colsX) && colsX > 0) cols = colsX;
                            if (int.TryParse(colrow[1], out int rowsX) && rowsX > 0) rows = rowsX;
                        }
                        return Util.BitmapUtil.CutImageByXY(new BitmapImage(new Uri(mm.ExInfo1)), id % cols, id / cols, cols, rows);
                    }
                    catch { }
                }
            }
            return null;
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

        private void BoxImageData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshAsImage();
        }

        private void BoxImageGroupData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshAsImageGroup();
        }

        private void BoxAnimationData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshAsAnimation();
        }

        private void BoxImageData_GotFocus(object sender, RoutedEventArgs e)
        {
            RefreshAsImage();
        }

        private void BoxImageGroupData_GotFocus(object sender, RoutedEventArgs e)
        {
            RefreshAsImageGroup();
        }

        private void BoxParticleData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshAsParticle();
        }

        private void RefreshAsImage()
        {
            MetaModel m = (BoxImageData.SelectedItem as MetaModel);
            if (m == null) return;
            if (!string.IsNullOrEmpty(m?.Result)) Result = m?.Result;
            try
            {
                ImageExample.Source = new BitmapImage(new Uri(m?.ExInfo1));
            }
            catch { }
            codeText.Focus();
        }

        private void RefreshAsImageGroup()
        {
            MetaModel m = (BoxImageGroupData.SelectedItem as MetaModel);
            if (m == null) return;
            string[] colrow = m?.ExInfo2.Split(',');
            int cols = 1, rows = 1;
            if (colrow != null && colrow.Length > 1)
            {
                if (int.TryParse(colrow[0], out int colsX) && colsX > 0) cols = colsX;
                if (int.TryParse(colrow[1], out int rowsX) && rowsX > 0) rows = rowsX;
            }
            this.cols = cols;
            this.rows = rows;
            if (!string.IsNullOrEmpty(m?.Result)) Result = "\"" + m.Result.Trim('"') + selectedIndex + "\"";
            SplitGrid.Background = GetGridBrush();
            try
            {
                ImageGroupExample.Source = new BitmapImage(new Uri(m?.ExInfo1));
            }
            catch { }
            SelectedIndex = 1;
            SplitGrid.RemoveVisual(hint);
            codeText.Focus();
        }

        private void RefreshAsParticle()
        {
            MetaModel m = (BoxParticleData.SelectedItem as MetaModel);
            if (m == null) return;
            if (!string.IsNullOrEmpty(m?.Result)) Result = m?.Result;
            try
            {
                string s = m?.ExInfo2;
                if (!string.IsNullOrEmpty(s))
                {
                    ParticleExample.Source = GetImage(s);
                }
            }
            catch { }

            BinaryReader sr = null;
            try
            {
                Uri uri = new Uri(m?.ExInfo1);
                if (uri.Scheme == "file")
                {
                    sr = new BinaryReader(new FileStream(m?.ExInfo1, FileMode.Open));
                }
                else
                {
                    StreamResourceInfo info = Application.GetResourceStream(uri);
                    sr = new BinaryReader(info.Stream);
                }
                StringBuilder sb = new StringBuilder();
                ReadHGEFormat(sr, sb);
                txtParticle.Text = sb.ToString();
            }
            finally
            {
                if (sr != null) sr.Close();
            }

            codeText.Focus();
        }

        private void RefreshAsAnimation()
        {
            MetaModel m = (BoxAnimationData.SelectedItem as MetaModel);
            if (m == null) return;
            if (!string.IsNullOrEmpty(m?.Result)) Result = m?.Result;
            try
            {
                AnimationExample.Source = new BitmapImage(new Uri(m?.ExInfo1));
            }
            catch { }
            codeText.Focus();
        }

        private DrawingBrush GetGridBrush()
        {
            DrawingBrush db = new DrawingBrush();
            DrawingGroup dg = new DrawingGroup();
            GeometryDrawing gd = new GeometryDrawing();
            GeometryGroup gg = new GeometryGroup();
            for (int i = 1; i < rows; i++)
            {
                gg.Children.Add(new LineGeometry(new Point(0, (double)i / rows), new Point(1, (double)i / rows)));
            }
            for (int i = 1; i < cols; i++)
            {
                gg.Children.Add(new LineGeometry(new Point((double)i / cols, 0), new Point((double)i / cols, 1)));
            }
            gd.Geometry = gg;
            gd.Pen = new Pen(new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)), 0.001);
            dg.Children.Add(gd);
            db.Drawing = dg;
            return db;
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

        private void SplitGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CalculateSelection(e);
            mouseDown = true;
        }

        private void SplitGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                CalculateSelection(e);
            }
        }

        private void CalculateSelection(MouseEventArgs e)
        {
            Point p = e.GetPosition(SplitGrid);
            int selX = Convert.ToInt32(p.X / SplitGrid.ActualWidth * cols - 0.5);
            int selY = Convert.ToInt32(p.Y / SplitGrid.ActualHeight * rows - 0.5);
            SelectedIndex = 1 + selX + selY * cols;
        }

        private void RefreshView(int selX, int selY)
        {
            DrawCursorAtPoint(selX, selY);
            MetaModel m = (BoxImageGroupData.SelectedItem as MetaModel);
            if (!string.IsNullOrEmpty(m?.Result)) Result = "\"" + m.Result.Trim('"') + selectedIndex + "\"";
        }

        private void DrawCursorAtPoint(int selX, int selY)
        {
            double x0 = selX * SplitGrid.ActualWidth / cols;
            double y0 = selY * SplitGrid.ActualHeight / rows;
            if (SplitGrid.ChildrenCount <= 0)
            {
                hint = new DrawingVisual();
                DrawRect(hint, new Point(x0, y0)
                    , new Point(x0 + SplitGrid.ActualWidth / cols, y0 + SplitGrid.ActualHeight / rows));
                SplitGrid.AddVisual(hint);
            }
            else
            {
                hint = (DrawingVisual)SplitGrid[0];
                DrawRect(hint, new Point(x0, y0)
                    , new Point(x0 + SplitGrid.ActualWidth / cols, y0 + SplitGrid.ActualHeight / rows));
            }
        }

        private void DrawRect(DrawingVisual v, Point p1, Point p2)
        {
            using (DrawingContext dc = v.RenderOpen())
            {
                dc.DrawRectangle(new SolidColorBrush(Color.FromArgb(128, 153, 217, 234))
                    , new Pen(new SolidColorBrush(Color.FromArgb(255, 0, 162, 232)), 1.0), new Rect(p1, p2));
            }
        }

        private void SplitGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {
                RefreshAsImage();
            }
            else if (tabControl.SelectedIndex == 1)
            {
                RefreshAsImageGroup();
            }
            else if (tabControl.SelectedIndex == 2)
            {
                RefreshAsParticle();
            }
            else
            {
                RefreshAsAnimation();
            }
        }

        private static void ReadHGEFormat(BinaryReader sr, StringBuilder sb)
        {
            sb.Append("Default index: ");
            sb.Append(sr.ReadByte());
            sr.ReadByte();
            sb.Append("\nBlend mode: ");
            sb.Append(sr.ReadByte());
            sr.ReadByte();
            sb.Append("\nEmission: ");
            sb.Append(sr.ReadInt32());
            sb.Append(" p/sec\nSystem life time: ");
            float f = sr.ReadSingle();
            sb.Append(f == -1 ? "infinite" : f.ToString());
            sb.Append(" sec\n\nParticle life time: ");
            sb.Append(sr.ReadSingle());
            sb.Append(" ~ ");
            sb.Append(sr.ReadSingle());
            sb.Append(" sec\n\nDirection: ");
            sb.Append(sr.ReadSingle());
            sb.Append(" deg\nSpread: ");
            sb.Append(sr.ReadSingle());
            sb.Append(" deg\nIs relative: ");
            sb.Append(sr.ReadBoolean());
            sr.ReadByte();
            sr.ReadByte();
            sr.ReadByte();
            sb.Append("\n\nSpeed Range: ");
            sb.Append(sr.ReadSingle());
            sb.Append(" ~ ");
            sb.Append(sr.ReadSingle());
            sb.Append("\nGravity Range: ");
            sb.Append(sr.ReadSingle());
            sb.Append(" ~ ");
            sb.Append(sr.ReadSingle());
            sb.Append("\nRadial Acceleration Range: ");
            sb.Append(sr.ReadSingle());
            sb.Append(" ~ ");
            sb.Append(sr.ReadSingle());
            sb.Append("\nTangential Acceleration Range: ");
            sb.Append(sr.ReadSingle());
            sb.Append(" ~ ");
            sb.Append(sr.ReadSingle());
            sb.Append("\n\nSize: ");
            sb.Append(sr.ReadSingle());
            sb.Append(" -> ");
            sb.Append(sr.ReadSingle());
            sb.Append(" variation: ");
            sb.Append(sr.ReadSingle());
            sb.Append("\nSpin: ");
            sb.Append(sr.ReadSingle());
            sb.Append(" -> ");
            sb.Append(sr.ReadSingle());
            sb.Append(" variation: ");
            sb.Append(sr.ReadSingle());
            sb.Append("\nColor: ");
            sb.Append(Convert.ToInt32(sr.ReadSingle() * 255));
            sb.Append(" ");
            sb.Append(Convert.ToInt32(sr.ReadSingle() * 255));
            sb.Append(" ");
            sb.Append(Convert.ToInt32(sr.ReadSingle() * 255));
            sb.Append(" -> ");
            sb.Append(Convert.ToInt32(sr.ReadSingle() * 255));
            sb.Append(" ");
            sb.Append(Convert.ToInt32(sr.ReadSingle() * 255));
            sb.Append(" ");
            sb.Append(Convert.ToInt32(sr.ReadSingle() * 255));
            sb.Append("\ncolor variation: ");
            sb.Append(sr.ReadSingle());
            sb.Append("\nalpha variation: ");
            sb.Append(sr.ReadSingle());
        }
    }
}

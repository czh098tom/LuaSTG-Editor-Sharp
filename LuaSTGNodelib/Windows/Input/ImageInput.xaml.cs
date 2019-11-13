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
    /// ImageInput.xaml 的交互逻辑
    /// </summary>
    public partial class ImageInput : InputWindow
    {
        public ObservableCollection<MetaModel> imageInfo;
        public ObservableCollection<MetaModel> ImageInfo { get => imageInfo; }
        public ObservableCollection<MetaModel> imageGroupInfo;
        public ObservableCollection<MetaModel> ImageGroupInfo { get => imageGroupInfo; }
        public ObservableCollection<MetaModel> animationInfo;
        public ObservableCollection<MetaModel> AnimationInfo { get => animationInfo; }

        public ObservableCollection<MetaModel> imageInfoSys;
        public ObservableCollection<MetaModel> ImageInfoSys { get => imageInfoSys; }
        public ObservableCollection<MetaModel> imageGroupInfoSys;
        public ObservableCollection<MetaModel> ImageGroupInfoSys { get => imageGroupInfoSys; }
        public ObservableCollection<MetaModel> animationInfoSys;
        public ObservableCollection<MetaModel> AnimationInfoSys { get => animationInfoSys; }

        int cols = 1, rows = 1;

        DrawingVisual hint;

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

        public ImageInput(string s, MainWindow owner, AttrItem item, ImageClassType imageClassType = ImageClassType.None)
        {
            imageInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.ImageLoad].GetAllSimpleWithDifficulty();
            imageGroupInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.ImageGroupLoad].GetAllSimpleWithDifficulty();
            animationInfo = item.Parent.parentWorkSpace.Meta.aggregatableMetas[(int)MetaType.AnimationLoad].GetAllSimpleWithDifficulty();

            AddInternalMetas();

            InitializeComponent();

            //BoxImageData.ItemsSource = ImageInfo;
            //BoxImageGroupData.ItemsSource = ImageGroupInfo;

            tabAnimation.Visibility = (imageClassType & ImageClassType.Animation) != 0 ? Visibility.Visible : Visibility.Collapsed;

            Result = s;
            codeText.Text = Result;
        }

        private void AddInternalMetas()
        {
            imageInfoSys = new ObservableCollection<MetaModel>(PluginEntry.SysImage);

            imageGroupInfoSys = new ObservableCollection<MetaModel>(PluginEntry.SysImageGroup);

            animationInfoSys = new ObservableCollection<MetaModel>();
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

        private void RefreshAsImage()
        {
            MetaModel m = (BoxImageData.SelectedItem as MetaModel);
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

        private void RefreshAsAnimation()
        {
            MetaModel m = (BoxAnimationData.SelectedItem as MetaModel);
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
            for(int i = 1; i < rows; i++)
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
            if(e.Key==Key.Enter)
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
            else
            {
                RefreshAsAnimation();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using LuaSTGEditorSharp.Windows.Input.Canvas;

namespace LuaSTGEditorSharp.Windows.Input
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class SizeInput : InputWindow
    {
        private bool? clipTo10 = null;

        private double selectedX, selectedY;

        private bool dragStarted = false;

        public override string Result
        {
            get => base.Result;
            set
            {
                base.Result = value;
                List<string> cs = Separate(Result);
                if (cs.Count >= 1 && !string.IsNullOrEmpty(cs[0]) && double.TryParse(cs[0], out double b1)) SelectedX = b1;
                if (cs.Count >= 2 && !string.IsNullOrEmpty(cs[1]) && double.TryParse(cs[1], out double b2)) SelectedY = b2;
            }
        }

        public void CombineResult()
        {
            result = SelectedX + "," + SelectedY;
            RaisePropertyChanged("Result");
        }

        public double SelectedX
        {
            get => Math.Abs(selectedX);
            set
            {
                selectedX = value;
                RaisePropertyChanged("SelectedX");
                Point p = new Point(DrawingCanvas.LSTGXToScrX(selectedX), DrawingCanvas.LSTGYToScrY(selectedY));
                DrawCursorAtPoint(p);
                CombineResult();
            }
        }

        public double SelectedY
        {
            get => Math.Abs(selectedY);
            set
            {
                selectedY = value;
                RaisePropertyChanged("SelectedY");
                Point p = new Point(DrawingCanvas.LSTGXToScrX(selectedX), DrawingCanvas.LSTGYToScrY(selectedY));
                DrawCursorAtPoint(p);
                CombineResult();
            }
        }

        public SizeInput(string s)
        {
            //DialogResult = false;
            InitializeComponent();
            Result = s;
            codeText.Text = Result;
        }

        private SolidColorBrush brushRect = new SolidColorBrush(Color.FromArgb(128, 100, 48, 254));
        private SolidColorBrush brushEllipse = new SolidColorBrush(Color.FromArgb(128, 255, 181, 26));

        private void DrawCursor(DrawingVisual dv, Point pos)
        {
            using (DrawingContext dc = dv.RenderOpen())
            {
                Point p0 = new Point(DrawingCanvas.LSTGXToScrX(0), DrawingCanvas.LSTGYToScrY(0));
                Point p1 = new Point(p0.X * 2 - pos.X, p0.Y * 2 - pos.Y);
                Point p2 = new Point(+pos.X, +pos.Y);
                dc.DrawRectangle(brushRect, null, new Rect(p1, p2));
                dc.DrawEllipse(brushEllipse, null, p0, pos.X - p0.X, pos.Y - p0.Y);
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(Canvas);
            DrawCursorAtPoint(point);
            SelectedX= DrawingCanvas.ScrXToLSTGX(point.X, clipTo10);
            SelectedY = DrawingCanvas.ScrYToLSTGY(point.Y, clipTo10);
            dragStarted = true;
        }

        private void DrawCursorAtPoint(Point point)
        {
            DrawingVisual v;
            if (Canvas.ChildrenCount <= 0)
            {
                v = new DrawingVisual();
                DrawCursor(v, point);
                Canvas.AddVisual(v);
            }
            else
            {
                v = (DrawingVisual)Canvas[0];
                DrawCursor(v, point);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dragStarted = false;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(Canvas);
            if (dragStarted)
            {
                DrawCursorAtPoint(point);
                SelectedX = DrawingCanvas.ScrXToLSTGX(point.X, clipTo10);
                SelectedY = DrawingCanvas.ScrYToLSTGY(point.Y, clipTo10);
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

        private void NotClip_Click(object sender, RoutedEventArgs e)
        {
            clipTo10 = null;
        }

        private void ClipTo1_Click(object sender, RoutedEventArgs e)
        {
            clipTo10 = false;
        }

        private void ClipTo10_Click(object sender, RoutedEventArgs e)
        {
            clipTo10 = true;
        }

        private void InputWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(codeText);
        }

        private void Text_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                this.Close();
            }
        }
    }
}

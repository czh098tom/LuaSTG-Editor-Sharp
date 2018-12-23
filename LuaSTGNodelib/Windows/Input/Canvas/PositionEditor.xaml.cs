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

namespace LuaSTGEditorSharp.Windows.Input.Canvas
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class PositionEditor : Window, INotifyPropertyChanged
    {
        private double selectedX, selectedY;

        private bool dragStarted = false;

        public double SelectedX
        {
            get => selectedX;
            set
            {
                selectedX = value;
                RaisePropertyChanged("SelectedX");
                Point p = new Point(DrawingCanvas.LSTGXToScrX(selectedX), DrawingCanvas.LSTGYToScrY(selectedY));
                DrawCursorAtPoint(p);
            }
        }

        public double SelectedY
        {
            get => selectedY;
            set
            {
                selectedY = value;
                RaisePropertyChanged("SelectedY");
                Point p = new Point(DrawingCanvas.LSTGXToScrX(selectedX), DrawingCanvas.LSTGYToScrY(selectedY));
                DrawCursorAtPoint(p);
            }
        }

        public PositionEditor()
        {
            //DialogResult = false;
            InitializeComponent();
        }

        private Pen drawingPen = new Pen(Brushes.AliceBlue, 2);
        private Pen drawingPenBorder = new Pen(Brushes.Black, 3);

        private void DrawCursor(DrawingVisual dv, Point pos)
        {
            using (DrawingContext dc = dv.RenderOpen())
            {
                Point p1 = new Point(pos.X - 5, pos.Y - 5);
                Point p2 = new Point(pos.X + 5, pos.Y + 5);
                Point p3 = new Point(pos.X - 5, pos.Y + 5);
                Point p4 = new Point(pos.X + 5, pos.Y - 5);
                dc.DrawLine(drawingPenBorder, p1, p2);
                dc.DrawLine(drawingPenBorder, p3, p4);
                dc.DrawLine(drawingPen, p1, p2);
                dc.DrawLine(drawingPen, p3, p4);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(Canvas);
            DrawCursorAtPoint(point);
            SelectedX= DrawingCanvas.ScrXToLSTGX(point.X);
            SelectedY = DrawingCanvas.ScrYToLSTGY(point.Y);
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
                SelectedX = DrawingCanvas.ScrXToLSTGX(point.X);
                SelectedY = DrawingCanvas.ScrYToLSTGY(point.Y);
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public void RaisePropertyChanged(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }
    }
}

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
    public partial class VectorEditor : Window, INotifyPropertyChanged
    {
        private double selectedX, selectedY;

        private double beginX, beginY;

        private bool headDragStarted = false;
        private bool tailDragStarted = false;

        public double BeginX
        {
            get => beginX;
            set
            {
                selectedX += value - beginX;
                beginX = value;
                RaisePropertyChanged("BeginX");
                Point p = new Point(DrawingCanvas.LSTGXToScrX(selectedX), DrawingCanvas.LSTGYToScrY(selectedY));
                DrawCursorAtPoint(p);
                RaisePropertyChanged("SelectedX");
            }
        }

        public double BeginY
        {
            get => beginY;
            set
            {
                selectedY += value - beginY;
                beginY = value;
                RaisePropertyChanged("BeginY");
                Point p = new Point(DrawingCanvas.LSTGXToScrX(selectedX), DrawingCanvas.LSTGYToScrY(selectedY));
                DrawCursorAtPoint(p);
                RaisePropertyChanged("SelectedY");
            }
        }

        public double SelectedX
        {
            get => selectedX;
            set
            {
                selectedX = value;
                RaisePropertyChanged("SelectedX");
                Point p = new Point(DrawingCanvas.LSTGXToScrX(selectedX), DrawingCanvas.LSTGYToScrY(selectedY));
                DrawCursorAtPoint(p);
                RaisePropertyChanged("OffsetX");
                RaisePropertyChanged("Radius");
                RaisePropertyChanged("Theta");
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
                RaisePropertyChanged("OffsetY");
                RaisePropertyChanged("Radius");
                RaisePropertyChanged("Theta");
            }
        }

        public double OffsetX
        {
            get => selectedX - beginX;
            set
            {
                SelectedX = beginX + value;
                RaisePropertyChanged("OffsetX");
                RaisePropertyChanged("Radius");
                RaisePropertyChanged("Theta");
            }
        }

        public double OffsetY
        {
            get => selectedY - beginY;
            set
            {
                SelectedY = beginY + value;
                RaisePropertyChanged("OffsetY");
                RaisePropertyChanged("Radius");
                RaisePropertyChanged("Theta");
            }
        }

        public double Radius
        {
            get => Math.Sqrt(OffsetX * OffsetX + OffsetY * OffsetY);
            set
            {
                double ori = value / Math.Sqrt(OffsetX * OffsetX + OffsetY * OffsetY);
                OffsetX *= ori;
                OffsetY *= ori;
                RaisePropertyChanged("Radius");
            }
        }

        public double Theta
        {
            get => Math.Atan2(OffsetY, OffsetX) / Math.PI * 180;
            set
            {
                double ori = Math.Sqrt(OffsetX * OffsetX + OffsetY * OffsetY);
                double ang = value / 180 * Math.PI;
                OffsetX = Math.Cos(ang) * ori;
                OffsetY = Math.Sin(ang) * ori;
                RaisePropertyChanged("Theta");
            }
        }

        public VectorEditor()
        {
            //DialogResult = false;
            InitializeComponent();
        }

        private Brush beginBrush = Brushes.AliceBlue;
        private Pen beginBorder = new Pen(Brushes.Black, 1);
        private Pen drawingPen = new Pen(Brushes.AliceBlue, 2);
        private Pen drawingPenBorder = new Pen(Brushes.Black, 3);

        private void DrawCursor(DrawingVisual dv, Point begin, Point pos)
        {
            using (DrawingContext dc = dv.RenderOpen())
            {
                Point p1 = new Point(pos.X - 5, pos.Y - 5);
                Point p2 = new Point(pos.X + 5, pos.Y + 5);
                Point p3 = new Point(pos.X - 5, pos.Y + 5);
                Point p4 = new Point(pos.X + 5, pos.Y - 5);
                dc.DrawLine(drawingPenBorder, begin, pos);
                dc.DrawLine(drawingPen, begin, pos);
                dc.DrawLine(drawingPenBorder, p1, p2);
                dc.DrawLine(drawingPenBorder, p3, p4);
                dc.DrawLine(drawingPen, p1, p2);
                dc.DrawLine(drawingPen, p3, p4);
                dc.DrawEllipse(beginBrush, beginBorder, begin, 5, 5);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(Canvas);
            DrawCursorAtPoint(point);
            OffsetX = DrawingCanvas.ScrXToLSTGX(point.X) - beginX;
            OffsetY = DrawingCanvas.ScrYToLSTGY(point.Y) - beginY;
            headDragStarted = true;
        }

        private void DrawCursorAtPoint(Point point)
        {
            DrawingVisual v;
            if (Canvas.ChildrenCount <= 0)
            {
                v = new DrawingVisual();
                DrawCursor(v, new Point(DrawingCanvas.LSTGXToScrX(BeginX), DrawingCanvas.LSTGYToScrY(BeginY)), point);
                Canvas.AddVisual(v);
            }
            else
            {
                v = (DrawingVisual)Canvas[0];
                DrawCursor(v, new Point(DrawingCanvas.LSTGXToScrX(BeginX), DrawingCanvas.LSTGYToScrY(BeginY)), point);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            headDragStarted = false;
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(Canvas);
            DrawCursorAtPoint(new Point(selectedX, selectedY));
            BeginX = DrawingCanvas.ScrXToLSTGX(point.X);
            BeginY = DrawingCanvas.ScrYToLSTGY(point.Y);
            tailDragStarted = true;
        }

        private void Canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            tailDragStarted = false;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(Canvas);
            if (headDragStarted)
            {
                DrawCursorAtPoint(point);
                OffsetX = DrawingCanvas.ScrXToLSTGX(point.X) - beginX;
                OffsetY = DrawingCanvas.ScrYToLSTGY(point.Y) - beginY;
            }
            else if (tailDragStarted)
            {
                DrawCursorAtPoint(new Point(selectedX, selectedY));
                BeginX = DrawingCanvas.ScrXToLSTGX(point.X);
                BeginY = DrawingCanvas.ScrYToLSTGY(point.Y);
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

using System;
using System.Collections.Generic;
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
    /// SingleLineInput.xaml 的交互逻辑
    /// </summary>
    public partial class ARGBInput : InputWindow
    {
        public struct HSVColor
        {
            public float hue;
            public float saturation;
            public float value;

            public HSVColor(float h,float s,float v) { hue = h; saturation = s; value = v; }
        }

        private bool dragStarted = false;

        private byte a = 255;
        private float h = 0f;
        private float s = 100f;
        private float v = 100f;

        private string aStr, rStr, gStr, bStr;

        public string AStr
        {
            get
            {
                if(byte.TryParse(aStr, out byte _))
                {
                    return A.ToString();
                }
                else
                {
                    return aStr;
                }
            }
            set
            {
                if(byte.TryParse(value, out byte result))
                {
                    A = result;
                }
                else
                {
                    aStr = value;
                    CombineResult();
                    RaisePropertyChanged("AStr");
                }
            }
        }

        public string RStr
        {
            get
            {
                if (byte.TryParse(rStr, out _))
                {
                    return R.ToString();
                }
                else
                {
                    return rStr;
                }
            }
            set
            {
                if (byte.TryParse(value, out byte result))
                {
                    R = result;
                }
                else
                {
                    rStr = value;
                    CombineResult();
                }
                RaisePropertyChanged("RStr");
            }
        }

        public string GStr
        {
            get
            {
                if (byte.TryParse(gStr, out _))
                {
                    return G.ToString();
                }
                else
                {
                    return gStr;
                }
            }
            set
            {
                if (byte.TryParse(value, out byte result))
                {
                    G = result;
                }
                else
                {
                    gStr = value;
                    CombineResult();
                }
                RaisePropertyChanged("GStr");
            }
        }

        public string BStr
        {
            get
            {
                if (byte.TryParse(bStr, out _))
                {
                    return B.ToString();
                }
                else
                {
                    return bStr;
                }
            }
            set
            {
                if (byte.TryParse(value, out byte result))
                {
                    B = result;
                }
                else
                {
                    bStr = value;
                    CombineResult();
                }
                RaisePropertyChanged("BStr");
            }
        }

        public byte A
        {
            get => a;
            set
            {
                a = value;
                aStr = value.ToString();
                UpdateState();
                CombineResult();
                RaisePropertyChanged("AStr");
            }
        }

        public byte R
        {
            get => HSVToRGB(new HSVColor(h, s, v)).R;
            set
            {
                HSVColor hsv = RGBToHSV(Color.FromRgb(value, G, B));
                h = hsv.hue;
                s = hsv.saturation;
                v = hsv.value;
                UpdateState();
                RaisePropertyChanged("H");
                RaisePropertyChanged("S");
                RaisePropertyChanged("V");
                rStr = value.ToString();
                CombineResult();
            }
        }

        public byte G
        {
            get => HSVToRGB(new HSVColor(h, s, v)).G;
            set
            {
                HSVColor hsv = RGBToHSV(Color.FromRgb(R, value, B));
                h = hsv.hue;
                s = hsv.saturation;
                v = hsv.value;
                UpdateState();
                RaisePropertyChanged("H");
                RaisePropertyChanged("S");
                RaisePropertyChanged("V");
                gStr = value.ToString();
                CombineResult();
            }
        }

        public byte B
        {
            get => HSVToRGB(new HSVColor(h, s, v)).B;
            set
            {
                HSVColor hsv = RGBToHSV(Color.FromRgb(R, G, value));
                h = hsv.hue;
                s = hsv.saturation;
                v = hsv.value;
                UpdateState();
                RaisePropertyChanged("H");
                RaisePropertyChanged("S");
                RaisePropertyChanged("V");
                bStr = value.ToString();
                CombineResult();
            }
        }

        public float H
        {
            get => h;
            set
            {
                h = value;
                UpdateState();
                RaisePropertyChanged("H");
                RaisePropertyChanged("RStr");
                RaisePropertyChanged("GStr");
                RaisePropertyChanged("BStr");
                CombineResult();
            }
        }

        public float S
        {
            get => s;
            set
            {
                s = value;
                UpdateState();
                RaisePropertyChanged("S");
                RaisePropertyChanged("RStr");
                RaisePropertyChanged("GStr");
                RaisePropertyChanged("BStr");
                CombineResult();
            }
        }

        public float V
        {
            get => v;
            set
            {
                v = value;
                UpdateState();
                RaisePropertyChanged("V");
                RaisePropertyChanged("RStr");
                RaisePropertyChanged("GStr");
                RaisePropertyChanged("BStr");
                CombineResult();
            }
        }

        public override string Result
        {
            get => base.Result;
            set
            {
                base.Result = value;
                List<string> cs = Separate(Result);
                if (cs.Count >= 1) AStr = cs[0];
                if (cs.Count >= 2) RStr = cs[1];
                if (cs.Count >= 3) GStr = cs[2];
                if (cs.Count >= 4) BStr = cs[3];
            }
        }

        public void CombineResult()
        {
            result = AStr + "," + RStr + "," + GStr + "," + BStr;
            RaisePropertyChanged("Result");
        }

        public void CombineResult(string a, string r, string g, string b)
        {
            result = a + "," + r + "," + g + "," + b;
            RaisePropertyChanged("Result");
        }

        public static Color HSVToRGB(HSVColor HSV)
        {
            HSV.hue -= Convert.ToSingle(Math.Floor(HSV.hue / 360) * 360);
            HSV.saturation /= 100;
            HSV.value /= 100;
            byte v = Convert.ToByte(HSV.value * 255);
            if (HSV.saturation == 0)
            {
                return Color.FromArgb(255, v, v, v);
            }
            int h = Convert.ToInt32(Math.Floor(((float)HSV.hue) / 60)) % 6;
            float f = ((float)HSV.hue) / 60 - h;
            byte a = Convert.ToByte(v * (1 - HSV.saturation));
            byte b = Convert.ToByte(v * (1 - HSV.saturation * f));
            byte c = Convert.ToByte(v * (1 - HSV.saturation * (1 - f)));
            switch (h)
            {
                case 0:
                    return Color.FromArgb(255, v, c, a);
                case 1:
                    return Color.FromArgb(255, b, v, a);
                case 2:
                    return Color.FromArgb(255, a, v, c);
                case 3:
                    return Color.FromArgb(255, a, b, v);
                case 4:
                    return Color.FromArgb(255, c, a, v);
                case 5:
                    return Color.FromArgb(255, v, a, b);
                default:
                    throw new NotImplementedException();
            }
        }

        public static HSVColor RGBToHSV(Color RGB)
        {
            HSVColor hsv = new HSVColor();
            byte max = Math.Max(RGB.R, RGB.G);
            max = Math.Max(max, RGB.B);
            byte min = Math.Min(RGB.R, RGB.G);
            min = Math.Min(min, RGB.B);
            hsv.value = ((float)max) / 255;
            int mm = max - min;
            if (max == 0)
            {
                hsv.saturation = 0;
            }
            else
            {
                hsv.saturation = ((float)mm) / max;
            }
            if (mm == 0)
            {
                hsv.hue = 0;
            }
            else if (RGB.R == max)
            {
                hsv.hue = ((float)(RGB.G - RGB.B)) / mm * 60;
            }
            else if(RGB.G == max)
            {
                hsv.hue = 120 + ((float)(RGB.B - RGB.R)) / mm * 60;
            }
            else if (RGB.B == max)
            {
                hsv.hue = 240 + ((float)(RGB.R - RGB.G)) / mm * 60;
            }
            if (hsv.hue < 0) hsv.hue += 360;
            hsv.saturation *= 100;
            hsv.value *= 100;
            return hsv;
        }

        public LinearGradientBrush GetWCBrush(byte r, byte g, byte b)
        {
            LinearGradientBrush brush = new LinearGradientBrush
            {
                EndPoint = new Point(1, 0)
            };
            GradientStopCollection gradientStops = new GradientStopCollection
            {
                new GradientStop(Color.FromArgb(255, 255, 255, 255), 0),
                new GradientStop(Color.FromArgb(255, r, g, b), 1)
            };
            brush.GradientStops = gradientStops;
            return brush;
        }

        public LinearGradientBrush GetCTBrush(byte r, byte g, byte b)
        {
            LinearGradientBrush brush = new LinearGradientBrush
            {
                EndPoint = new Point(0, 7.5),
                StartPoint = new Point(0, 248.5)
            };
            brush.MappingMode = BrushMappingMode.Absolute;
            GradientStopCollection gradientStops = new GradientStopCollection
            {
                new GradientStop(Color.FromArgb(255, r, g, b), 0),
                new GradientStop(Color.FromArgb(0, r, g, b), 1)
            };
            brush.GradientStops = gradientStops;
            return brush;
        }

        public ARGBInput(string s, MainWindow owner)
        {
            InitializeComponent();
            Result = s;
            //codeText.Text = Result;
            UpdateState();
        }

        public void UpdateState()
        {
            HSVColor hsv = new HSVColor(h, s, v);
            Color current = HSVToRGB(hsv);
            current.A = a;
            //update rgb (overwrite RGB text)
            rStr = current.R.ToString();
            gStr = current.G.ToString();
            bStr = current.B.ToString();
            Color maxHue = HSVToRGB(new HSVColor(hsv.hue, 100f, 100f));
            ColorCanvas.Background = GetWCBrush(maxHue.R, maxHue.G, maxHue.B);
            ScrollAlpha.Background = GetCTBrush(maxHue.R, maxHue.G, maxHue.B);
            ColorCurrent.Background = new SolidColorBrush(current);
            DrawCursorAtPoint(new Point(hsv.saturation * 255 / 100, 255 - hsv.value * 255 / 100));
        }

        private readonly Pen drawingPen = new Pen(Brushes.AliceBlue, 2);
        private readonly Pen drawingPenBorder = new Pen(Brushes.Black, 3);

        private void DrawCursor(DrawingVisual dv, Point pos)
        {
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawEllipse(Brushes.Transparent, drawingPenBorder, pos, 5, 5);
                dc.DrawEllipse(Brushes.Transparent, drawingPen, pos, 5, 5);
            }
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

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("x: " + e.GetPosition(Canvas).X + ", y: " + e.GetPosition(Canvas).Y);
            Point point = e.GetPosition(Canvas);
            UpdatePoint(point);
            Canvas.CaptureMouse();
            dragStarted = true;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragStarted)
            {
                Point point = e.GetPosition(Canvas);
                UpdatePoint(point);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas.ReleaseMouseCapture();
            dragStarted = false;
        }

        private void UpdatePoint(Point point)
        {
            point.X = point.X > 255 ? 255 : point.X;
            point.X = point.X < 0 ? 0 : point.X;
            point.Y = point.Y > 255 ? 255 : point.Y;
            point.Y = point.Y < 0 ? 0 : point.Y;
            s = Convert.ToSingle(point.X / 255 * 100);
            v = Convert.ToSingle((255 - point.Y) / 255 * 100);
            UpdateState();
            RaisePropertyChanged("S");
            RaisePropertyChanged("V");
            RaisePropertyChanged("RStr");
            RaisePropertyChanged("GStr");
            RaisePropertyChanged("BStr");
            CombineResult();
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

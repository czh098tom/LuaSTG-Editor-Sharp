using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class PointSetInput : InputWindow
    {

        private TextBox prevFocused = null;
        private int prevFocusedSelBeg = 0;
        private int prevFocusedSelLength = 0;

        private StrVector selectedVec;

        private ObservableCollection<StrVector> decomposedVectors = new ObservableCollection<StrVector>();

        static List<string> SeparatePointSet(string s)
        {
            try
            {
                List<string> vs = new List<string>();
                int lastlocptr = 0;
                char[] c = s.Trim().ToCharArray();
                Stack<char> expr = new Stack<char>();
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i] == '(' || c[i] == '[' || c[i] == '{')
                    {
                        expr.Push(c[i]);
                    }
                    else if (c[i] == ')' || c[i] == ']' || c[i] == '}')
                    {
                        if (expr.Peek() == '(' && c[i] == ')') expr.Pop();
                        else if (expr.Peek() == '[' && c[i] == ']') expr.Pop();
                        else if (expr.Peek() == '{' && c[i] == '}') expr.Pop();
                        else throw new InvalidOperationException();
                    }
                    else if (c[i] == ',' && i != 0)
                    {
                        if (expr.Count == 0)
                        {
                            vs.Add(new string(c, lastlocptr, i - lastlocptr));
                            lastlocptr = i + 1;
                        }
                    }
                }
                vs.Add(new string(c, lastlocptr, c.Length - lastlocptr));
                return vs;
            }
            catch (InvalidOperationException)
            {
                return new List<string>() { s };
            }
        }

        private ObservableCollection<StrVector> DecomposedVectors { get => decomposedVectors; set => decomposedVectors = value; }

        public string ResultTXT
        {
            get => Result;
            set
            {
                Result = value;
                RaisePropertyChanged("ResultTXT");
                ReCalcPointSet();
                RaisePropertyChanged("DecomposedX");
                RaisePropertyChanged("DecomposedY");
            }
        }

        public string CurrentX
        {
            get => selectedVec?.X;
            set
            {
                if (selectedVec != null)
                {
                    selectedVec.X = value;
                    ReCalcResult();
                    RaisePropertyChanged("CurrentX");
                    object a = sumBox.SelectedItem;
                    sumBox.ItemsSource = null;
                    sumBox.ItemsSource = decomposedVectors;
                    sumBox.SelectedItem = a;
                    RaisePropertyChanged("DecomposedX");
                    RaisePropertyChanged("ResultTXT");
                }
            }
        }

        public string CurrentY
        {
            get => selectedVec?.Y;
            set
            {
                if(selectedVec!=null)
                {
                    selectedVec.Y = value;
                    ReCalcResult();
                    RaisePropertyChanged("CurrentY");
                    object a = sumBox.SelectedItem;
                    sumBox.ItemsSource = null;
                    sumBox.ItemsSource = decomposedVectors;
                    sumBox.SelectedItem = a;
                    RaisePropertyChanged("DecomposedY");
                    RaisePropertyChanged("ResultTXT");
                }
            }
        }

        private class StrVector
        {
            public string X { get; set; } = "";
            public string Y { get; set; } = "";

            public string Display
            {
                get
                {
                    return "(" + X + ", " + Y + ")";
                }
            }
        }

        private void ReCalcResult()
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach(StrVector sv in decomposedVectors)
            {
                if (!first) sb.Append(", ");
                sb.Append(sv.X);
                sb.Append(", ");
                sb.Append(sv.Y);
                first = false;
            }
            Result = sb.ToString();
        }

        public PointSetInput(string s)
        {
            Result = s;
            ReCalcPointSet();
            InitializeComponent();
            sumBox.ItemsSource = decomposedVectors;
        }

        private void ReCalcPointSet()
        {
            decomposedVectors.Clear();
            List<string> xPoly = new List<string>();
            List<string> yPoly = new List<string>();
            int id = 0;
            foreach(string s in SeparatePointSet(ResultTXT))
            {
                string s2 = s.Trim();
                if (id % 2 == 0 && !string.IsNullOrEmpty(s2)) xPoly.Add(s2);
                else if (id % 2 != 0 && !string.IsNullOrEmpty(s2)) yPoly.Add(s2);
                id++;
            }
            int tx = xPoly.Count;
            int ty = yPoly.Count;
            int maxTerm = tx > ty ? tx : ty;
            for (int i = 0; i < maxTerm; i++)
            {
                string x, y;
                if (i < tx) x = xPoly[i]; else x = "0";
                if (i < ty) y = yPoly[i]; else y = "0";
                decomposedVectors.Add(new StrVector() { X = x, Y = y });
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
        
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            if (prevFocused != null) 
            {
                if(sender is Button b)
                {
                    int pfb = prevFocusedSelBeg;
                    TextBox prevFocus = prevFocused;
                    prevFocus.Select(prevFocusedSelBeg, prevFocusedSelLength);
                    string sbeg = prevFocus.Text.Substring(0, prevFocusedSelBeg);
                    string send = prevFocus.Text.Substring(prevFocusedSelBeg + prevFocusedSelLength);
                    prevFocus.Text = sbeg + (b.Tag?.ToString() ?? "") + send;
                    prevFocus.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
                    prevFocus.Focus();
                    prevFocus.Select(pfb + (b.Tag?.ToString().Length ?? 0), 0);
                }
            }
        }

        private static bool IsEmpty(string s)
        {
            if (string.IsNullOrEmpty(s)) return true;
            try
            {
                if (Convert.ToInt32(s) == 0) return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        private bool? XToBeSynced()
        {
            if(IsEmpty(CurrentX))
            {
                if(IsEmpty(CurrentY))
                {
                    return null;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (IsEmpty(CurrentY))
                {
                    return false;
                }
                else
                {
                    return prevFocused.Tag?.ToString() == "Y";
                }
            }
        }

        private void InputWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(codeText);
        }

        private void Text_GotFocus(object sender, RoutedEventArgs e)
        {
            prevFocused = sender as TextBox;
            prevFocusedSelBeg = prevFocused.SelectionStart;
            prevFocusedSelLength = prevFocused.SelectionLength;
        }

        private void SumBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedVec = sumBox.SelectedItem as StrVector;
            RaisePropertyChanged("CurrentX");
            RaisePropertyChanged("CurrentY");
        }

        private void SyncXY_Click(object sender, RoutedEventArgs e)
        {
            bool? syncType = XToBeSynced();
            if (syncType!=null)
            {
                if (syncType == true)
                {
                    string tempx = Regex.Replace(CurrentY, @"(?<![a-zA-Z])x\b", "____TEMPy___");
                    tempx = Regex.Replace(tempx, @"(?<![a-zA-Z])y\b", "x");
                    tempx = Regex.Replace(tempx, @"(?<![a-zA-Z])____TEMPy___\b", "y");
                    CurrentX = tempx;
                }
                else
                {
                    string tempy = Regex.Replace(CurrentX, @"(?<![a-zA-Z])x\b", "____TEMPy___");
                    tempy = Regex.Replace(tempy, @"(?<![a-zA-Z])y\b", "x");
                    tempy = Regex.Replace(tempy, @"(?<![a-zA-Z])____TEMPy___\b", "y");
                    CurrentY = tempy;
                }
            }
        }

        private void SyncTri_Click(object sender, RoutedEventArgs e)
        {
            bool? syncType = XToBeSynced();
            if (syncType != null)
            {
                if (syncType == true)
                {
                    string tempx = Regex.Replace(CurrentY, "\\bcos\\(", "____TEMPsin__(");
                    tempx = Regex.Replace(tempx, "\\bsin\\(", "cos(");
                    tempx = Regex.Replace(tempx, "____TEMPsin__\\(", "sin(");
                    CurrentX = tempx;
                }
                else
                {
                    string tempy = Regex.Replace(CurrentX, "\\bcos\\(", "____TEMPsin__(");
                    tempy = Regex.Replace(tempy, "\\bsin\\(", "cos(");
                    tempy = Regex.Replace(tempy, "____TEMPsin__\\(", "sin(");
                    CurrentY = tempy;
                }
            }
        }

        private void Text_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                this.Close();
            }
        }

        private void Button_Position_Click(object sender, RoutedEventArgs e)
        {
            PositionEditor pe = new PositionEditor();
            if (pe.ShowDialog() == true)
            {
                ResultTXT += ", " + pe.SelectedX + ", " + pe.SelectedY;
            }
        }

        private void Button_Vector_Click(object sender, RoutedEventArgs e)
        {
            VectorEditor ve = new VectorEditor();
            if (ve.ShowDialog() == true)
            {
                ResultTXT += ", " + ve.SelectedX + ", " + ve.SelectedY;
            }
        }
    }
}

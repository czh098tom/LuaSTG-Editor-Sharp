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
    /// <summary>
    /// PositionInput.xaml 的交互逻辑
    /// </summary>
    public partial class VectorInput : InputWindow
    {
        /*
        #region routed
        static RoutedUICommand InsTop { get; }
        static RoutedUICommand InsBottom { get; }
        static RoutedUICommand InsLeft { get; }
        static RoutedUICommand InsRight { get; }
        static RoutedUICommand InsSin { get; }
        static RoutedUICommand InsCos { get; }

        static PositionInput()
        {
            InputGestureCollection inputs;
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.G, ModifierKeys.Control, "Ctrl+G")
            };
            InsTop = new RoutedUICommand("Fold Region", "FoldRegion", typeof(EditorRoutedCommands), inputs);
        }
        #endregion
        */

        private TextBox prevFocused = null;
        private int prevFocusedSelBeg = 0;
        private int prevFocusedSelLength = 0;

        private string decomposedX;
        private string decomposedY;

        private StrVector selectedVec;

        private ObservableCollection<StrVector> decomposedVectors = new ObservableCollection<StrVector>();

        static List<string> SeparatePolynomial(string s)
        {
            try
            {
                List<string> vs = new List<string>();
                int lastlocptr = 0;
                char[] c = s.ToCharArray();
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
                    else if (c[i] == '+')
                    {
                        if (expr.Count == 0)
                        {
                            vs.Add(new string(c, lastlocptr, i - lastlocptr));
                            lastlocptr = i + 1;
                        }
                    }
                    else if (c[i] == '-')
                    {
                        if (expr.Count == 0)
                        {
                            vs.Add(new string(c, lastlocptr, i - lastlocptr));
                            lastlocptr = i;
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
                ReCalcDecompostion();
                ReCalcPolynominal();
                RaisePropertyChanged("DecomposedX");
                RaisePropertyChanged("DecomposedY");
            }
        }

        public string DecomposedX
        {
            get => decomposedX;
            set
            {
                decomposedX = value;
                RaisePropertyChanged("DecomposedX");
                ReCalcResult();
                RaisePropertyChanged("ResultTXT");
                ReCalcPolynominal();
            }
        }

        public string DecomposedY
        {
            get => decomposedY;
            set
            {
                decomposedY = value;
                RaisePropertyChanged("DecomposedY");
                ReCalcResult();
                RaisePropertyChanged("ResultTXT");
                ReCalcPolynominal();
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
                    MergeX();
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
                    MergeY();
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
            Result = decomposedX + "," + decomposedY;
        }

        private void MergeX()
        {
            char[] trimType = new char[] { '+', ' ' };
            decomposedX = "";
            int indexlast0s;
            for (indexlast0s = decomposedVectors.Count - 1; indexlast0s >= 0; indexlast0s--)
            {
                string temp = decomposedVectors[indexlast0s].X.Trim(trimType);
                if (!string.IsNullOrEmpty(temp) && temp != "0")
                {
                    break;
                }
            }
            for (int i = 0; i <= indexlast0s; i++) 
            {
                string temp = decomposedVectors[i].X.Trim(trimType);
                if (!string.IsNullOrEmpty(temp))
                {
                    if (temp[0] != '-')
                    {
                        decomposedX += "+" + temp;
                    }
                    else
                    {
                        decomposedX += temp;
                    }
                }
                else
                {
                    decomposedX += "+0";
                }
            }
            decomposedX = decomposedX.Trim(trimType);
        }

        private void MergeY()
        {
            char[] trimType = new char[] { '+', ' ' };
            decomposedY = "";
            int indexlast0s;
            for (indexlast0s = decomposedVectors.Count - 1; indexlast0s >= 0; indexlast0s--)
            {
                string temp = decomposedVectors[indexlast0s].Y.Trim(trimType);
                if (!string.IsNullOrEmpty(temp) && temp != "0")
                {
                    break;
                }
            }
            for (int i = 0; i <= indexlast0s; i++)
            {
                string temp = decomposedVectors[i].Y.Trim(trimType);
                if (!string.IsNullOrEmpty(temp))
                {
                    if (temp[0] != '-')
                    {
                        decomposedY += "+" + temp;
                    }
                    else
                    {
                        decomposedY += temp;
                    }
                }
                else
                {
                    decomposedY += "+0";
                }
            }
            decomposedY = decomposedY.Trim(trimType);
        }

        public VectorInput(string s, MainWindow owner)
        {
            Result = s;
            ReCalcDecompostion();
            ReCalcPolynominal();
            InitializeComponent();
            sumBox.ItemsSource = decomposedVectors;
        }

        private void ReCalcDecompostion()
        {
            List<string> pos = Separate(Result);
            int count = pos.Count;
            if (count > 0) decomposedX = pos[0]; else decomposedX = "";
            if (count > 1) decomposedY = pos[1]; else decomposedY = "";
        }

        private void ReCalcPolynominal()
        {
            decomposedVectors.Clear();
            List<string> xPoly = SeparatePolynomial(decomposedX);
            List<string> yPoly = SeparatePolynomial(decomposedY);
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
                    prevFocus.Text = sbeg + b.Tag.ToString() + send;
                    prevFocus.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
                    prevFocus.Focus();
                    prevFocus.Select(pfb + b.Tag.ToString().Length, 0);
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
                    return prevFocused.Tag.ToString() == "Y";
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
                    string tempx = Regex.Replace(CurrentY, ".x\\b", ".____TEMPy___");
                    tempx = Regex.Replace(tempx, ".y\\b", ".x");
                    tempx = Regex.Replace(tempx, ".____TEMPy___\\b", ".y");
                    CurrentX = tempx;
                }
                else
                {
                    string tempy = Regex.Replace(CurrentX, ".x\\b", ".____TEMPy___");
                    tempy = Regex.Replace(tempy, ".y\\b", ".x");
                    tempy = Regex.Replace(tempy, ".____TEMPy___\\b", ".y");
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

        private void Button_Vector_Click(object sender, RoutedEventArgs e)
        {
            VectorEditor ve = new VectorEditor();
            if (ve.ShowDialog() == true)
            {
                DecomposedX += (ve.OffsetX < 0 || DecomposedX.Trim().Length <= 0
                    ? ve.OffsetX.ToString() : "+" + ve.OffsetX.ToString());
                DecomposedY += (ve.OffsetY < 0 || DecomposedY.Trim().Length <= 0
                    ? ve.OffsetY.ToString() : "+" + ve.OffsetY.ToString());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace LuaSTGEditorSharp.Windows.Input
{
    public class InputWindow : Window, IInputWindow, INotifyPropertyChanged
    {
        protected static List<string> Separate(string s)
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
                    else if (c[i] == ',')
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
            catch(InvalidOperationException)
            {
                return new List<string>() { s };
            }
            catch (NullReferenceException)
            {
                return new List<string>() { };
            }
        }

        protected string result;

        public virtual string Result
        {
            get => result;
            set
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }

        public InputWindow() : base() { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void AppendTitle(string s)
        {
            Title = s + " - " + Title;
        }
    }
}

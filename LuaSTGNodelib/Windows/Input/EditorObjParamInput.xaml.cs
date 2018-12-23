using System;
using System.ComponentModel;
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
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;

namespace LuaSTGEditorSharp.Windows.Input
{
    /// <summary>
    /// PositionInput.xaml 的交互逻辑
    /// </summary>
    public partial class EditorObjParamInput : InputWindow
    {
        private List<string> paramName;

        class ParamItem : INotifyPropertyChanged
        {
            private readonly EditorObjParamInput parent;

            private string name;
            private string value;

            public ParamItem(EditorObjParamInput p) { parent = p; }

            public string Name
            {
                get => name;
                set
                {
                    name = value;
                    RaiseProertyChanged("Name");
                }
            }

            public string Value
            {
                get => value;
                set
                {
                    this.value = value;
                    RaiseProertyChanged("Value");
                }
            }

            public string Value_Invoke
            {
                get => value;
                set
                {
                    this.value = value;
                    RaiseProertyChanged("Value");
                    parent.CombineParams();
                    parent.RaisePropertyChanged("ResultTXT");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void RaiseProertyChanged(string propName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }

        private ObservableCollection<ParamItem> Items { get; set; } = new ObservableCollection<ParamItem>();

        private List<ParamItem> SeparateParam(string s, List<string> paramName)
        {
            try
            {
                List<ParamItem> vs = new List<ParamItem>();
                int lastlocptr = 0;
                int lastparam = 0;
                string par;
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
                            par = "Parameter";
                            if (lastparam < paramName.Count) par = paramName[lastparam];
                            vs.Add(new ParamItem(this) { Name = par, Value = new string(c, lastlocptr, i - lastlocptr) });
                            lastlocptr = i + 1;
                            lastparam++;
                        }
                    }
                }
                par = "Parameter";
                if (lastparam < paramName.Count) par = paramName[lastparam];
                vs.Add(new ParamItem(this) { Name = par, Value = new string(c, lastlocptr, c.Length - lastlocptr) });
                lastparam++;
                for (int i = lastparam; i < paramName.Count; i++)
                {
                    vs.Add(new ParamItem(this) { Name = paramName[i], Value = "" });
                }
                return vs;
            }
            catch (InvalidOperationException)
            {
                return new List<ParamItem>() { new ParamItem(this) { Value = s, Name = "Parameters" } };
            }
        }

        public string ResultTXT
        {
            get => Result;
            set
            {
                Result = value;
                RaisePropertyChanged("ResultTXT");
                DecomposeParams();
            }
        }

        public void DecomposeParams()
        {
            Items.Clear();
            foreach (ParamItem item in SeparateParam(ResultTXT, paramName))
            {
                Items.Add(item);
            }
        }

        public void CombineParams()
        {
            string s = "";
            bool first = true;
            foreach (ParamItem item in Items)
            {
                if (first)
                {
                    s += item.Value;
                    first = false;
                }
                else
                {
                    s += "," + item.Value;
                }
            }
            Result = s;
        }

        public EditorObjParamInput(AttrItem original, MetaType type, string s, MainWindow owner)
        {
            AbstractMetaData metaData = original.Parent.parentWorkSpace.Meta;
            paramName = Separate(metaData.aggregatableMetas[(int)type]
                .FindOfName(original.Parent.attributes[0].AttrInput.Trim('\"'))?.GetParam());
            Result = s;
            DecomposeParams();
            InitializeComponent();
            sumBox.ItemsSource = Items;
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

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox t = sender as TextBox;
            t.Focus();
            t.SelectAll();
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

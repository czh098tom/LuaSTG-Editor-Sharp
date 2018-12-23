using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Toolbox
{
    internal class SearchModel : INotifyPropertyChanged
    {
        private string name;
        private string icon;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Icon
        {
            get => icon;
            set
            {
                icon = value;
                RaisePropertyChanged("Icon");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

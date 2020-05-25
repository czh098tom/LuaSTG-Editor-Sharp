using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace DanmakuRandomizer.Model
{
    internal abstract class Node : INotifyPropertyChanged
    {
        public abstract string Text { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string dependencyPropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(dependencyPropertyName));
        }

        public void SetProperty<T>(ref T target, T value)
        {
            target = value;
            RaisePropertyChanged("Text");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.Windows.Input
{
    public interface IInputWindow
    {
        bool? ShowDialog();
        string Result { get; set; }
        void AppendTitle(string s);
    }
}

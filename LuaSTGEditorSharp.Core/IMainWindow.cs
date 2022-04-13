using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.EditorData;

namespace LuaSTGEditorSharp
{
    public interface IMainWindow
    {
        void Insert(TreeNodeBase t, bool isInvoke = true);
        void Reveal(TreeNodeBase t);
        DocumentData ActivatedWorkSpaceData { get; }
    }
}

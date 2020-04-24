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
        void Insert(TreeNode t, bool isInvoke = true);
        void Reveal(TreeNode t);
        DocumentData ActivatedWorkSpaceData { get; }
    }
}

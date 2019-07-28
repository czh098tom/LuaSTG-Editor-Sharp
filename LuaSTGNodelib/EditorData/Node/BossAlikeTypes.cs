using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public class BossAlikeTypes : ITypeEnumerable
    {
        private static readonly Type[] types =
            { typeof(Boss.BossSCStart), typeof(Boss.BossSCFinish), typeof(Data.Function), typeof(Task.TaskDefine) };

        public IEnumerator<Type> GetEnumerator()
        {
            foreach (Type t in types)
            {
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    public class TaskAlikeTypes : ITypeEnumerable
    {
        private static readonly Type[] types =
            { typeof(Task.TaskNode), typeof(Data.Function), typeof(Task.Tasker) };

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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node
{
    //Task finish (return) uses this.
    public class TaskTypes : ITypeEnumerable
    {
        private static readonly Type[] types =
            { typeof(Task.TaskNode), typeof(Task.TaskDefine), typeof(Task.Tasker) };

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

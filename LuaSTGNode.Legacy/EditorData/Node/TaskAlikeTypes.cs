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
            { typeof(Task.TaskNode), typeof(Task.TaskDefine), typeof(Data.Function), typeof(Task.Tasker),
              typeof(Enemy.CreateSimpleEnemy), // "create simple enemy with task"
              typeof(Boss.BossSCBeforeStart), typeof(Boss.BossSCBeforeFinish), typeof(Boss.BossSCAfter)};

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

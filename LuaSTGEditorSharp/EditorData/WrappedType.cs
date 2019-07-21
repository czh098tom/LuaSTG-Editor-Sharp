using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData
{
    public class WrappedType<T> : ITypeEnumerable
    {
        Type type;

        public WrappedType()
        {
            type = typeof(T);
        }

        public IEnumerator<Type> GetEnumerator()
        {
            yield return type;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

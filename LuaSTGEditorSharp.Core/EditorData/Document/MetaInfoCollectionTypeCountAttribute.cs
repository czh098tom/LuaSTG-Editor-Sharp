using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Document
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MetaInfoCollectionTypeCountAttribute : Attribute
    {
        public readonly int count;
        public MetaInfoCollectionTypeCountAttribute(int count)
        {
            this.count = count;
        }
    }
}

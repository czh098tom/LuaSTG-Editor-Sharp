using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node
{
    /// <summary>
    /// Mysterious Class.
    /// </summary>
    public class AttrItemListHelper<T> : IList<T> where T : GroupableAttrItemHelper, new()
    {
        [JsonIgnore, XmlIgnore]
        private readonly int beginIndex;

        [JsonIgnore, XmlIgnore]
        private readonly int offsetIndex;

        [JsonIgnore, XmlIgnore]
        private readonly string nameFormat;

        [JsonIgnore, XmlIgnore]
        private readonly string editWindow;

        [JsonIgnore, XmlIgnore]
        private readonly TreeNode parent;

        [JsonConstructor]
        public AttrItemListHelper() { }

        public AttrItemListHelper(TreeNode parent, int begin, int offset, string format, string editWindow = "")
        {
            this.parent = parent;
            beginIndex = begin;
            offsetIndex = offset;
            nameFormat = format;
            this.editWindow = editWindow;
        }

        public T this[int index]
        {
            get
            {
                T a = new T();
                a.GetFromList(parent.attributes, beginIndex + index * offsetIndex);
                return a;
            }
            set { }
        }

        public int Count => (parent.attributes.Count - beginIndex) / offsetIndex;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            int i = beginIndex + Count * offsetIndex;
            //parent.DoubleCheckAttr()
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

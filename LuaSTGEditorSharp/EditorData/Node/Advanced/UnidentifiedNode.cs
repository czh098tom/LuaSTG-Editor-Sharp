using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Advanced
{
    [Serializable, NodeIcon("images/16x16/unidentifiednode.png")]
    [IgnoreValidation]
    [CreateInvoke(0)]
    public class UnidentifiedNode : TreeNode
    {
        [JsonConstructor]
        private UnidentifiedNode() : base() { }

        public UnidentifiedNode(DocumentData workSpaceData)
            : base(workSpaceData)
        {
            attributes.Add(new DependencyAttrItem("Type", this, "userDefinedNode"));
        }

        [JsonIgnore, XmlIgnore]
        protected override bool EnableParityCheck => false;

        public override IEnumerable<string> ToLua(int spacing)
        {
            MetaModel target = GetModel();
            if (target != null)
            {
                string head, tail;
                string[,] props = GetProperties(target);
                int n = props.GetLength(0);
                List<object> list = new List<object>();
                for(int i = 0; i < n; i++)
                {
                    if (bool.TryParse(props[i, 1], out bool b) && b) 
                    {
                        list.Add(Lua.StringParser.ParseLua(NonMacrolize(i)));
                    }
                    else
                    {
                        list.Add(Macrolize(i));
                    }
                }
                object[] s = list.ToArray();
                try
                {
                    head = string.Format(target.ExInfo1, s) + "\n";
                    tail = string.Format(target.ExInfo2, s) + "\n";
                }
                catch
                {
                    head = "";
                    tail = "";
                }
                yield return head;
                foreach (var a in base.ToLua(spacing + 1))
                {
                    yield return a;
                }
                yield return tail;
            }
            else
            {
                foreach (var a in base.ToLua(spacing + 1))
                {
                    yield return a;
                }
            }
        }

        public override string ToString()
        {
            MetaModel target = GetModel();
            if (target != null)
            {
                string[,] props = GetProperties(target);
                int n = props.GetLength(0);
                n = n > attributes.Count - 1 ? attributes.Count - 1 : n;
                string s = "";
                bool first = true;
                for (int i = 0; i < n; i++)
                {
                    if (!first) s += ",";
                    s += "\"" + NonMacrolize(i) + "\"";
                    first = false;
                }
                return "Node \"" + NonMacrolize(-1) + "\", parameter (" + s + ")";
            }
            else
            {
                return "* Unknown node *";
            }
        }

        public override void ReflectAttr(DependencyAttrItem relatedAttrItem, DependencyAttributeChangedEventArgs args)
        {
            //if (relatedAttrItem.attrInput != originalvalue)
            {
                while (attributes.Count > 1)
                {
                    attributes.RemoveAt(1);
                }
                MetaModel target = GetModel();
                if (target != null)
                {
                    string[,] props = GetProperties(target);
                    int n = props.GetLength(0);
                    for (int i = 0; i < n; i++)
                    {
                        attributes.Add(new AttrItem(props[i, 0], this, props[i, 2]));
                    }
                }
            }
        }

        public override object Clone()
        {
            var n = new UnidentifiedNode(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        private MetaModel GetModel()
        {
            var metas = parentWorkSpace.Meta.aggregatableMetas[1].GetAllSimpleWithDifficulty("").ToArray();
            return metas.FirstOrDefault((mm) => mm.FullName == attributes[0].AttrInput);
        }

        private string[,] GetProperties(MetaModel source)
        {
            try
            {
                string[] paramStrSplited = source.Param.Split('\n');
                int n = paramStrSplited.Count();
                string[,] props = new string[n / 3, 3];
                //resolve exccess '\n'
                for (int i = 0; i < n - 1; i += 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        props[i / 3, j] = paramStrSplited[i + j];
                    }
                }
                return props;
            }
            catch
            {
                return null;
            }
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            MetaModel target = GetModel();
            yield return new Tuple<int, TreeNode>((target?.ExInfo1.Count((c) => c == '\n') ?? -1) + 1, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>((target?.ExInfo2.Count((c) => c == '\n') ?? -1) + 1, this);
        }
    }
}

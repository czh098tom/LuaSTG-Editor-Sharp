using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.General
{
    [Serializable, NodeIcon("images/16x16/comment.png")]
    [RCInvoke(0)]
    public class Comment : TreeNode
    {
        [JsonConstructor]
        private Comment() : base() { }

        public Comment(DocumentData workSpaceData) 
            : this(workSpaceData, "") { }

        public Comment(DocumentData workSpaceData, string code) 
            : base(workSpaceData)
        {
            /*
            attributes.Add(new AttrItem("Comment", code, this));
            attributes.Add(new AttrItem("Comment on child", "true", this, "bool"));
            */
            CommentContent = code;
            CommentOnChild = "true";
        }

        [JsonIgnore, XmlAttribute("Comment")]
        //[DefaultValue("")]
        public string CommentContent
        {
            get => DoubleCheckAttr(0, name: "Comment").attrInput;
            set => DoubleCheckAttr(0, name: "Comment").attrInput = value;
        }

        [JsonIgnore, XmlAttribute("CommentOnChild")]
        //[DefaultValue("true")]
        public string CommentOnChild
        {
            get => DoubleCheckAttr(1, "bool", "Comment on child").attrInput;
            set => DoubleCheckAttr(1, "bool", "Comment on child").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            bool incomm = false;
            TreeNode t = Parent;
            while (t != null)
            {
                incomm = t is Comment;
                if (incomm)
                {
                    if(t.attributes[1].AttrInput=="true") break;
                }
                t = t.Parent;
            }
            if (!incomm) 
            {
                if (NonMacrolize(1) == "true")
                {
                    yield return sp + "--[[ " + NonMacrolize(0) + "\n";
                    foreach (var a in base.ToLua(spacing + 1))
                    {
                        yield return a;
                    }
                    yield return sp + "]]\n";
                }
                else
                {
                    yield return sp + "--[[ " + NonMacrolize(0) + "]]\n";
                    foreach (var a in base.ToLua(spacing + 1))
                    {
                        yield return a;
                    }
                    yield return sp + "\n";
                }
            }
            else
            {
                yield return sp + NonMacrolize(0) + "\n";
                foreach (var a in base.ToLua(spacing + 1))
                {
                    yield return a;
                }
                yield return sp + "\n";
            }
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            if (NonMacrolize(1) == "true")
            {
                return "[Comment with child] " + attributes[0].AttrInput;
            }
            else
            {
                return "[Comment] " + attributes[0].AttrInput;
            }
        }

        public override object Clone()
        {
            var n = new Comment(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

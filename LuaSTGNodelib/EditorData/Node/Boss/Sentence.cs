using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/sentence.png")]
    [RequireAncestor(typeof(Dialog))]
    [RequireAncestor(typeof(TaskAlikeTypes))]
    [LeafNode]
    [RCInvoke(2)]
    public class Sentence : TreeNode
    {
        [JsonConstructor]
        public Sentence() : base() { }

        public Sentence(DocumentData workSpaceData) 
            : this(workSpaceData, "\"img_void\"", "\"left\"","","", "") { }

        public Sentence(DocumentData workSpaceData, string img, string lr, string txt, string time, string scale) 
            : base(workSpaceData)
        {
            Image = img;
            ImagePosition = lr;
            Text = txt;
            Time = time;
            Scale = scale;
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(0, "image").attrInput;
            set => DoubleCheckAttr(0, "image").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string ImagePosition
        {
            get => DoubleCheckAttr(1, "lrstr", "Image position").attrInput;
            set => DoubleCheckAttr(1, "lrstr", "Image position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Text
        {
            get => DoubleCheckAttr(2, "multilineText").attrInput;
            set => DoubleCheckAttr(2, "multilineText").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(3).attrInput;
            set => DoubleCheckAttr(3).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Scale
        {
            get => DoubleCheckAttr(4, "scale").attrInput;
            set => DoubleCheckAttr(4, "scale").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string attr3 = Macrolize(3);
            attr3 = string.IsNullOrEmpty(attr3) ? "nil" : attr3;
            string attr4 = Macrolize(4);
            attr4 = string.IsNullOrEmpty(attr4) ? "nil,nil" : attr4;
            yield return sp + "boss.dialog.sentence(self," + Macrolize(0) + "," + Macrolize(1) + ",[===[" 
                + NonMacrolize(2) + "]===]," + attr3 + "," + attr4 + ")\n";
        }

        public override string ToString()
        {
            return NonMacrolize(0) + " " + NonMacrolize(1) + " :\n" + NonMacrolize(2) + "\nwait "
                + (string.IsNullOrEmpty(NonMacrolize(3)) ? "default" : NonMacrolize(3)) + " frame(s)";
        }

        public override object Clone()
        {
            var n = new Sentence(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            string s = Macrolize(0);
            int i = 1;
            foreach (char c in s)
            {
                if (c == '\n') i++;
            }
            yield return new Tuple<int, TreeNode>(i, this);
        }
    }
}

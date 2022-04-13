using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Compile;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Advanced
{
    [Serializable, NodeIcon("definemacro.png")]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class DefineMacro : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private DefineMacro() : base() { }

        public DefineMacro(DocumentData workSpaceData) : base(workSpaceData)
        {
            /*
            attributes.Add(new AttrItem("Replace", this));
            attributes.Add(new AttrItem("By", this));
            */
            Replace = "";
            By = "";
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Replace")]
        public string Replace
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("By")]
        public string By
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "-- #define " + NonMacrolize(0) + " " + NonMacrolize(1) + sp + "\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "Macro: Replace " + attributes[0].AttrInput + " by " + attributes[1].AttrInput + " in compile process";
        }

        protected override void AddCompileSettings()
        {
            parentWorkSpace.CompileProcess.marcoDefinition.Add(
                new DefineMarcoSettings() { ToBeReplaced = NonMacrolize(0)
                , New = Macrolize(1) });
        }

        public override object Clone()
        {
            var n = new DefineMacro(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (string.IsNullOrEmpty(NonMacrolize(0)))
                messages.Add(new ArgNotNullMessage(attributes[0].AttrCap, 0, this));
            return messages;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Compile;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Advanced
{
    [Serializable, NodeIcon("images/16x16/archispace.png")]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class ArchiveSpaceIndicator : TreeNode
    {
        [JsonConstructor]
        private ArchiveSpaceIndicator() : base() { }

        public ArchiveSpaceIndicator(DocumentData workSpaceData) : base(workSpaceData)
        {
            Name = "";
        }

        [JsonIgnore, XmlAttribute("Name")]
        //[DefaultValue("")]
        public string Name
        {
            get => DoubleCheckAttr(0, "Name").attrInput;
            set => DoubleCheckAttr(0, "Name").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "-- archive space: " + NonMacrolize(0) + "\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Archive space: " + NonMacrolize(0);
        }

        protected override void AddCompileSettings()
        {
            parentWorkSpace.CompileProcess.archiveSpace = NonMacrolize(0);
        }

        public override object Clone()
        {
            var n = new ArchiveSpaceIndicator(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (string.IsNullOrEmpty(NonMacrolize(0)))
                messages.Add(new ArgNotNullMessage(attributes[0].AttrCap, 0, this));
            string s = NonMacrolize(0);
            if (s != "")
            {
                if (!s.EndsWith("\\") && !s.EndsWith("/"))
                {
                    messages.Add(new InvalidArchiveSpaceMessage(this));
                }
                else
                {
                    char[] cs = Path.GetInvalidPathChars();
                    foreach (char c in cs)
                    {
                        if (s.Contains(c))
                        {
                            messages.Add(new InvalidArchiveSpaceMessage(this));
                            break;
                        }
                    }
                }
            }
            return messages;
        }
    }
}

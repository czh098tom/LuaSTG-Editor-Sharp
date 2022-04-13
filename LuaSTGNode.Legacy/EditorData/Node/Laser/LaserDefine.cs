using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Laser
{
    [Serializable, NodeIcon("laserdefine.png")]
    [ClassNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class LaserDefine : DefinitionWithDifficulty
    {
        [JsonConstructor]
        private LaserDefine() : base() { }

        public LaserDefine(DocumentData workSpaceData)
            : this(workSpaceData, "", "All") { }

        public LaserDefine(DocumentData workSpaceData, string name, string difficulty)
            : base(workSpaceData)
        {
            Name = name;
            Difficulty = difficulty;
            /*
            attributes.Add(new AttrItem("Name", name, this));
            attributes.Add(new AttrItem("Difficulty", difficulty, this, "objDifficulty"));
            */
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "_editor_class[\"" + GetParsedNameWithDifficulty() + "\"]=Class(laser)\n";
            foreach (var a in base.ToLua(spacing))
            {
                yield return a;
            }
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
            foreach (Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override string ToString()
        {
            return "Define laser type \"" + GetNameWithDifficulty() + "\"";
        }

        public override object Clone()
        {
            var n = new LaserDefine(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override MetaInfo GetMeta()
        {
            return new LaserDefineMetaInfo(this);
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

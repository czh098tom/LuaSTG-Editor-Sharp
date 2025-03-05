using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Audio
{
    [Serializable, NodeIcon("playbgm.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(1)]
    public class PlayBGM : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private PlayBGM() : base() { }

        public PlayBGM(DocumentData workSpaceData)
            : this(workSpaceData, "\"\"", "", "false") { }

        public PlayBGM(DocumentData workSpaceData, string name, string time, string setstime)
            : base(workSpaceData)
        {
            /*
            attributes.Add(new AttrItem("Name", name, this, "BGM"));
            attributes.Add(new AttrItem("Time", time, this));
            attributes.Add(new AttrItem("Set stage time", setstime, this, "bool"));
            */
            Name = name;
            Time = time;
            SetStageTime = setstime;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0, "BGM").attrInput;
            set => DoubleCheckAttr(0, "BGM").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Time
        {
            get => DoubleCheckAttr(1).attrInput;
            set => DoubleCheckAttr(1).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string SetStageTime
        {
            get => DoubleCheckAttr(2, "bool", "Set stage time").attrInput;
            set => DoubleCheckAttr(2, "bool", "Set stage time").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s = "";
            if (string.IsNullOrEmpty(Macrolize(1)))
            {
                yield return sp + "LoadMusicRecord(" + Macrolize(0) + ")\n" + sp + "_play_music(" + Macrolize(0) + ")\n";
            }
            else
            {
                if (!string.IsNullOrEmpty(Macrolize(2)))
                {
                    s = sp + "if " + Macrolize(2) + " then ex.stageframe=int(" + Macrolize(1) + "*60) end\n";
                }
                yield return sp + "LoadMusicRecord(" + Macrolize(0) + ")\n"
                    + sp + "_play_music(" + Macrolize(0) + "," + Macrolize(1) + ")\n" + s;
            }
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            if ((!string.IsNullOrEmpty(Macrolize(1))) && (!string.IsNullOrEmpty(Macrolize(2))))
            {
                yield return new Tuple<int, TreeNodeBase>(3, this);
            }
            else
            {
                yield return new Tuple<int, TreeNodeBase>(2, this);
            }
        }

        public override string ToString()
        {
            string s = "";
            if (!string.IsNullOrEmpty(NonMacrolize(1)))
            {
                s = ", start from " + NonMacrolize(1) + " second(s)";
            }
            return "Play background music " + NonMacrolize(0) + s;
        }

        public override object Clone()
        {
            var n = new PlayBGM(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

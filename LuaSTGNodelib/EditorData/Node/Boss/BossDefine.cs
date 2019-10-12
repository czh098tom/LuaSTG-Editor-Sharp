using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bossdefine.png")]
    [ClassNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class BossDefine : TreeNode
    {
        [JsonConstructor]
        private BossDefine() : base() { }

        public BossDefine(DocumentData workSpaceData)
            : this(workSpaceData, "", "All", "Name", "", "\"\"") { }

        public BossDefine(DocumentData workSpaceData, string name, string difficulty, string display, string bg, string bgm)
            : base(workSpaceData)
        {
            Name = name;
            Difficulty = difficulty;
            DisplayedName = display;
            Background = bg;
            BackgroundMusic = bgm;
            /*
            attributes.Add(new AttrItem("Name", name, this));
            attributes.Add(new AttrItem("Difficulty", difficulty, this, "objDifficulty"));
            attributes.Add(new AttrItem("Displayed name", display, this));
            //attributes.Add(new AttrItem("Position", pos, this, "position"));
            //attributes.Add(new AttrItem("Spell Card Background", scbg, this, "bossBG"));
            //attributes.Add(new AttrItem("Boss image", img, this, "imageFile"));
            //attributes.Add(new AttrItem("Image Column", imgCol, this));
            //attributes.Add(new AttrItem("Image Row", imgRow, this));
            //attributes.Add(new AttrItem("Collision Size", colli, this, "size"));
            //attributes.Add(new AttrItem("Animation interval", intv, this));
            attributes.Add(new AttrItem("Background", bg, this, "BG"));
            attributes.Add(new AttrItem("Background Music", bgm, this, "BGM"));
            //attributes.Add(new AttrItem("Num of images", imgColNum, this));
            //attributes.Add(new AttrItem("Num of animations", imgRowNum, this));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Difficulty
        {
            get => DoubleCheckAttr(1, "objDifficulty").attrInput;
            set => DoubleCheckAttr(1, "objDifficulty").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string DisplayedName
        {
            get => DoubleCheckAttr(2, name: "Displayed name").attrInput;
            set => DoubleCheckAttr(2, name: "Displayed name").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Background
        {
            get => DoubleCheckAttr(3, "BG").attrInput;
            set => DoubleCheckAttr(3, "BG").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string BackgroundMusic
        {
            get => DoubleCheckAttr(4, "BGM", "Background Music").attrInput;
            set => DoubleCheckAttr(4, "BGM", "Background Music").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string difficultyS = (NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1));
            string fullName = "\"" + Lua.StringParser.ParseLua(NonMacrolize(0) + difficultyS) + "\"";
            string displayedName = Lua.StringParser.ParseLua(NonMacrolize(2));
            string difficultyString = Lua.StringParser.ParseLua(NonMacrolize(1));
            string bg = Macrolize(3);
            bg = string.IsNullOrEmpty(bg) ? "nil" : bg;
            yield return "_editor_class[" + fullName + "]=Class(boss)\n"
                + "_editor_class[" + fullName + "].cards={}\n"
                + "_editor_class[" + fullName + "].name=\"" + displayedName + "\"\n"
                + "_editor_class[" + fullName + "].bgm=" + Macrolize(4) + "\n"
                + "_editor_class[" + fullName + "]._bg=" + bg + "\n"
                + "_editor_class[" + fullName + "].difficulty=\"" + difficultyString + "\"\n";
            foreach (var a in base.ToLua(spacing))
            {
                yield return a;
            }
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(6, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override string ToString()
        {
            string difficultyS = NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1);
            return "Define boss \"" + NonMacrolize(0) + difficultyS + "\"";
        }

        public override object Clone()
        {
            var n = new BossDefine(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override string GetDifficulty()
        {
            return NonMacrolize(1) == "All" ? "" : NonMacrolize(1);
        }

        public override MetaInfo GetMeta()
        {
            return new BossDefineMetaInfo(this);
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

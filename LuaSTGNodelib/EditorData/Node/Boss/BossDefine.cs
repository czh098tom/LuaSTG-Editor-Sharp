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
            : this(workSpaceData, "", "All", "Name", "240,384", "", "", "4", "3", "16,16", "6", "", "\"\"", "4,4,4", "1,1") { }

        public BossDefine(DocumentData workSpaceData, string name, string difficulty, string display, string pos, string scbg
            , string img, string imgCol, string imgRow, string colli, string intv, string bg, string bgm, string imgColNum
            , string imgRowNum)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Name", name, this));
            attributes.Add(new AttrItem("Difficulty", difficulty, this, "objDifficulty"));
            attributes.Add(new AttrItem("Displayed name", display, this));
            attributes.Add(new AttrItem("Position", pos, this, "position"));
            attributes.Add(new AttrItem("Spell Card Background", scbg, this, "bossBG"));
            attributes.Add(new AttrItem("Boss image", img, this, "imageFile"));
            attributes.Add(new AttrItem("Image Column", imgCol, this));
            attributes.Add(new AttrItem("Image Row", imgRow, this));
            attributes.Add(new AttrItem("Collision Size", colli, this, "size"));
            attributes.Add(new AttrItem("Animation interval", intv, this));
            attributes.Add(new AttrItem("Background", bg, this, "BG"));
            attributes.Add(new AttrItem("Background Music", bgm, this, "BGM"));
            attributes.Add(new AttrItem("Num of images", imgColNum, this));
            attributes.Add(new AttrItem("Num of animations", imgRowNum, this));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string difficultyS = (NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1));
            string fullName = "\"" + Lua.StringParser.ParseLua(NonMacrolize(0) + difficultyS) + "\"";
            string displayedName = Lua.StringParser.ParseLua(NonMacrolize(2));
            string difficultyString = Lua.StringParser.ParseLua(NonMacrolize(1));
            string bg = Macrolize(10);
            bg = string.IsNullOrEmpty(bg) ? "nil" : bg;
            string scbg = Macrolize(4);
            scbg = string.IsNullOrEmpty(scbg) || scbg == "\"\"" ? "spellcard_background" : "_editor_class[" + scbg + "]";
            if(string.IsNullOrEmpty(Macrolize(5)))
            {
                yield return "_editor_class[" + fullName + "]=Class(boss)\n"
                           + "_editor_class[" + fullName + "].cards={}\n"
                           + "_editor_class[" + fullName + "].name=\"" + displayedName + "\"\n"
                           + "_editor_class[" + fullName + "].init=function(self,cards)\n"
                           + "    boss.init(self," + Macrolize(3) + ",\"" + displayedName + "\",cards,New(" + scbg + "),\"" + difficultyString + "\")\n"
                           + "end\n"
                           + "_editor_class[" + fullName + "].bgm=" + Macrolize(11) + "\n"
                           + "_editor_class[" + fullName + "]._bg=" + bg + "\n"
                           + "_editor_class[" + fullName + "].difficulty=\"" + difficultyString + "\"\n";
                foreach (var a in base.ToLua(spacing))
                {
                    yield return a;
                }
            }
            else
            {
                string filename = "\"" + Lua.StringParser.ParseLua(Path.GetFileName(NonMacrolize(5))) + "\"";
                yield return "_editor_class[" + fullName + "]=Class(boss)\n"
                           + "_editor_class[" + fullName + "].cards={}\n"
                           + "_editor_class[" + fullName + "].name=\"" + displayedName + "\"\n"
                           + "_editor_class[" + fullName + "].init=function(self,cards)\n"
                           + "    boss.init(self," + Macrolize(3) + ",\"" + displayedName + "\",cards,New(" + scbg + "),\"" + difficultyString + "\")\n"
                           + "    self._wisys:SetImage(" + filename + "," + Macrolize(7) + "," + Macrolize(6) 
                           + ",{" + Macrolize(12) + "},{" + Macrolize(13) + "}," + Macrolize(9) + "," + Macrolize(8) + ")\n"
                           + "end\n"
                           + "_editor_class[" + fullName + "].bgm=" + Macrolize(11) + "\n"
                           + "_editor_class[" + fullName + "]._bg=" + bg + "\n"
                           + "_editor_class[" + fullName + "].difficulty=\"" + difficultyString + "\"\n";
                /*
                yield return "LoadTexture(\'anonymous:\'.." + filename + "," + filename + ")\n"
                           + "bossimg_number_n={" + Macrolize(12) + "}\n"
                           + "bosstexture_n,bosstexture_m=GetTextureSize('anonymous:'.." + filename + ")\n"
                           + "bossimg_w,bossimg_h=bosstexture_n/" + Macrolize(6) + ",bosstexture_m/" + Macrolize(7) + "\n"
                           + "for i = 1," + Macrolize(7) + " do\n"
                           + "	LoadImageGroup('anonymous:'.." + filename + "..i,'anonymous:'.." + filename
                           + ",0,  bossimg_h*(i-1),bossimg_w,bossimg_h,bossimg_number_n[i],1," + Macrolize(8) + ")\n"
                           + "end\n"
                           + "_editor_class[" + fullName + "]=Class(boss)\n"
                           + "_editor_class[" + fullName + "].cards={}\n"
                           + "_editor_class[" + fullName + "].name=\"" + displayedName + "\"\n"
                           + "_editor_class[" + fullName + "].init=function(self,cards)\n"
                           + "    boss.init(self," + Macrolize(3) + ",\"" + displayedName + "\",cards,New(" + scbg + "),\"" + difficultyString + "\")\n"
                           + "	   self.ani_intv=" + Macrolize(9) + "\n"
                           + "	   for i=1," + Macrolize(7) + " do self['img'..i]={} end\n"
                           + "	   self.nn={" + Macrolize(12) + "}\n"
                           + "    self.mm={" + Macrolize(13) + "}\n"
                           + "    for i=2," + Macrolize(7) + " do self['ani'..i]=self.nn[i]-self.mm[i-1] end\n"
                           + "    for i=1," + Macrolize(7) + " do\n"
                           + "        for j=1,self.nn[i] do\n"
                           + "            self['img'..i][j]='anonymous:'.." + filename + "..i..j\n"
                           + "        end\n"
                           + "    end\n"
                           + "end\n"
                           + "_editor_class[" + fullName + "].bgm=" + Macrolize(11) + "\n"
                           + "_editor_class[" + fullName + "]._bg=" + bg + "\n"
                           + "_editor_class[" + fullName + "].difficulty=\"" + difficultyString + "\"\n";
                           */
                foreach (var a in base.ToLua(spacing))
                {
                    yield return a;
                }
            }
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            if (string.IsNullOrEmpty(Macrolize(5)))
            {
                yield return new Tuple<int, TreeNode>(9, this);
                foreach (Tuple<int, TreeNode> t in GetChildLines())
                {
                    yield return t;
                }
            }
            else
            {
                yield return new Tuple<int, TreeNode>(10, this);
                foreach (Tuple<int, TreeNode> t in GetChildLines())
                {
                    yield return t;
                }
            }
        }

        public override string ToString()
        {
            string difficultyS = NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1);
            return "Define boss \"" + NonMacrolize(0) + difficultyS + "\"";
        }

        protected override void AddCompileSettings()
        {
            if (!string.IsNullOrEmpty(NonMacrolize(5)) && !parentWorkSpace.CompileProcess.resourceFilePath.Contains(NonMacrolize(5)))
                parentWorkSpace.CompileProcess.resourceFilePath.Add(attributes[5].AttrInput);
        }

        public override object Clone()
        {
            var n = new BossDefine(parentWorkSpace)
            {
                attributes = new ObservableCollection<AttrItem>(from AttrItem a in attributes select (AttrItem)a.Clone()),
                Children = new ObservableCollection<TreeNode>(from TreeNode t in Children select (TreeNode)t.Clone()),
                _parent = _parent,
                isExpanded = isExpanded
            };
            n.FixAttrParent();
            n.FixChildrenParent();
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

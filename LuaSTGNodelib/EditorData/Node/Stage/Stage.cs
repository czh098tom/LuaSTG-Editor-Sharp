using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Stage
{
    [Serializable, NodeIcon("stage.png")]
    [RequireParent(typeof(StageGroup))]
    [RCInvoke(0)]
    public class Stage : TreeNode
    {
        [JsonConstructor]
        private Stage() : base() { }

        public Stage(DocumentData workSpaceData)
            : this(workSpaceData, "Spell Card") { }

        public Stage(DocumentData workSpaceData, string name)
            : base(workSpaceData)
        {
            Name = name;
            StartLifePractice = "8";
            StartPowerPractice = "400";
            StartFaithPractice = "50000";
            StartBombPractice = "8";
            AllowPractice = "true";
            /*
            attributes.Add(new AttrItem("Name", name, this));
            attributes.Add(new AttrItem("Start life (practice)", "7", this));
            attributes.Add(new AttrItem("Start power (practice)", "400", this));
            attributes.Add(new AttrItem("Start faith (practice)", "50000", this));
            attributes.Add(new AttrItem("Start bomb (practice)", "3", this));
            attributes.Add(new AttrItem("Allow practice", "true", this, "bool"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string StartLifePractice
        {
            get => DoubleCheckAttr(1, name: "Start life (practice)").attrInput;
            set => DoubleCheckAttr(1, name: "Start life (practice)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string StartPowerPractice
        {
            get => DoubleCheckAttr(2, name: "Start power (practice)").attrInput;
            set => DoubleCheckAttr(2, name: "Start power (practice)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string StartFaithPractice
        {
            get => DoubleCheckAttr(3, name: "Start faith (practice)").attrInput;
            set => DoubleCheckAttr(3, name: "Start faith (practice)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string StartBombPractice
        {
            get => DoubleCheckAttr(4, name: "Start bomb (practice)").attrInput;
            set => DoubleCheckAttr(4, name: "Start bomb (practice)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string AllowPractice
        {
            get => DoubleCheckAttr(5, "bool", "Allow practice").attrInput;
            set => DoubleCheckAttr(5, "bool", "Allow practice").attrInput = value;
        }

        public override string ToString()
        {
            return "Stage \"" + attributes[0].AttrInput + "\"";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            TreeNode Parent = GetLogicalParent();
            string parentStageGroupName = "";
            if (Parent?.attributes != null && Parent.AttributeCount >= 1)
            {
                parentStageGroupName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0));
            }
            string stageName = Lua.StringParser.ParseLua(NonMacrolize(0));
            yield return sp + "stage.group.AddStage(\'" + parentStageGroupName + "\',\'" 
                       + stageName
                       + "@" + parentStageGroupName + "\',{lifeleft=" + Macrolize(1) + ",power="
                       + Macrolize(2) + ",faith=" + Macrolize(3) + ",bomb=" + Macrolize(4) + "},"
                       + Macrolize(5) + ")\n"
                       + sp + "stage.group.DefStageFunc(\'" + stageName + "@" + parentStageGroupName
                       + "\',\'init\',function(self)\n"
                       + sp + s1 + "_init_item(self)\n"
                       + sp + s1 + "difficulty=self.group.difficulty\n"
                       + sp + s1 + "New(mask_fader,'open')\n"
                       + sp + s1 + "if jstg then jstg.CreatePlayers() else New(_G[lstg.var.player_name]) end\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + s1 + "task.New(self,function()\n"
                       + sp + s1 + s1 + "while coroutine.status(self.task[1])~=\'dead\' do task.Wait() end\n"
                       + sp + s1 + s1 + "stage.group.FinishReplay()\n"
                       + sp + s1 + s1 + "New(mask_fader,\'close\')\n"
                       + sp + s1 + s1 + "task.New(self,function()\n"
                       + sp + s1 + s1 + s1 + "local _,bgm=EnumRes(\'bgm\')\n"
                       + sp + s1 + s1 + s1 + "for i=1,30 do\n"
                       + sp + s1 + s1 + s1 + s1 + "for _,v in pairs(bgm) do\n"
                       + sp + s1 + s1 + s1 + s1 + s1 + "if GetMusicState(v)=='playing' then\n"
                       + sp + s1 + s1 + s1 + s1 + s1 + s1 + "SetBGMVolume(v,1-i/30)\n"
                       + sp + s1 + s1 + s1 + s1 + s1 + "end\n"
                       + sp + s1 + s1 + s1 + s1 + "end\n"
                       + sp + s1 + s1 + s1 + s1 + "task.Wait()\n"
                       + sp + s1 + s1 + s1 + "end\n"
                       + sp + s1 + s1 + "end)\n"
                       + sp + s1 + s1 + "task.Wait(30)\n"
                       + sp + s1 + s1 + "stage.group.FinishStage()\n"
                       + sp + s1 + "end)\n"
                       + sp + "end)\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(6, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(19, this);
        }

        public override object Clone()
        {
            var n = new Stage(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (string.IsNullOrEmpty(NonMacrolize(0)))
                messages.Add(new ArgNotNullMessage(attributes[0].AttrCap, 0, this));
            TreeNode p = GetLogicalParent();
            if (p?.attributes == null || p.AttributeCount < 1)
            {
                messages.Add(new CannotFindAttributeInParent(1, this));
            }
            return messages;
        }
    }
}

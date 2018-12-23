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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/stage.png")]
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
            attributes.Add(new AttrItem("Name", name, this));
            attributes.Add(new AttrItem("Start life (practice)", "7", this));
            attributes.Add(new AttrItem("Start power (practice)", "400", this));
            attributes.Add(new AttrItem("Start faith (practice)", "50000", this));
            attributes.Add(new AttrItem("Start bomb (practice)", "3", this));
            attributes.Add(new AttrItem("Allow practice", "true", this, "bool"));
        }

        public override string ToString()
        {
            return "Stage \"" + attributes[0].AttrInput + "\"";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(4);
            string parentStageGroupName = Lua.StringParser.ParseLua(NonMacrolize(Parent.attributes[0]));
            string stageName = Lua.StringParser.ParseLua(NonMacrolize(0));
            yield return "stage.group.AddStage(\'" + parentStageGroupName + "\',\'" 
                       + stageName
                       + "@" + parentStageGroupName + "\',{lifeleft=" + Macrolize(1) + ",power="
                       + Macrolize(2) + ",faith=" + Macrolize(3) + ",bomb=" + Macrolize(4) + "},"
                       + Macrolize(5) + ")\n"
                       + "stage.group.DefStageFunc(\'" + stageName + "@" + parentStageGroupName
                       + "\',\'init\',function(self)\n"
                       + sp + "_init_item(self)\n"
                       + sp + "difficulty=self.group.difficulty\n"
                       + sp + "New(mask_fader,'open')\n"
                       + sp + "jstg.CreatePlayers()\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "task.New(self,function()\n"
                       + sp + sp + "while coroutine.status(self.task[1])~=\'dead\' do task.Wait() end\n"
                       + sp + sp + "stage.group.FinishReplay()\n"
                       + sp + sp + "New(mask_fader,\'close\')\n"
                       + sp + sp + "task.New(self,function()\n"
                       + sp + sp + sp + "local _,bgm=EnumRes(\'bgm\')\n"
                       + sp + sp + sp + "for i=1,30 do\n"
                       + sp + sp + sp + sp + "for _,v in pairs(bgm) do\n"
                       + sp + sp + sp + sp + sp + "if GetMusicState(v)=='playing' then\n"
                       + sp + sp + sp + sp + sp + sp + "SetBGMVolume(v,1-i/30)\n"
                       + sp + sp + sp + sp + sp + "end\n"
                       + sp + sp + sp + sp + "end\n"
                       + sp + sp + sp + sp + "task.Wait()\n"
                       + sp + sp + sp + "end\n"
                       + sp + sp + "end)\n"
                       + sp + sp + "task.Wait(30)\n"
                       + sp + sp + "stage.group.FinishStage()\n"
                       + sp + "end)\n"
                       + "end)\n";
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
            var n = new Stage(parentWorkSpace)
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

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (string.IsNullOrEmpty(NonMacrolize(0)))
                messages.Add(new ArgNotNullMessage(attributes[0].AttrCap, 0, this));
            return messages;
        }
    }
}

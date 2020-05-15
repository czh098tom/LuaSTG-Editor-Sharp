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
    [Serializable, NodeIcon("stagegoto.png")]
    [RequireAncestor(typeof(Stage))]
    [LeafNode]
    [RCInvoke(0)]
    class StageGoto : TreeNode
    {
        [JsonConstructor]
        private StageGoto() : base() { }

        public StageGoto(DocumentData workSpaceData)
            : this(workSpaceData, "1") { }

        public StageGoto(DocumentData workSpaceData, string index)
            : base(workSpaceData)
        {
            StageIndex = index;
        }

        [JsonIgnore, NodeAttribute]
        public string StageIndex
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override string ToString()
        {
            return "Go to stage " + NonMacrolize(0);
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            yield return
              sp + "New(mask_fader,'close')" + "\n"
            + sp + "task.New(self, function()" + "\n"
            + sp + s1 + "local _, bgm = EnumRes('bgm')" + "\n"
            + sp + s1 + "for i = 1, 30 do" + "\n"
            + sp + s1 + s1 + "for _, v in pairs(bgm) do" + "\n"
            + sp + s1 + s1 + s1 + "if GetMusicState(v) == 'playing' then" + "\n"
            + sp + s1 + s1 + s1 + s1 + "SetBGMVolume(v, 1 - i / 30)" + "\n"
            + sp + s1 + s1 + s1 + "end" + "\n"
            + sp + s1 + s1 + s1 + "task.Wait()" + "\n"
            + sp + s1 + s1 + "end" + "\n"
            + sp + s1 + "end" + "\n"
            + sp + "end)" + "\n"
            + sp + "task.Wait(30) stage.group.GoToStage(" + Macrolize(0) + ")" + "\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(13, this);
        }

        public override object Clone()
        {
            var n = new StageGoto(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}


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
    [Serializable, NodeIcon("stagefinishgroup.png")]
    [RequireAncestor(typeof(Stage))]
    [LeafNode]
    class StageGroupFinish : TreeNode
    {
        [JsonConstructor]
        private StageGroupFinish() : base() { }

        public StageGroupFinish(DocumentData workSpaceData)
            : base(workSpaceData) { }

        public override string ToString()
        {
            return "Finish current stage group";
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return
                  sp + "New(mask_fader,'close')" + "\n"
                + sp + "_stop_music()" + "\n"
                + sp + "task.Wait(30)" + "\n"
                + sp + "stage.group.FinishGroup()" + "\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(4, this);
        }

        public override object Clone()
        {
            var n = new StageGroupFinish(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("bginit.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(BossBGLayer)), Uniqueness]
    public class BossBGLayerInit : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private BossBGLayerInit() : base() { }

        public BossBGLayerInit(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s1 = Indent(1);
            yield return sp + "function(self)\n" 
                + sp + s1 + "self.task={}\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end,\n";
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(2, this);
            foreach (Tuple<int, TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "on init";
        }

        public override object Clone()
        {
            var n = new BossBGLayerInit(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

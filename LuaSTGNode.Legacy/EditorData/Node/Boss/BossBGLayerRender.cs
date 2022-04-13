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
    [Serializable, NodeIcon("bgrender.png")]
    [CannotDelete, CannotBan]
    [RequireParent(typeof(BossBGLayer)), Uniqueness]
    public class BossBGLayerRender : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private BossBGLayerRender() : base() { }

        public BossBGLayerRender(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "function(self)\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return sp + "end\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
            foreach(Tuple<int,TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNodeBase>(1, this);
        }

        public override string ToString()
        {
            return "on render";
        }

        public override object Clone()
        {
            var n = new BossBGLayerRender(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

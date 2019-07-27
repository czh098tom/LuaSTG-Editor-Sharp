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
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bglayer.png")]
    [RequireParent(typeof(BossBGDefine))]
    [CreateInvoke(0), RCInvoke(0)]
    public class BossBGLayer : TreeNode
    {
        [JsonConstructor]
        private BossBGLayer() : base() { }

        public BossBGLayer(DocumentData workSpaceData)
            : this(workSpaceData, "", "false","0,0","0","0,0","0","\"\"","1,1") { }

        public BossBGLayer(DocumentData workSpaceData, string img, string tile, string pos, string rot, string v
            , string omega, string blend, string scale)
            : base(workSpaceData)
        {
            attributes.Add(new AttrItem("Image", img, this, "image"));
            attributes.Add(new AttrItem("Is tile", tile, this, "bool"));
            attributes.Add(new AttrItem("Position", pos, this, "position"));
            attributes.Add(new AttrItem("Rotation", rot, this, "rotation"));
            attributes.Add(new AttrItem("Velocity(x,y)", v, this, "velocityPos"));
            attributes.Add(new AttrItem("Omiga", omega, this, "omega"));
            attributes.Add(new AttrItem("Blend mode", blend, this, "blend"));
            attributes.Add(new AttrItem("scale", scale, this));
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            string s = sp + "_spellcard_background.AddLayer(self,";
            foreach(AttrItem item in attributes)
            {
                s += Macrolize(item) + ",";
            }
            yield return s + "\n";
            foreach (var a in base.ToLua(spacing))
            {
                yield return a;
            }
            yield return sp + ")\n";
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach (Tuple<int, TreeNode> t in GetChildLines())
            {
                yield return t;
            }
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            string blend = Lua.StringParser.ParseLua(NonMacrolize(6));
            blend = blend == "\"\"" || blend == "\"mul+alpha\"" || string.IsNullOrEmpty(blend)
                ? "" : ", blend mode: " + blend;
            return "Layer " + NonMacrolize(0) + blend;
        }

        public override object Clone()
        {
            var n = new BossBGLayer(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

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
            Image = img;
            IsTile = tile;
            Position = pos;
            Rotation = rot;
            Velocity = v;
            Omega = omega;
            Blend = blend;
            Scale = scale;
            /*
            attributes.Add(new AttrItem("Image", img, this, "image"));
            attributes.Add(new AttrItem("Is tile", tile, this, "bool"));
            attributes.Add(new AttrItem("Position", pos, this, "position"));
            attributes.Add(new AttrItem("Rotation", rot, this, "rotation"));
            attributes.Add(new AttrItem("Velocity(x,y)", v, this, "velocityPos"));
            attributes.Add(new AttrItem("Omiga", omega, this, "omega"));
            attributes.Add(new AttrItem("Blend mode", blend, this, "blend"));
            attributes.Add(new AttrItem("scale", scale, this));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Image
        {
            get => DoubleCheckAttr(0, "image").attrInput;
            set => DoubleCheckAttr(0, "image").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string IsTile
        {
            get => DoubleCheckAttr(1, "bool", "Is tile").attrInput;
            set => DoubleCheckAttr(1, "bool", "Is tile").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(2, "position").attrInput;
            set => DoubleCheckAttr(2, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Rotation
        {
            get => DoubleCheckAttr(3, "rotation").attrInput;
            set => DoubleCheckAttr(3, "rotation").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Velocity
        {
            get => DoubleCheckAttr(4, "velocityPos", "Velocity(x,y)").attrInput;
            set => DoubleCheckAttr(4, "velocityPos", "Velocity(x,y)").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Omega
        {
            get => DoubleCheckAttr(5, "omega", "Omiga").attrInput;
            set => DoubleCheckAttr(5, "omega", "Omiga").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Blend
        {
            get => DoubleCheckAttr(6, "blend", "Blend mode").attrInput;
            set => DoubleCheckAttr(6, "blend", "Blend mode").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Scale
        {
            get => DoubleCheckAttr(7).attrInput;
            set => DoubleCheckAttr(7).attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string s = sp + "_spellcard_background.AddLayer(self,";
            foreach(AttrItem item in attributes)
            {
                s += Macrolize(item) + ",";
            }
            yield return s + "\n";
            foreach (var a in base.ToLua(spacing + 1))
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

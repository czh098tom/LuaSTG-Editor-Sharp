using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Render
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/onrender.png")]
    [RequireParent(typeof(Bullet.BulletDefine), typeof(Laser.BentLaserDefine), typeof(Object.ObjectDefine))]
    public class OnRender : TreeNode, ICallBackFunc
    {
        [JsonConstructor]
        public OnRender() : base() { }

        public OnRender(DocumentData workSpaceData) : base(workSpaceData) { }

        public override IEnumerable<string> ToLua(int spacing)
        {
            TreeNode Parent = this.Parent;
            string parentName = "";
            if (Parent?.attributes != null && Parent.AttributeCount >= 2)
            {
                parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0) +
                   (Parent.NonMacrolize(1) == "All" ? "" : ":" + Parent.NonMacrolize(1)));
            }
            yield return "_editor_class[\"" + parentName + "\"].render=function(self)\n";
            foreach (var a in base.ToLua(spacing + 1))
            {
                yield return a;
            }
            yield return "end\n";
        }

        public override string ToString()
        {
            return "on render()";
        }

        public override object Clone()
        {
            var n = new OnRender(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        [JsonIgnore]
        public string FuncName
        {
            get => "render";
        }
    }
}

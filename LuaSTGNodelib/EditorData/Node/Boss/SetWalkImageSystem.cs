using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Message;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("/LuaSTGNodeLib;component/images/16x16/bosswalkimg.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [LeafNode]
    [CreateInvoke(0)]
    public class SetBossWalkImageSystem : TreeNode
    {
        [JsonConstructor]
        private SetBossWalkImageSystem() : base() { }

        public SetBossWalkImageSystem(DocumentData workSpaceData)
            : this(workSpaceData, "", "4,3", "4,4,4", "1,1", "6", "16,16") { }

        public SetBossWalkImageSystem(DocumentData workSpaceData, string imgpath, string colrow,
            string noi, string noa, string animintv, string collisize) : base(workSpaceData)
        {
            ImagePath = imgpath;
            ColRow = colrow;
            NumOfImages = noi;
            NumOfAnims = noa;
            AnimInterval = animintv;
            CollisionSize = collisize;
        }

        [JsonIgnore]
        public string ImagePath
        {
            get => DoubleCheckAttr(0, "imageFile", "Image path").attrInput;
            set => DoubleCheckAttr(0, "imageFile", "Image path").attrInput = value;
        }

        [JsonIgnore]
        public string ColRow
        {
            get => DoubleCheckAttr(1, "colrow", "Cols and rows").attrInput;
            set => DoubleCheckAttr(1, "colrow", "Cols and rows").attrInput = value;
        }

        [JsonIgnore]
        public string NumOfImages
        {
            get => DoubleCheckAttr(2, "numofimg", "Number of images").attrInput;
            set => DoubleCheckAttr(2, "numofimg", "Number of images").attrInput = value;
        }

        [JsonIgnore]
        public string NumOfAnims
        {
            get => DoubleCheckAttr(3, "numofani", "Number of repetend").attrInput;
            set => DoubleCheckAttr(3, "numofani", "Number of repetend").attrInput = value;
        }

        [JsonIgnore]
        public string AnimInterval
        {
            get => DoubleCheckAttr(4, "animinterval", "Animation interval").attrInput;
            set => DoubleCheckAttr(4, "animinterval", "Animation interval").attrInput = value;
        }

        [JsonIgnore]
        public string CollisionSize
        {
            get => DoubleCheckAttr(5, "size", "Collision size").attrInput;
            set => DoubleCheckAttr(5, "size", "Collision size").attrInput = value;
        }

        public static string InverseFromColRow(string source)
        {
            return string.Join(",", source.Split(',').Reverse());
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "self._wisys = BossWalkImageSystem(self)\n"
                + sp + "self._wisys:SetImage(\"" + GetPath(0) + "\"," + InverseFromColRow(Macrolize(1))
                + ",{" + Macrolize(2) + "},{" + Macrolize(3) + "}," + Macrolize(4) + "," + Macrolize(5) + ")\n";
        }

        public override string ToString()
        {
            return "Set walk image of current object from \"" + NonMacrolize(0) + "\" and its behaviour to that of boss";
        }

        protected override void AddCompileSettings()
        {
            string sk = parentWorkSpace.CompileProcess.archiveSpace + Path.GetFileName(NonMacrolize(0));
            if (!string.IsNullOrEmpty(NonMacrolize(0))
                && !parentWorkSpace.CompileProcess.resourceFilePath.ContainsKey(sk))
                parentWorkSpace.CompileProcess.resourceFilePath.Add(sk, NonMacrolize(0));
        }

        public override object Clone()
        {
            var n = new SetBossWalkImageSystem(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(2, this);
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

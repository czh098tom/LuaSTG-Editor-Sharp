using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Boss
{
    [Serializable, NodeIcon("bosscreate.png")]
    [RequireAncestor(typeof(Stage.Stage), typeof(Data.Function))]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class CreateBoss : FixedAttributeTreeNode
    {
        [JsonConstructor]
        private CreateBoss() : base() { }

        public CreateBoss(DocumentData workSpaceData)
            : this(workSpaceData, "", "true")
        { }

        public CreateBoss(DocumentData workSpaceData, string name, string wait)
            : base(workSpaceData)
        {
            Name = name;
            Wait = wait;
            /*
            attributes.Add(new AttrItem("Name", name, this, "bossDef"));
            attributes.Add(new AttrItem("Wait", wait, this, "bool"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0, "bossDef").attrInput;
            set => DoubleCheckAttr(0, "bossDef").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Wait
        {
            get => DoubleCheckAttr(1, "bool").attrInput;
            set => DoubleCheckAttr(1, "bool").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            yield return sp + "local _boss_wait=" + Macrolize(1) + "\n"
                       + sp + "local _ref=New(_editor_class[" + Macrolize(0) + "],_editor_class[" + Macrolize(0) + "].cards)\n"
                       + sp + "last=_ref\n"
                       + sp + "if _boss_wait then while IsValid(_ref) do task.Wait() end end\n";
        }

        public override IEnumerable<Tuple<int,TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(4, this);
        }

        public override string ToString()
        {
            return "Create boss " + NonMacrolize(0) + (NonMacrolize(1)=="true"?", wait":"");
        }

        public override object Clone()
        {
            var n = new CreateBoss(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override MetaInfo GetReferredMeta()
        {
            AttrItem original = attributes[0];
            AbstractMetaData metaData = original.Parent.parentWorkSpace.Meta;
            return (metaData.aggregatableMetas[(int)MetaType.Boss]
                .FindOfName(original.Parent.NonMacrolize(0).Trim('\"'))) as MetaInfo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Laser
{
    [Serializable, NodeIcon("lasercreate.png")]
    [RequireAncestor(typeof(CodeAlikeTypes))]
    [CreateInvoke(0), RCInvoke(2)]
    public class CreateLaser : ObjectCreatorNode
    {
        [JsonConstructor]
        private CreateLaser() : base() { }

        public CreateLaser(DocumentData workSpaceData)
            : this(workSpaceData, "", "self.x,self.y", "")
        { }

        public CreateLaser(DocumentData workSpaceData, string name, string pos, string param)
            : base(workSpaceData)
        {
            Name = name;
            Position = pos;
            Parameters = param;
            /*
            attributes.Add(new AttrItem("Name", name, this, "laserDef"));
            attributes.Add(new AttrItem("Position", pos, this, "position"));
            attributes.Add(new AttrItem("Parameters", param, this, "laserParam"));
            */
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0, "laserDef").attrInput;
            set => DoubleCheckAttr(0, "laserDef").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Position
        {
            get => DoubleCheckAttr(1, "position").attrInput;
            set => DoubleCheckAttr(1, "position").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string Parameters
        {
            get => DoubleCheckAttr(2, "laserParam").attrInput;
            set => DoubleCheckAttr(2, "laserParam").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string p = Macrolize(2);
            yield return sp + "last=New(_editor_class[" + Macrolize(0) + "]," + Macrolize(1)
                + (string.IsNullOrEmpty(p) ? "" : "," + p) + ")\n";
            foreach (var item in ParseChildrenIfValid(spacing)) yield return item;
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            yield return new Tuple<int, TreeNodeBase>(1, this);
            foreach (var item in GetLinesForChildrenIfValid()) yield return item;
        }

        public override string ToString()
        {
            return "Create laser of type " + NonMacrolize(0) + " at (" + NonMacrolize(1) + ") with parameter " + NonMacrolize(2);
        }

        public override object Clone()
        {
            var n = new CreateLaser(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override MetaInfo GetReferredMeta()
        {
            AttrItem original = attributes[0];
            AbstractMetaData metaData = original.Parent.parentWorkSpace.Meta;
            return (metaData.aggregatableMetas[(int)MetaType.Laser]
                .FindOfName(original.Parent.NonMacrolize(0).Trim('\"'))) as MetaInfo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using LuaSTGEditorSharp.EditorData.Node.General;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Project
{
    [Serializable, NodeIcon("folder.png")]
    [CannotDelete, CannotBan]
    public class ProjectRoot : FixedAttributeTreeNode
    { 
        [JsonConstructor]
        private ProjectRoot() : base()
        {
            //activated = true;
        }

        public ProjectRoot(DocumentData workSpaceData)
            : this(workSpaceData, "") { }

        public ProjectRoot(DocumentData workSpaceData, string name) 
            : base(workSpaceData)
        {
            Name = name;
            //attributes.Add(new AttrItem("Name", name, this));
            //activated = true;
        }

        [JsonIgnore, NodeAttribute]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        public override IEnumerable<Tuple<int, TreeNodeBase>> GetLines()
        {
            foreach(Tuple<int,TreeNodeBase> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override string ToString()
        {
            return attributes[0].AttrInput;
        }

        public override object Clone()
        {
            var n = new Folder(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}

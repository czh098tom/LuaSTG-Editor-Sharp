using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LuaSTGEditorSharp.EditorData.Node.Task;
using LuaSTGEditorSharp.EditorData.Interfaces;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public class TaskDefineMetaInfo : MetaInfo, IComparable<TaskDefineMetaInfo>
    {
        public TaskDefineMetaInfo(TaskDefine target) : base(target) { }

        public override string Name => Lua.StringParser.ParseLua(target.PreferredNonMacrolize(0, nameof(TaskDefine.Name)));

        public override string ScrString => "Name: " + Name + "\nParameters: " + GetParam();

        public override string Difficulty => "";

        public override string FullName => Name;

        public override string GetParam()
        {
            return target.PreferredNonMacrolize(1, nameof(TaskDefine.Parameter));
        }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.Task].Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.Task].Remove(meta);
        }

        public override MetaModel GetFullMetaModel()
        {
            MetaModel parent = new MetaModel
            {
                Icon = "/LuaSTGNode.Legacy;component/images/16x16/taskdefine.png",
                Text = Name
            };
            string[] childs = GetParamList();
            MetaModel prop;
            foreach (string s in childs)
            {
                prop = new MetaModel
                {
                    Icon = "/LuaSTGEditorSharp.Core;component/images/16x16/properties.png",
                    Text = s
                };
                parent.Children.Add(prop);
            }
            return parent;
        }

        public override MetaModel GetSimpleMetaModel()
        {
            return new MetaModel
            {
                Result = "\"" + FullName + "\"",
                Text = ScrString,
                Difficulty = Difficulty,
                FullName = FullName,
                Param = GetParam(),
                Icon = "/LuaSTGNode.Legacy;component/images/16x16/taskdefine.png"
            };
        }

        public int CompareTo(TaskDefineMetaInfo other)
        {
            return Name.CompareTo(other.Name);
        }

        public string[] GetParamList()
        {
            string s = GetParam();
            if (!string.IsNullOrEmpty(s))
            {
                return s.Split(',');
            }
            else
            {
                return new string[] { };
            }
        }
    }
}

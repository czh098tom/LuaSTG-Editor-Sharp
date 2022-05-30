using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Node.Advanced;
using LuaSTGEditorSharp.Windows;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public class UserDefinedNodeDefaultValueMetaInfo : MetaInfo, IComparable<UserDefinedNodeMetaInfo>
    {
        public override string Name
        {
            get => target.PreferredNonMacrolize(1, "Name");
        }

        public override string FullName
        {
            get => target.PreferredNonMacrolize(1, "Name");
        }

        public override string Difficulty => "";

        public override string ScrString => Name;

        public UserDefinedNodeDefaultValueMetaInfo(UserDefinedNode target) : base(target) { }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.UserDefinedDefaultValueData.Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.UserDefinedDefaultValueData.Remove(meta);
        }

        public override MetaModel GetFullMetaModel()
        {
            MetaModel metaModel = new MetaModel
            {
                Icon = "/LuaSTGEditorSharp.Core;component/images/16x16/userdefinednode.png",
                Text = target.PreferredNonMacrolize(0, "Source type") + ":" + target.PreferredNonMacrolize(1, "Name")
            };
            return metaModel;
        }

        public int CompareTo(UserDefinedNodeMetaInfo other)
        {
            return Name.CompareTo(other.Name);
        }

        public override MetaModel GetSimpleMetaModel()
        {
            string s = GetParam();
            string[] paramStrs = s.Split('\n');
            string display = "";
            bool first = true;
            //resolve exceed '\n'
            for (int i = 0; i < paramStrs.Count() - 1; i += 3)
            {
                if (!first) display += ",";
                display += paramStrs[i];
                first = false;
            }
            return new MetaModel
            {
                Result = Name,
                Text = Name + " (" + display + ")",
                FullName = FullName,
                Icon = "/LuaSTGEditorSharp.Core;component/images/16x16/userdefinednode.png",
                ExInfo1 = target.PreferredNonMacrolize(0, "Source type"),
                Param = s
            };
        }
    }
}

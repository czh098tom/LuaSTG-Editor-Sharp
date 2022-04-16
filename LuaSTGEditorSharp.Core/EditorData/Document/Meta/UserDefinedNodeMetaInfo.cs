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
    public class UserDefinedNodeMetaInfo : MetaInfo, IComparable<UserDefinedNodeMetaInfo>
    {
        public override string Name
        {
            get => target.PreferredNonMacrolize(0, "Name");
        }

        public override string FullName
        {
            get => target.PreferredNonMacrolize(0, "Name");
        }

        public override string Difficulty => "";

        public override string GetParam()
        {
            string s = "";
            if (!int.TryParse(target.PreferredNonMacrolize(3, "Number of prop"), out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 4; i <= nAttr * 3 + 3; i += 3)
            {
                if (target.PreferredNonMacrolize(i, null) != "")
                {
                    s += target.PreferredNonMacrolize(i, null) + "\n" 
                        + target.PreferredNonMacrolize(i + 1, null) + "\n" 
                        + target.PreferredNonMacrolize(i + 2, null) + "\n";
                }
            }
            return s;
        }

        public override string ScrString => Name;
        
        public UserDefinedNodeMetaInfo(UserDefinedNode target) : base(target) { }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.UserDefinedData.Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.UserDefinedData.Remove(meta);
        }

        public override MetaModel GetFullMetaModel()
        {
            MetaModel metaModel = new MetaModel
            {
                Icon = "/LuaSTGEditorSharp.Core;component/images/16x16/userdefinednode.png",
                Text = Name
            };
            MetaModel sub;
            if (!int.TryParse(target.PreferredNonMacrolize(3, "Number of prop"), out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 4; i <= nAttr * 3 + 3; i += 3)
            {
                if (target.PreferredNonMacrolize(i, null) != "")
                {
                    sub = new MetaModel
                    {
                        Icon = "/LuaSTGEditorSharp.Core;component/images/16x16/properties.png",
                        Text = target.PreferredNonMacrolize(i, null)
                    };
                    metaModel.Children.Add(sub);
                }
            }
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
                ExInfo1 = target.PreferredNonMacrolize(1, "Head parse rule"),
                ExInfo2 = target.PreferredNonMacrolize(2, "Tail parse rule"),
                Param = s
            };
        }
    }
}

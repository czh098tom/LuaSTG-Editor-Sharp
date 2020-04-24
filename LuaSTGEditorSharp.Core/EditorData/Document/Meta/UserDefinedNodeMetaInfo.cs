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
            get => target.attributes[0].AttrInput;
        }

        public override string FullName
        {
            get => target.attributes[0].AttrInput;
        }

        public override string Difficulty => "";

        

        public override string GetParam()
        {
            string s = "";
            if (!int.TryParse(target.attributes[3].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 4; i <= nAttr * 3 + 3; i += 3)
            {
                if (target.attributes[i].AttrInput != "")
                {
                    s += target.attributes[i].AttrInput + "\n" 
                        + target.attributes[i + 1].AttrInput + "\n" 
                        + target.attributes[i + 2].AttrInput + "\n";
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
                Icon = "/LuaSTGEditorSharp;component/images/16x16/userdefinednode.png",
                Text = Name
            };
            MetaModel sub;
            if (!int.TryParse(target.attributes[3].AttrInput, out int nAttr)) nAttr = 0;
            nAttr = nAttr > AppConstants.mxUAttr ? AppConstants.mxUAttr : nAttr;
            nAttr = nAttr < 0 ? 0 : nAttr;
            for (int i = 4; i <= nAttr * 3 + 3; i += 3)
            {
                if (target.attributes[i].AttrInput != "")
                {
                    sub = new MetaModel
                    {
                        Icon = "/LuaSTGEditorSharp;component/images/16x16/properties.png",
                        Text = target.attributes[i].AttrInput
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
                Icon = "/LuaSTGEditorSharp;component/images/16x16/userdefinednode.png",
                ExInfo1 = target.attributes[1].AttrInput,
                ExInfo2 = target.attributes[2].AttrInput,
                Param = s
            };
        }
    }
}

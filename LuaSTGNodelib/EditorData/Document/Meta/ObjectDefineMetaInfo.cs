using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Node.Bullet;
using LuaSTGEditorSharp.EditorData.Node.Object;
using LuaSTGEditorSharp.Windows;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public class ObjectDefineMetaInfo : MetaInfo, IComparable<ObjectDefineMetaInfo>
    {
        ObjectInit Init { get; set; }

        public ObjectDefineMetaInfo(ObjectDefine target) : base(target) { }

        private void TryChild()
        {
            foreach (TreeNode t in this.target.GetLogicalChildren())
            {
                if (t is ObjectInit) Init = t as ObjectInit;
            }
        }

        public override string Name
        {
            get => Lua.StringParser.ParseLua(target.attributes[0].AttrInput);
        }

        public override string Difficulty
        {
            get => Lua.StringParser.ParseLua(target.attributes[1].AttrInput);
        }

        public string Params
        {
            get
            {
                TryChild();
                return Init.attributes[0].AttrInput;
            }
        }

        public string[] GetParamList()
        {
            TryChild();
            string s = Init.attributes[0].AttrInput;
            if(!string.IsNullOrEmpty(s))
            {
                return s.Split(',');
            }
            else
            {
                return new string[] { };
            }
        }

        public string[] GetCallBackFunc()
        {
            return (from TreeNode t 
                    in target.GetLogicalChildren()
                    where t is CallBackFunc
                    select t.attributes[0].AttrInput).ToArray();
        }

        public int CompareTo(ObjectDefineMetaInfo other)
        {
            return Name.CompareTo(other.Name);
        }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.Object].Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.Object].Remove(meta);
        }

        public override string FullName
        {
            get
            {
                string d = Difficulty;
                d = d == "All" ? "" : ":" + d;
                return Name + d;
            }
        }

        public override string ScrString
        {
            get => "Name: " + Name + "\nDifficulty: " + Difficulty + "\nParameters: " + Params;
        }

        public override MetaModel GetFullMetaModel()
        {
            MetaModel parent = new MetaModel
            {
                Icon = "/LuaSTGPlusNodeLib;component/images/16x16/objectdefine.png",
                Text = Name
            };
            MetaModel child = new MetaModel()
            {
                Icon = "/LuaSTGPlusNodeLib;component/images/16x16/callbackfunc.png",
                Text = "init"
            };
            parent.Children.Add(child);
            string[] childs = GetParamList();
            MetaModel prop;
            foreach (string s in childs)
            {
                prop = new MetaModel
                {
                    Icon = "/LuaSTGPlusNodeLib;component/images/16x16/properties.png",
                    Text = s
                };
                child.Children.Add(prop);
            }
            childs = GetCallBackFunc();
            foreach (string s in childs)
            {
                child = new MetaModel
                {
                    Icon = "/LuaSTGPlusNodeLib;component/images/16x16/callbackfunc.png",
                    Text = s
                };
                parent.Children.Add(child);
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
                Param = Params,
                Icon = "/LuaSTGPlusNodeLib;component/images/16x16/objectdefine.png"
            };
        }

        public override string GetParam()
        {
            return Params;
        }
    }
}

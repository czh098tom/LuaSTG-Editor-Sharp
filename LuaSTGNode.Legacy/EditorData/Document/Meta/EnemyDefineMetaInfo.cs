using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Node.Enemy;
using LuaSTGEditorSharp.EditorData.Node.Object;
using LuaSTGEditorSharp.Windows;

namespace LuaSTGEditorSharp.EditorData.Document.Meta
{
    public class EnemyDefineMetaInfo : MetaInfoWithDifficulty, IComparable<EnemyDefineMetaInfo>
    {
        EnemyInit Init { get; set; }

        public EnemyDefineMetaInfo(EnemyDefine target) : base(target) { }

        private void TryChild()
        {
            foreach (TreeNodeBase t in this.target.GetLogicalChildren())
            {
                if (t is EnemyInit) Init = t as EnemyInit;
            }
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
            return (from TreeNodeBase t
                    in target.GetLogicalChildren()
                    where t is CallBackFunc
                    select t.PreferredNonMacrolize(0, "Event type")).ToArray();
        }

        public int CompareTo(EnemyDefineMetaInfo other)
        {
            return Name.CompareTo(other.Name);
        }

        public override void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.Enemy].Add(meta);
        }

        public override void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData)
        {
            documentMetaData.aggregatableMetas[(int)MetaType.Enemy].Remove(meta);
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
                Icon = "/LuaSTGNode.Legacy;component/images/16x16/enemydefine.png",
                Text = Name
            };
            MetaModel child = new MetaModel()
            {
                Icon = "/LuaSTGNode.Legacy;component/images/16x16/callbackfunc.png",
                Text = "init"
            };
            parent.Children.Add(child);
            string[] childs = GetParamList();
            MetaModel prop;
            foreach (string s in childs)
            {
                prop = new MetaModel
                {
                    Icon = "/LuaSTGEditorSharp.Core;component/images/16x16/properties.png",
                    Text = s
                };
                child.Children.Add(prop);
            }
            childs = GetCallBackFunc();
            foreach (string s in childs)
            {
                child = new MetaModel
                {
                    Icon = "/LuaSTGNode.Legacy;component/images/16x16/callbackfunc.png",
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
                Icon = "/LuaSTGNode.Legacy;component/images/16x16/enemydefine.png"
            };
        }

        public override string GetParam()
        {
            return Params;
        }
    }
}

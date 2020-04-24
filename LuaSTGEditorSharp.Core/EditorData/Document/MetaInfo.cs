using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Windows;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document.Meta;

namespace LuaSTGEditorSharp.EditorData.Document
{
    public abstract class MetaInfo : IAggregatableMeta
    {
        public TreeNode target;
        public MetaInfo(TreeNode target) { this.target = target; }

        public abstract string Name { get; }
        public abstract string ScrString { get; }
        public abstract string Difficulty { get; }
        public abstract string FullName { get; }

        public string GetDifficulty()
        {
            return Difficulty;
        }

        public abstract void Create(IAggregatableMeta meta, MetaDataEntity documentMetaData);
        public abstract void Remove(IAggregatableMeta meta, MetaDataEntity documentMetaData);
        public abstract MetaModel GetFullMetaModel();
        public abstract MetaModel GetSimpleMetaModel();

        public string GetFullName()
        {
            return FullName;
        }

        public virtual string GetParam()
        {
            return "";
        }

        public virtual string GetExInfo()
        {
            return "";
        }
    }
}

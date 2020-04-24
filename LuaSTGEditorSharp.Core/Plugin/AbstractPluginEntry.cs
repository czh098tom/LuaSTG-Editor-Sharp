using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Interfaces;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.Execution;
using System.Windows.Media.Imaging;

namespace LuaSTGEditorSharp.Plugin
{
    public abstract class AbstractPluginEntry
    {
        public abstract IInputWindowSelectorRegister GetInputWindowSelectorRegister();
        public abstract AbstractMetaData GetMetaData();
        public abstract AbstractMetaData GetMetaData(IMetaInfoCollection[] meta);
        public abstract AbstractToolbox GetToolbox(IMainWindow mw);
        public abstract IViewDefinition GetViewDefinitionWindow(DocumentData document);
        protected AbstractNodeTypeCache nodeTypeCache;
        public AbstractNodeTypeCache NodeTypeCache { get => nodeTypeCache; }
        protected LSTGExecution execution;
        public LSTGExecution Execution { get => execution; }
        public abstract Type[] StageNodeType { get; }
        public abstract Type[] BossSCNodeType { get; }
        public abstract int MetaInfoCollectionTypeCount { get; }
        public abstract int[][] MetaInfoCollectionWatchDict { get; }
        public virtual PluginTool[] PluginTools { get => new PluginTool[] { }; }

        public abstract string TargetLSTGVersion { get; }

        public bool MatchStageNodeTypes(Type tov)
        {
            foreach(Type t in StageNodeType)
            {
                if (t == tov)
                {
                    return true;
                }
            }
            return false;
        }

        public bool MatchBossSCNodeTypes(Type tov)
        {
            foreach (Type t in BossSCNodeType)
            {
                if (t == tov)
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<KeyValuePair<string, BitmapImage>> GetNodeImageResources()
        {
            foreach (KeyValuePair<Type, TypeCacheData> kvp in nodeTypeCache.NodeTypeInfo)
            {
                string s = kvp.Value.icon;
                yield return new KeyValuePair<string, BitmapImage>(s, new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute)));
            }
        }
    }
}

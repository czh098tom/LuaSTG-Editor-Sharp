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
    /// <summary>
    /// Base class for dll library entry
    /// </summary>
    public abstract class AbstractPluginEntry
    {
        /// <summary>
        /// Get a registration class for registration of input windows.
        /// </summary>
        /// <returns> An implementation of <see cref="IInputWindowSelectorRegister"/> which registers the input window information. </returns>
        public abstract IInputWindowSelectorRegister GetInputWindowSelectorRegister();
        /// <summary>
        /// Get a "serialized" version of meta data.
        /// </summary>
        /// <returns> An implementation of <see cref="AbstractMetaData"/> which contains serialized meta data. </returns>
        public abstract AbstractMetaData GetMetaData();
        /// <summary>
        /// Get a "serialized" version of meta data and initialize it using an array.
        /// </summary>
        /// <param name="meta"> Source of meta data. </param>
        /// <returns> An implementation of <see cref="AbstractMetaData"/> which contains serialized meta data in the input array. </returns>
        public abstract AbstractMetaData GetMetaData(IMetaInfoCollection[] meta);
        /// <summary>
        /// Get a toolbox object for representing toolbox in main window.
        /// </summary>
        /// <param name="mw"> An implementation of <see cref="IMainWindow"/> which contains implementation of insering nodes. </param>
        /// <returns> An implementation of <see cref="AbstractToolbox"/> which contains toolbox definition. </returns>
        public abstract AbstractToolbox GetToolbox(IMainWindow mw);
        /// <summary>
        /// Get a view definition node for definition viewing in workspace.
        /// </summary>
        /// <param name="document"> A document which is asked to view. </param>
        /// <returns> An implementation of <see cref="IViewDefinition"/> which can be shown.</returns>
        public abstract IViewDefinition GetViewDefinitionWindow(DocumentData document);
        /// <summary>
        /// Store the node type cache object used in reflection. Must be initialized in constructor in child class implementation.
        /// </summary>
        public AbstractNodeTypeCache NodeTypeCache { get; protected set; }
        /// <summary>
        /// Store the LuaSTG execution object. Must be initialized in constructor in child class implementation.
        /// </summary>
        public LSTGExecution Execution { get; protected set; }
        /// <summary>
        /// Get the types of stage nodes which can be used in searching for stage debugging.
        /// </summary>
        public abstract Type[] StageNodeType { get; }
        /// <summary>
        /// Get the types of boss spell card nodes which can be used in searching for stage debugging.
        /// </summary>
        public abstract Type[] BossSCNodeType { get; }
        /// <summary>
        /// Gets the count of MetaInfo types.
        /// </summary>
        public abstract int MetaInfoCollectionTypeCount { get; }
        /// <summary>
        /// Get the nested array used in detecting identifier conflicts.
        /// <para/>
        /// For example, <code>{{1,2},{1,3}}</code> means identifier names in type 1 and 2 should be different, 1 and 3
        /// should be different.
        /// </summary>
        public abstract int[][] MetaInfoCollectionWatchDict { get; }
        /// <summary>
        /// Get an array of tools used in menu.
        /// </summary>
        public virtual PluginTool[] PluginTools { get => new PluginTool[] { }; }
        /// <summary>
        /// When overrided, indicating another assembly to hold <see cref="TreeNode"/> definitions.
        /// </summary>
        public virtual string NodeAssemblyName { get => ""; }
        /// <summary>
        /// Get the target LuaSTG version hint in settings window.
        /// </summary>
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
            foreach (KeyValuePair<Type, TypeCacheData> kvp in NodeTypeCache.NodeTypeInfo)
            {
                string s = kvp.Value.icon;
                yield return new KeyValuePair<string, BitmapImage>(s, new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute)));
            }
        }
    }
}

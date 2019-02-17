using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Interfaces;

namespace LuaSTGEditorSharp.Plugin.DefaultNullPlugin
{
    internal class DefaultPluginEntry : AbstractPluginEntry
    {
        public DefaultPluginEntry() : base()
        {
            nodeTypeCache = new DefaultNodeTypeCache();
        }

        public override Type[] StageNodeType => new Type[0];

        public override Type[] BossSCNodeType => new Type[0];

        public override int MetaInfoCollectionTypeCount => 2;

        public override int[][] MetaInfoCollectionWatchDict => new int[][] { new int[] { 0 }, new int[] { 1 } };

        public override IInputWindowSelector GetInputWindowSelector()
        {
            return new DefaultInputWindowSelector();
        }

        public override AbstractMetaData GetMetaData()
        {
            return new DefaultMetaData();
        }

        public override AbstractMetaData GetMetaData(IMetaInfoCollection[] meta)
        {
            return new DefaultMetaData(meta);
        }

        public override AbstractToolbox GetToolbox(MainWindow mw)
        {
            return new DefaultToolBox(mw);
        }

        public override IViewDefinition GetViewDefinitionWindow(DocumentData document)
        {
            return new DefaultViewDefinition(document);
        }
    }
}

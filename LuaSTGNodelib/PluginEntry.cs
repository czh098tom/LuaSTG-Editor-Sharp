using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.Windows;
using LuaSTGEditorSharp.EditorData.Node;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Interfaces;

namespace LuaSTGEditorSharp
{
    public class PluginEntry : AbstractPluginEntry
    {
        public PluginEntry() : base()
        {
            nodeTypeCache = new NodeTypeCache();
        }

        public override IInputWindowSelector GetInputWindowSelector()
        {
            return new Windows.Input.InputWindowSelector();
        }

        public override AbstractToolbox GetToolbox(MainWindow mw)
        {
            return new PluginToolbox(mw);
        }

        public override AbstractMetaData GetMetaData()
        {
            return new MetaData();
        }

        public override AbstractMetaData GetMetaData(IMetaInfoCollection[] meta)
        {
            return new MetaData(meta);
        }

        public override IViewDefinition GetViewDefinitionWindow(DocumentData document)
        {
            return new ViewDefinition(document);
        }

        public override Type[] StageNodeType
        {
            get => new Type[] { typeof(EditorData.Node.Stage.Stage) };
        }

        public override Type[] BossSCNodeType
        {
            get => new Type[] { typeof(EditorData.Node.Boss.BossSpellCard) };
        }

        public override int[][] MetaInfoCollectionWatchDict
            => new int[][]{
                new int[]{ 1 },
                new int[]{ 2, 3, 6, 7, 8 },
                new int[]{ 4 },
                new int[]{ 5 }
            };

        public override int MetaInfoCollectionTypeCount { get => (int)MetaType.__max; }
    }
}

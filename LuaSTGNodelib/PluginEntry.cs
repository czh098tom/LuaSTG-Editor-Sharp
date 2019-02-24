using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.Windows;
using LuaSTGEditorSharp.EditorData;
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
                new int[]{ (int)MetaType.userDefined },
                new int[]{ (int)MetaType.stageGroup },
                new int[]{ (int)MetaType.Boss, (int)MetaType.Bullet, (int)MetaType.BossBG
                    , (int)MetaType.Laser, (int)MetaType.BentLaser },
                new int[]{ (int)MetaType.ImageLoad },
                new int[]{ (int)MetaType.BGMLoad }
            };

        public override int MetaInfoCollectionTypeCount { get => (int)MetaType.__max; }
    }
}

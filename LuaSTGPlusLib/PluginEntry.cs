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
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Interfaces;

namespace LuaSTGEditorSharp
{
    public class PluginEntry : AbstractPluginEntry
    {
        public PluginEntry() : base()
        {
            nodeTypeCache = new NodeTypeCache();
            execution = new LSTGEXPlusExecution();
        }

        public override string NodeAssemblyName => "LuaSTGNode.Legacy";

        public override IInputWindowSelectorRegister GetInputWindowSelectorRegister()
        {
            return new Windows.Input.InputWindowSelectorRegister();
        }

        public override AbstractToolbox GetToolbox(IMainWindow mw)
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
                new int[]{ (int)MetaType.UserDefined },
                new int[]{ (int)MetaType.StageGroup },
                new int[]{ (int)MetaType.Boss, (int)MetaType.Bullet, (int)MetaType.BossBG
                    , (int)MetaType.Laser, (int)MetaType.BentLaser, (int)MetaType.Object },
                new int[]{ (int)MetaType.Task },
                new int[]{ (int)MetaType.ImageLoad },
                new int[]{ (int)MetaType.ImageGroupLoad },
                new int[]{ (int)MetaType.BGMLoad },
                new int[]{ (int)MetaType.FXLoad }
            };

        public override int MetaInfoCollectionTypeCount { get => (int)MetaType.__max; }

        public override string TargetLSTGVersion => "LuaSTG ex+ 0.81b/c, OLC ver 5 and 0.82a/b";
    }
}

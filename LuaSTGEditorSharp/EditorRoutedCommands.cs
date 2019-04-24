using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LuaSTGEditorSharp
{
    public class EditorRoutedCommands
    {
        #region Extra edit commands
        public static RoutedUICommand FoldRegion { get; }
        public static RoutedUICommand UnfoldAsRegion { get; }
        public static RoutedUICommand GoToLineX { get; }
        public static RoutedUICommand SwitchBan { get; }
        #endregion
        #region export
        public static RoutedUICommand ViewCode { get; }
        public static RoutedUICommand ExportCode { get; }
        public static RoutedUICommand RunProject { get; }
        public static RoutedUICommand SCDebug { get; }
        public static RoutedUICommand StageDebug { get; }
        public static RoutedUICommand ExportZip { get; }
        #endregion
        #region insert
        public static RoutedUICommand InsertPreset { get; }
        public static RoutedUICommand SavePreset { get; }
        public static RoutedUICommand RefreshPreset { get; }
        #endregion
        #region insertFac
        public static RoutedUICommand SwitchBefore { get; }
        public static RoutedUICommand SwitchAfter { get; }
        public static RoutedUICommand SwitchChild { get; }
        public static RoutedUICommand SwitchParent { get; }
        #endregion
        #region view
        public static RoutedUICommand ViewFileFolder { get; }
        public static RoutedUICommand ViewModFolder { get; }
        public static RoutedUICommand ViewDefinition { get; }
        #endregion
        public static RoutedUICommand Settings { get; }
        public static RoutedUICommand AboutNode { get; }

        #region used to take parameters
        public static RoutedUICommand AdjustProp { get; }
        #endregion

        static EditorRoutedCommands()
        {
            InputGestureCollection inputs;
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.G, ModifierKeys.Control, "Ctrl+G")
            };
            FoldRegion = new RoutedUICommand("Fold Region", "FoldRegion", typeof(EditorRoutedCommands), inputs);
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.U, ModifierKeys.Control, "Ctrl+U")
            };
            UnfoldAsRegion = new RoutedUICommand("Unfold as Region", "UnfoldAsRegion", typeof(EditorRoutedCommands), inputs);
            GoToLineX = new RoutedUICommand("Go to line X", "GoToLineX", typeof(EditorRoutedCommands));
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.F4)
            };
            SwitchBan = new RoutedUICommand("Ban", "Ban", typeof(EditorRoutedCommands), inputs);
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.F2)
            };
            ViewCode = new RoutedUICommand("View Code", "ViewCode", typeof(EditorRoutedCommands), inputs);
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.F5)
            };
            RunProject = new RoutedUICommand("Run Project", "RunProject", typeof(EditorRoutedCommands), inputs);
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.F6)
            };
            SCDebug = new RoutedUICommand("Debug SpellCard", "DebugSC", typeof(EditorRoutedCommands), inputs);
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.F7)
            };
            StageDebug = new RoutedUICommand("Debug Stage", "DebugStage", typeof(EditorRoutedCommands), inputs);
            ExportCode = new RoutedUICommand("Export Code", "ExportCode", typeof(EditorRoutedCommands));
            ExportZip = new RoutedUICommand("Export Zip", "ExportZip", typeof(EditorRoutedCommands));
            InsertPreset = new RoutedUICommand("Insert Preset", "InsertPreset", typeof(EditorRoutedCommands));
            SavePreset = new RoutedUICommand("Save Preset", "SavePreset", typeof(EditorRoutedCommands));
            RefreshPreset = new RoutedUICommand("Refresh Preset", "RefreshPreset", typeof(EditorRoutedCommands));
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.Up, ModifierKeys.Alt, "Alt+Up")
            };
            SwitchBefore = new RoutedUICommand("Switch Before State", "SwitchBefore", typeof(EditorRoutedCommands), inputs);
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.Down, ModifierKeys.Alt, "Alt+Down")
            };
            SwitchAfter = new RoutedUICommand("Switch After State", "SwitchAfter", typeof(EditorRoutedCommands), inputs);
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.Right, ModifierKeys.Alt, "Alt+Right")
            };
            SwitchChild = new RoutedUICommand("Switch Child State", "SwitchChild", typeof(EditorRoutedCommands), inputs);
            inputs = new InputGestureCollection
            {
                new KeyGesture(Key.Left, ModifierKeys.Alt, "Alt+Left")
            };
            SwitchParent = new RoutedUICommand("Switch Parent State", "SwitchParent", typeof(EditorRoutedCommands), inputs);
            ViewFileFolder = new RoutedUICommand("View File Folder", "ViewFileFolder", typeof(EditorRoutedCommands));
            ViewModFolder = new RoutedUICommand("View Mod Folder", "ViewModFolder", typeof(EditorRoutedCommands));
            ViewDefinition = new RoutedUICommand("View Definition", "ViewDefination", typeof(EditorRoutedCommands));
            Settings = new RoutedUICommand("Settings", "Settings", typeof(EditorRoutedCommands));
            AboutNode = new RoutedUICommand("About Node", "AboutNode", typeof(EditorRoutedCommands));
            AdjustProp = new RoutedUICommand("Adjust Properties", "AdjustProperties", typeof(EditorRoutedCommands));
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace LuaSTGEditorSharp.Windows
{
    using Application = System.Windows.Application;
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window , INotifyPropertyChanged
    {
        App mainApp = Application.Current as App;

        static readonly int[] resX = { 640, 960, 1280 };
        static readonly int[] resY = { 480, 720, 960 };

        ObservableCollection<string> pluginPaths = new ObservableCollection<string>();

        private string zipExecutablePath;
        public string ZipExecutablePath
        {
            get => zipExecutablePath;
            set
            {
                zipExecutablePath = value;
                RaiseProertyChanged("ZipExecutablePath");
            }
        }

        private string luaSTGExecutablePath;
        public string LuaSTGExecutablePath
        {
            get => luaSTGExecutablePath;
            set
            {
                luaSTGExecutablePath = value;
                RaiseProertyChanged("LuaSTGExecutablePath");
            }
        }

        private string tempPath;
        public string TempPath
        {
            get => tempPath;
            set
            {
                tempPath = value;
                RaiseProertyChanged("TempPath");
            }
        }

        private int debugResolutionX;
        public int DebugResolutionX
        {
            get => debugResolutionX;
            set
            {
                debugResolutionX = value;
                RaiseProertyChanged("DebugResolutionX");
            }
        }

        private int debugResolutionY;
        public int DebugResolutionY
        {
            get => debugResolutionY;
            set
            {
                debugResolutionY = value;
                RaiseProertyChanged("DebugResolutionY");
            }
        }

        public string CombinedResolution
        {
            get => DebugResolutionX + "x" + DebugResolutionY;
            set
            {
                string[] vs = value.Split('x');
                if(vs!=null && vs.Count() > 1)
                {
                    if(int.TryParse(vs[0], out int x))
                    {
                        DebugResolutionX = x;
                    }
                    if (int.TryParse(vs[1], out int y))
                    {
                        DebugResolutionY = y;
                    }
                    RaiseProertyChanged("CombinedResolution");
                }
            }
        }

        public int IndexedReso
        {
            get
            {
                switch(DebugResolutionX)
                {
                    case 640:
                        return 0;
                    case 960:
                        return 1;
                    case 1280:
                        return 2;
                    default:
                        return -1;
                }
            }
            set
            {
                if (value != -1)
                {
                    DebugResolutionX = resX[value];
                    DebugResolutionY = resY[value];
                    RaiseProertyChanged("IndexedReso");
                }
            }
        }

        private bool debugWindowed;
        public bool DebugWindowed
        {
            get => debugWindowed;
            set
            {
                debugWindowed = value;
                RaiseProertyChanged("DebugWindowed");
            }
        }

        private bool debugCheat;
        public bool DebugCheat
        {
            get => debugCheat;
            set
            {
                debugCheat = value;
                RaiseProertyChanged("DebugCheat");
            }
        }

        private bool debugUpdateLib;
        public bool DebugUpdateLib
        {
            get => debugUpdateLib;
            set
            {
                debugUpdateLib = value;
                RaiseProertyChanged("DebugUpdateLib");
            }
        }

        private bool debugSaveProj;
        public bool DebugSaveProj
        {
            get => debugSaveProj;
            set
            {
                debugSaveProj = value;
                RaiseProertyChanged("DebugSaveProj");
            }
        }

        private bool packProj;
        public bool PackProj
        {
            get => packProj;
            set
            {
                packProj = value;
                RaiseProertyChanged("PackProj");
            }
        }

        private bool autoMoveToNew;
        public bool AutoMoveToNew
        {
            get => autoMoveToNew;
            set
            {
                autoMoveToNew = value;
                RaiseProertyChanged("AutoMoveToNew");
            }
        }

        private bool md5Check;
        public bool MD5Check
        {
            get => md5Check;
            set
            {
                md5Check = value;
                RaiseProertyChanged("MD5Check");
            }
        }

        private string authorName;
        public string AuthorName
        {
            get => authorName;
            set
            {
                authorName = value;
                RaiseProertyChanged("AuthorName");
            }
        }

        private bool batchPacking;
        public bool BatchPacking
        {
            get => batchPacking;
            set
            {
                batchPacking = value;
                RaiseProertyChanged("BatchPacking");
            }
        }

        private string pluginPath;
        public string PluginPath
        {
            get => pluginPath;
            set
            {
                pluginPath = value;
                RaiseProertyChanged("PluginPath");
            }
        }

        private bool spaceIndentation;
        public bool SpaceIndentation
        {
            get => spaceIndentation;
            set
            {
                spaceIndentation = value;
                RaiseProertyChanged("SpaceIndentation");
                RaiseProertyChanged("TabIndentation");
            }
        }

        public bool TabIndentation
        {
            get => !spaceIndentation;
            set
            {
                spaceIndentation = !value;
                RaiseProertyChanged("TabIndentation");
                RaiseProertyChanged("SpaceIndentation");
            }
        }

        private int indentationSpaceLength;
        public int IndentationSpaceLength
        {
            get => indentationSpaceLength;
            set
            {
                indentationSpaceLength = value;
                RaiseProertyChanged("IndentationSpaceLength");
            }
        }

        private bool dynamicDebugReporting;
        public bool DynamicDebugReporting
        {
            get => dynamicDebugReporting;
            set
            {
                dynamicDebugReporting = value;
                RaiseProertyChanged("DynamicDebugReporting");
            }
        }

        #region InSettings
        public string ZipExecutablePathSettings
        {
            get => mainApp.ZipExecutablePath;
            set => mainApp.ZipExecutablePath = value;
        }

        public string LuaSTGExecutablePathSettings
        {
            get => mainApp.LuaSTGExecutablePath;
            set => mainApp.LuaSTGExecutablePath = value;
        }

        public string TempPathSettings
        {
            get => mainApp.TempPath;
            set => mainApp.TempPath = value;
        }

        public int DebugResolutionXSettings
        {
            get => mainApp.DebugResolutionX;
            set => mainApp.DebugResolutionX = value;
        }

        public int DebugResolutionYSettings
        {
            get => mainApp.DebugResolutionY;
            set => mainApp.DebugResolutionY = value;
        }

        public bool DebugWindowedSettings
        {
            get => mainApp.DebugWindowed;
            set => mainApp.DebugWindowed = value;
        }

        public bool DebugCheatSettings
        {
            get => mainApp.DebugCheat;
            set => mainApp.DebugCheat = value;
        }

        public bool DebugUpdateLibSettings
        {
            get => mainApp.DebugUpdateLib;
            set => mainApp.DebugUpdateLib = value;
        }

        public bool DebugSaveProjSettings
        {
            get => mainApp.DebugSaveProj;
            set => mainApp.DebugSaveProj = value;
        }

        public bool PackProjSettings
        {
            get => mainApp.PackProj;
            set => mainApp.PackProj = value;
        }

        public bool AutoMoveToNewSettings
        {
            get => mainApp.AutoMoveToNew;
            set => mainApp.AutoMoveToNew = value;
        }

        public bool MD5CheckSettings
        {
            get => mainApp.SaveResMeta;
            set => mainApp.SaveResMeta = value;
        }

        public string AuthorNameSettings
        {
            get => mainApp.AuthorName;
            set => mainApp.AuthorName = value;
        }

        public bool BatchPackingSettings
        {
            get => mainApp.BatchPacking;
            set => mainApp.BatchPacking = value;
        }

        public string PluginPathSettings
        {
            get => mainApp.PluginPath;
            set => mainApp.PluginPath = value;
        }

        public bool DynamicDebugReportingSettings
        {
            get => mainApp.DynamicDebugReporting;
            set => mainApp.DynamicDebugReporting = value;
        }

        public bool SpaceIndentationSettings
        {
            get => mainApp.SpaceIndentation;
            set => mainApp.SpaceIndentation = value;
        }

        public int IndentationSpaceLengthSettings
        {
            get => mainApp.IndentationSpaceLength;
            set => mainApp.IndentationSpaceLength = value;
        }
        #endregion

        public string TargetVersion
        {
            get => Plugin.PluginHandler.Plugin.TargetLSTGVersion;
        }

        private void WriteSettings()
        {
            AuthorNameSettings = AuthorName;
            AutoMoveToNewSettings = AutoMoveToNew;
            BatchPackingSettings = BatchPacking;
            DebugCheatSettings = DebugCheat;
            DebugResolutionXSettings = DebugResolutionX;
            DebugResolutionYSettings = DebugResolutionY;
            DebugSaveProjSettings = DebugSaveProj;
            DebugUpdateLibSettings = DebugUpdateLib;
            DebugWindowedSettings = DebugWindowed;
            LuaSTGExecutablePathSettings = LuaSTGExecutablePath;
            MD5CheckSettings = MD5Check;
            PackProjSettings = PackProj;
            PluginPathSettings = PluginPath;
            TempPathSettings = TempPath;
            ZipExecutablePathSettings = ZipExecutablePath;
            SpaceIndentationSettings = SpaceIndentation;
            IndentationSpaceLengthSettings = IndentationSpaceLength;
        }

        private void ReadSettings()
        {
            AuthorName = AuthorNameSettings;
            AutoMoveToNew = AutoMoveToNewSettings;
            BatchPacking = BatchPackingSettings;
            DebugCheat = DebugCheatSettings;
            DebugResolutionX = DebugResolutionXSettings;
            DebugResolutionY = DebugResolutionYSettings;
            DebugSaveProj = DebugSaveProjSettings;
            DebugUpdateLib = DebugUpdateLibSettings;
            DebugWindowed = DebugWindowedSettings;
            LuaSTGExecutablePath = LuaSTGExecutablePathSettings;
            MD5Check = MD5CheckSettings;
            PackProj = PackProjSettings;
            PluginPath = PluginPathSettings;
            TempPath = TempPathSettings;
            ZipExecutablePath = ZipExecutablePathSettings;
            SpaceIndentation = SpaceIndentationSettings;
            IndentationSpaceLength = IndentationSpaceLengthSettings;
        }

        public SettingsWindow()
        {
            ReadSettings();
            InitializeComponent();

            HashSet<string> pathIgnorance = new HashSet<string>();

            try
            {
                pathIgnorance.Add("LuaSTGEditorSharp.Core.dll");
                var dependencyFiles =
                    from string s
                    in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
                    where Path.GetExtension(s) == ".dependencies"
                    select s;
                foreach (string s in dependencyFiles)
                {
                    FileStream fs = null;
                    StreamReader sr = null;
                    try
                    {
                        fs = new FileStream(s, FileMode.Open, FileAccess.Read);
                        sr = new StreamReader(fs);
                        while (!sr.EndOfStream)
                        {
                            string ig = sr.ReadLine().Trim('\r', '\n', ' ');
                            if (!pathIgnorance.Contains(ig)) pathIgnorance.Add(ig);
                        }
                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show(e.ToString());
                    }
                    finally
                    {
                        sr?.Close();
                        fs?.Close();
                    }
                }
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }

            //new List<string>(pathIgnorance).ForEach((s)=>System.Windows.MessageBox.Show(s));
            //System.Windows.MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);

            pluginPaths = new ObservableCollection<string>(
                from string s
                in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
                where Path.GetExtension(s) == ".dll" && !pathIgnorance.Contains(Path.GetFileName(s))
                select Path.GetFileName(s)
                );
            PluginList.ItemsSource = pluginPaths;
        }

        public SettingsWindow(int i) : this()
        {
            switch (i)
            {
                case 0:
                    GeneralTab.IsSelected = true;
                    break;
                case 1:
                    CompilerTab.IsSelected = true;
                    break;
                case 2:
                    DebugTab.IsSelected = true;
                    break;
                case 3:
                    EditorTab.IsSelected = true;
                    break;
                default:
                    GeneralTab.IsSelected = true;
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaiseProertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void ButtonZipExecutablePath_Click(object sender, RoutedEventArgs e)
        {
            var chooseExc = new OpenFileDialog()
            {
                Filter = "Zip Executable|7z.exe"
            };
            if (chooseExc.ShowDialog() != System.Windows.Forms.DialogResult.Cancel) 
            {
                ZipExecutablePath = chooseExc.FileName;
            }
        }

        private void ButtonLuaSTGExecutablePath_Click(object sender, RoutedEventArgs e)
        {
            var chooseExc = new OpenFileDialog()
            {
                Filter = "LuaSTG Executable|" + Plugin.PluginHandler.Plugin.Execution.ExecutableName
            };
            if (chooseExc.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                LuaSTGExecutablePath = chooseExc.FileName;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            WriteSettings();
            Properties.Settings.Default.Save();
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            WriteSettings();
            Properties.Settings.Default.Save();
        }

        private void ButtonRegisterExt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process p = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "ExtensionRegister.exe",
                        CreateNoWindow = true
                    }
                };
                p.Start();
                p.WaitForExit();
            }
            catch { }
        }
    }
}

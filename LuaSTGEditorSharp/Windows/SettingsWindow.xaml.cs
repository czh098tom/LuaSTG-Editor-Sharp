using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

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

        #region InSettings
        public string ZipExecutablePathSettings
        {
            get => mainApp.ZipExecutablePath;
            set
            {
                mainApp.ZipExecutablePath = value;
            }
        }

        public string LuaSTGExecutablePathSettings
        {
            get => mainApp.LuaSTGExecutablePath;
            set
            {
                mainApp.LuaSTGExecutablePath = value;
            }
        }

        public string TempPathSettings
        {
            get => mainApp.TempPath;
            set
            {
                mainApp.TempPath = value;
            }
        }

        public int DebugResolutionXSettings
        {
            get => mainApp.DebugResolutionX;
            set
            {
                mainApp.DebugResolutionX = value;
            }
        }

        public int DebugResolutionYSettings
        {
            get => mainApp.DebugResolutionY;
            set
            {
                mainApp.DebugResolutionY = value;
            }
        }

        public bool DebugWindowedSettings
        {
            get => mainApp.DebugWindowed;
            set
            {
                mainApp.DebugWindowed = value;
            }
        }

        public bool DebugCheatSettings
        {
            get => mainApp.DebugCheat;
            set
            {
                mainApp.DebugCheat = value;
            }
        }

        public bool DebugUpdateLibSettings
        {
            get => mainApp.DebugUpdateLib;
            set
            {
                mainApp.DebugUpdateLib = value;
            }
        }

        public bool DebugSaveProjSettings
        {
            get => mainApp.DebugSaveProj;
            set
            {
                mainApp.DebugSaveProj = value;
            }
        }

        public bool PackProjSettings
        {
            get => mainApp.PackProj;
            set
            {
                mainApp.PackProj = value;
            }
        }

        public bool AutoMoveToNewSettings
        {
            get => mainApp.AutoMoveToNew;
            set
            {
                mainApp.AutoMoveToNew = value;
            }
        }

        public bool MD5CheckSettings
        {
            get => mainApp.SaveResMeta;
            set
            {
                mainApp.SaveResMeta = value;
            }
        }

        public string AuthorNameSettings
        {
            get => mainApp.AuthorName;
            set
            {
                mainApp.AuthorName = value;
            }
        }

        public bool BatchPackingSettings
        {
            get => mainApp.BatchPacking;
            set
            {
                mainApp.BatchPacking = value;
            }
        }
        #endregion

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
            TempPathSettings = TempPath;
            ZipExecutablePathSettings = ZipExecutablePath;
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
            TempPath = TempPathSettings;
            ZipExecutablePath = ZipExecutablePathSettings;
        }

        public SettingsWindow()
        {
            ReadSettings();
            InitializeComponent();
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
                Filter = "LuaSTG Executable|LuaSTGPlus.dev.exe"
            };
            if (chooseExc.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                LuaSTGExecutablePath = chooseExc.FileName;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            WriteSettings();
            Properties.Settings.Default.Save();
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
    }
}
